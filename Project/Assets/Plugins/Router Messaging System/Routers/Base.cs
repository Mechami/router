using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;

/** \brief Namespace for Router Messaging System */
namespace RouterMessagingSystem
{
	/** \brief Router that calls basic functions only. */
	/// \todo Implement RouteMessageContinuously.
	public static class Router
	{
		private static Dictionary<RoutingEvent, RoutePointer> PointerTable = null;
		private static Dictionary<RoutingEvent, List<Route>> RouteTable = null;
		private static bool TablesExist = false;

		static Router()
		{
			Debug.Log("Router initialized!");
		}

		/** \brief Registers a new Route with the Router. */
		/// \note Prints an error if the specified Route is invalid or cannot be registered.\n
		/// \note An invalid Route contains null properties.
		/// \todo Find a cleaner way to perform error printing.
		public static void AddRoute(Route NewRoute /**< Route to be registered. */)
		{
			// bool ValidRoute = RouteIsValid(NewRoute); // Debating on whether if using NewRoute directly is too confusing or not.
			bool ValidRoute = NewRoute; // Compute if the new route is valid then cache the results here.
			TablesExist = ((ValidRoute && !TablesExist)? ConstructTables() : TablesExist);

			if (!ValidRoute)
			{
				Debug.LogError("[Router] Cannot register invalid route " + NewRoute + ".", NewRoute.Subscriber);
			}

			if (ValidRoute && RouteIsRegistered(NewRoute))
			{
				Debug.LogError("[Router] Cannot register duplicate route " + NewRoute + ".", NewRoute.Subscriber);
			}

			if (ValidRoute && !RouteIsRegistered(NewRoute))
			{
				RegisterRoute(NewRoute);
				AttachAddress(NewRoute);
			}
		}

		/** \brief Removes a Route from routing table. */
		/// \note Prints an error if the specified Route cannot be removed.
		public static void RemoveRoute(Route OldRoute /**< Route to be removed. */)
		{
			// if (TablesExist && RouteIsValid(OldRoute))
			if (TablesExist && OldRoute)
			{
				DetachAddress(OldRoute);
				DeregisterRoute(OldRoute);
			}
			else
			{
				Debug.LogError("[Router] Cannot remove route " + OldRoute + ".", OldRoute.Subscriber);
			}

			TablesExist = (TablesExist? DeconstructTables() : TablesExist);
		}

		/** \brief Flushes all Routes from the routing table. */
		/// \warning Only use this if you absolutely need to reset the entire routing table at runtime.\n
		/// \warning Every subscriber will need to re-register with the Router after the tables are flushed.\n
		/// \warning This can potentially be a slow operation, depending on the amount of subscribers that need to be re-registered.
		public static void FlushRoutes()
		{
			// Wondering if this will have any affect on performance.
			if (TablesExist)
			{
				RouteTable = null;
				PointerTable = null;
				TablesExist = false;
				Debug.LogWarning("[Router] Routing tables have been flushed!");
			}
		}

		/** \brief Returns the total amount of Routes registered with the Router. */
		/// \returns Int representing the Routes registered.
		public static int RouteCount()
		{
			if (TablesExist)
			{
				int TotalRoutes = 0;

				foreach (KeyValuePair<RoutingEvent, List<Route>> KVP in RouteTable)
				{
					TotalRoutes += KVP.Value.Count;
				}

				return TotalRoutes;
			}
			else
			{
				return 0;
			}
		}

		/** \brief Routes a message of the specified event to all subscribers. */
		public static void RouteMessage(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && KeyHasAddress(EventType))
			{
				PointerTable[EventType]();
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static void RouteMessageDescendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid(Scope) && EventIsPopulated(EventType))
			{
				List<Route> RT = RouteTable[EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.transform));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.\n
		/// Accepts a Component that is used to obtain a reference to the target GameObject.
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static void RouteMessageDescendants<T>(T Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component. */, RoutingEvent EventType /**< Type of event to send. */) where T: Component
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid<T>(Scope) && EventIsPopulated(EventType))
			{
				List<Route> RT = RouteTable[EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.transform));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.\n
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static void RouteMessageAscendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid(Scope) && EventIsPopulated(EventType))
			{
				List<Route> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.\n
		/// Accepts a Component that is used to obtain a reference to the target GameObject.
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static void RouteMessageAscendants<T>(T Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component.*/, RoutingEvent EventType /**< Type of event to send. */) where T: Component
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid<T>(Scope) && EventIsPopulated(EventType))
			{
				List<Route> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers inside the specified radius. */
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageArea(AreaMessage MessageParameters /**< Struct containing parameters for the area message. */)
		{
			CleanDeadRoutes(MessageParameters.AreaEvent);

			if (TablesExist && EventIsPopulated(MessageParameters.AreaEvent))
			{
				decimal RadiusD = new Decimal(MessageParameters.Radius);
				List<Route> RT = RouteTable[MessageParameters.AreaEvent].FindAll(x => (new Decimal(Vector3.Distance(MessageParameters.Origin, x.Subscriber.transform.position)) <= RadiusD));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers outside the specified radius. */
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageAreaInverse(AreaMessage MessageParameters /**< Struct containing parameters for the area message. */)
		{
			CleanDeadRoutes(MessageParameters.AreaEvent);

			if (TablesExist && EventIsPopulated(MessageParameters.AreaEvent))
			{
				decimal RadiusD = new Decimal(MessageParameters.Radius);
				List<Route> RT = RouteTable[MessageParameters.AreaEvent].FindAll(x => (new Decimal(Vector3.Distance(MessageParameters.Origin, x.Subscriber.transform.position)) > RadiusD));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers inside a ring specified by an inner and outer radius. */
		/// Uses the specified Vector3 as the origin point.
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageAreaBand(AreaBandMessage MessageParameters)
		{
			CleanDeadRoutes(MessageParameters.AreaEvent);

			if (TablesExist && EventIsPopulated(MessageParameters.AreaEvent))
			{
				decimal InnerRadiusD = new Decimal(MessageParameters.InnerRadius), OuterRadiusD = new Decimal(MessageParameters.OuterRadius);
				List<Route> RT = RouteTable[MessageParameters.AreaEvent].FindAll(x => { decimal Distance = new Decimal(Vector3.Distance(MessageParameters.Origin, x.Subscriber.transform.position)); return ((Distance >= InnerRadiusD) && (Distance <= OuterRadiusD)); });
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers outside a ring specified by an inner and outer radius. */
		/// Uses the specified Vector3 as the origin point.
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageInverseAreaBand(AreaBandMessage MessageParameters)
		{
			CleanDeadRoutes(MessageParameters.AreaEvent);

			if (TablesExist && EventIsPopulated(MessageParameters.AreaEvent))
			{
				decimal InnerRadiusD = new Decimal(MessageParameters.InnerRadius), OuterRadiusD = new Decimal(MessageParameters.OuterRadius);
				List<Route> RT = RouteTable[MessageParameters.AreaEvent].FindAll(x => { decimal Distance = new Decimal(Vector3.Distance(MessageParameters.Origin, x.Subscriber.transform.position)); return ((Distance < InnerRadiusD) || (Distance > OuterRadiusD)); });
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message to inactive subscribers. */
		public static void RouteMessageInactiveObjects(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && EventIsPopulated(EventType))
			{
				List<Route> RT = RouteTable[EventType].FindAll(x => !x.Subscriber.gameObject.activeInHierarchy);
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message to active subscribers. */
		public static void RouteMessageActiveObjects(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && EventIsPopulated(EventType))
			{
				List<Route> RT = RouteTable[EventType].FindAll(x => x.Subscriber.gameObject.activeInHierarchy);
				RT.ForEach(x => x.Address());
			}
		}

		/** \internal *******************/
		/** \internal Utility Functions */
		/** \internal *******************/

		/// \internal Structors

		private static bool ConstructTables()
		{
			RouteTable = new Dictionary<RoutingEvent, List<Route>>();
			PointerTable = new Dictionary<RoutingEvent, RoutePointer>();
			return true;
		}

		private static bool DeconstructTables()
		{
			RouteTable = ((RouteTable.Count > 0)? RouteTable : null);
			PointerTable = ((PointerTable.Count > 0)? PointerTable : null);
			return ((RouteTable != null) && (PointerTable != null));
		}

		/// \internal Registration Functions

		private static void RegisterRoute(Route RT)
		{
			if (!RouteTable.ContainsKey(RT.RouteEvent))
			{
				RouteTable.Add(RT.RouteEvent, new List<Route>());
			}

			RouteTable[RT.RouteEvent].Add(RT);
		}

		private static void DeregisterRoute(Route RT)
		{
			if (EventIsPopulated(RT.RouteEvent))
			{
				RouteTable[RT.RouteEvent].Remove(RT);
			}
			else
			{
				RouteTable.Remove(RT.RouteEvent);
			}
		}

		private static void AttachAddress(Route RT)
		{
			if (!PointerTable.ContainsKey(RT.RouteEvent))
			{
				PointerTable.Add(RT.RouteEvent, RT.Address);
			}
			else
			{
				PointerTable[RT.RouteEvent] = (PointerTable[RT.RouteEvent] + RT.Address);
			}
		}

		private static void DetachAddress(Route RT)
		{
			if (!KeyHasAddress(RT.RouteEvent))
			{
				PointerTable.Remove(RT.RouteEvent);
			}
			else
			{
				PointerTable[RT.RouteEvent] = (PointerTable[RT.RouteEvent] - RT.Address);
			}
		}

		/// \internal Janitorial Functions

		private static void CleanDeadRoutes(RoutingEvent EventType)
		{
			// Without the check this is supposedly at most a 3(O(n)) operation.
			// With the check this ranges from a max of 4(O(n)) to 1 O(n) operation.
			if (TablesExist && EventIsPopulated(EventType) && TableIsPolluted(EventType))
			{
				Route[] DeadRoutes = Array.FindAll(RouteTable[EventType].ToArray(), x => (x.Subscriber == null));
				Array.ForEach(DeadRoutes, x => {DeregisterRoute(x); DetachAddress(x);});
				TablesExist = (TablesExist? DeconstructTables() : TablesExist);
			}
		}

		// This check was moved to its own function because it was causing the code to be compiled into a nested if-statement.
		private static bool TableIsPolluted(RoutingEvent EventType)
		{
			return !RouteTable[EventType].TrueForAll(x => (x.Subscriber != null));
		}

		/// \internal Misc Functions

		private static bool RouteIsValid(Route RT)
		{
			return ((RT.Subscriber != null) && (RT.Address != null) && (RT.RouteEvent != RoutingEvent.Null));
		}

		private static bool RouteIsRegistered(Route RT)
		{
			return (EventIsRegistered(RT.RouteEvent) && RouteTable[RT.RouteEvent].Contains(RT));
		}

		private static bool KeyHasAddress(RoutingEvent EventType)
		{
			return (PointerTable.ContainsKey(EventType) && (PointerTable[EventType] != null));
		}

		private static bool EventIsPopulated(RoutingEvent EventType)
		{
			return (EventIsRegistered(EventType) && (RouteTable[EventType].Count >= 1));
		}

		private static bool EventIsRegistered(RoutingEvent EventType)
		{
			return (RouteTable.ContainsKey(EventType) && (RouteTable[EventType] != null));
		}

		private static bool ScopeIsValid(GameObject Scope)
		{
			return (Scope != null);
		}

		private static bool ScopeIsValid<T>(T Scope) where T: Component
		{
			return (Scope != null);
		}
	}
}