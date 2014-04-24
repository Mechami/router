using UnityEngine;
using System;
using System.Collections.Generic;

namespace RouterMessagingSystem
{
	/** \brief Router that returns R from sending messages. */
	/// \todo Redocument this.
	/// \todo Add RouteMessage(RoutingEvent, Queue)
	public static class Router<R>
	{
		private static Dictionary<RoutingEvent, RoutePointer<R>> PointerTable = null;
		private static Dictionary<RoutingEvent, List<Route<R>>> RouteTable = null;
		private static bool TablesExist = false;

		static Router()
		{
			Debug.Log("Router<" + typeof(R) + "> initialized!");
		}

		/** \brief Registers a new Route with the Router. */
		/// \warning Any Routes with null attributes will not be registered.
		public static void AddRoute(Route<R> NewRoute /**< Route to be registered. */)
		{
			TablesExist = (TablesExist? TablesExist : ConstructTables());

			if ((NewRoute.Subscriber != null) && (NewRoute.Address != null) && (NewRoute.RouteEvent != RoutingEvent.Null))
			{
				RegisterRoute(NewRoute);
				AttachAddress(NewRoute);
			}
		}

		/** \brief Removes a Route from routing table. */
		public static void RemoveRoute(Route<R> OldRoute /**< Route to be removed. */)
		{
			if (TablesExist)
			{
				DeregisterRoute(OldRoute);
				DetachAddress(OldRoute);
			}

			TablesExist = (TablesExist? DeconstructTables() : TablesExist);
		}

		/** \brief Flushes all Routes from the routing table. */
		/// \warning Only use this if you absolutely need to reset the entire routing table at runtime.\n
		/// \warning Every subscriber will need to re-register with the Router after the tables are flushed.\n
		/// \warning This is a relatively slow operation, depending on the amount of subscribers that need to be re-added.
		public static void FlushRoutes()
		{
			RouteTable = null;
			PointerTable = null;
			TablesExist = false;
		}

		/** \brief Routes a message of the specified event to all subscribers and returns their values. */
		/// \returns Returns a List<R> containing all return data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		public static List<R> RouteMessage(RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && KeyHasValue(EventType) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				Delegate[] RPL = PointerTable[EventType].GetInvocationList();
				R[] Results = new R[RPL.Length];

				for (int i = 0; i < RPL.Length; i++)
				{
					Results[i] = (RPL[i] as RoutePointer<R>)();
				}

				return (new List<R>(Results));
			}
			else
			{
				return null;
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.\n
		/// \returns Returns a List<R> containing all return data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static List<R> RouteMessageDescendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R>> RT = RouteTable[EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address();
				}

				return (new List<R>(Results));
			}
			else
			{
				return null;
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.\n
		/// Accepts a Component that is used to derive a reference to the target GameObject.\n
		/// \returns Returns a List<R> containing all return data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static List<R> RouteMessageDescendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R>> RT = RouteTable[EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address();
				}

				return (new List<R>(Results));
			}
			else
			{
				return null;
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.\n
		/// \returns Returns a List<R> containing all return data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static List<R> RouteMessageAscendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R>> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address();
				}

				return (new List<R>(Results));
			}
			else
			{
				return null;
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.\n
		/// Accepts a Component that is used to derive a reference to the target GameObject.\n
		/// \returns Returns a List<R> containing all return data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static List<R> RouteMessageAscendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component.*/, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R>> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address();
				}

				return (new List<R>(Results));
			}
			else
			{
				return null;
			}
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// Uses the specified GameObject as the origin point.\n
		/// \returns Returns a List<R> containing all return data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		/// \note Only works for subscribed GameObjects.
		public static List<R> RouteMessageArea(GameObject Origin /**< GameObject specifying the origin of the event radius.\n Does not need to be subscribed unless it also is to receive the event. */, RoutingEvent EventType /**< Type of event to send. */, float Radius /**< Radius of the event in meters. */)
		{
			if (TablesExist && ScopeIsValid(Origin) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);
				decimal RadiusD = (new Decimal(Radius));

				List<Route<R>> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) <= RadiusD));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address();
				}

				return (new List<R>(Results));
			}
			else
			{
				return null;
			}
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// Uses the specified Component as the origin point.\n
		/// \returns Returns a List<R> containing all return data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		/// \note Only works for subscribed GameObjects.
		/// \bug Seems to call some routes twice; Possibly present in other methods as-well.
		public static List<R> RouteMessageArea(Component Origin /**< Component specifying the origin of the event radius.\n Can be of any type derived from Component.\n Does not need to be subscribed unless it also is to receive the event. */, RoutingEvent EventType /**< Type of event to send. */, float Radius /**< Radius of the event in meters. */)
		{
			if (TablesExist && ScopeIsValid(Origin.gameObject) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);
				decimal RadiusD = (new Decimal(Radius));

				List<Route<R>> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) <= RadiusD));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address();
				}

				return (new List<R>(Results));
			}
			else
			{
				return null;
			}
		}

		/** \internal *******************/
		/** \internal Utility Functions */
		/** \internal *******************/

		/// \internal Structors

		private static bool ConstructTables()
		{
			RouteTable = new Dictionary<RoutingEvent, List<Route<R>>>();
			PointerTable = new Dictionary<RoutingEvent, RoutePointer<R>>();
			return ((RouteTable != null) && (PointerTable != null));
		}

		private static bool DeconstructTables()
		{
			RouteTable = ((RouteTable.Count > 0)? RouteTable : null);
			PointerTable = ((PointerTable.Count > 0)? PointerTable : null);
			return ((RouteTable != null) && (PointerTable != null));
		}

		/// \internal Registration Functions

		private static void RegisterRoute(Route<R> RT)
		{
			if (!RouteTable.ContainsKey(RT.RouteEvent))
			{
				RouteTable.Add(RT.RouteEvent, new List<Route<R>>());
			}
			else
			{
				RouteTable[RT.RouteEvent].Add(RT);
			}
		}

		private static void DeregisterRoute(Route<R> RT)
		{
			if (EventIsRegistered(RT.RouteEvent))
			{
				RouteTable[RT.RouteEvent].Remove(RT);
			}
			else
			{
				RouteTable.Remove(RT.RouteEvent);
			}
		}

		private static void AttachAddress(Route<R> RT)
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

		private static void DetachAddress(Route<R> RT)
		{
			if (!KeyHasValue(RT.RouteEvent))
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
			if (!RouteTable[EventType].TrueForAll(x => x.Subscriber != null))
			{
				Route<R>[] DeadRoutes = RouteTable[EventType].ToArray();
				DeadRoutes = Array.FindAll(DeadRoutes, x => x.Subscriber == null);
				Array.ForEach(DeadRoutes, x => {DeregisterRoute(x); DetachAddress(x);});
				TablesExist = (TablesExist? DeconstructTables() : TablesExist);
			}
		}

		/// \internal Misc Functions

		private static bool KeyHasValue(RoutingEvent EventType)
		{
			return (PointerTable.ContainsKey(EventType) && (PointerTable[EventType] != null));
		}

		private static bool EventIsRegistered(RoutingEvent EventType)
		{
			return (RouteTable.ContainsKey(EventType) && (RouteTable[EventType] != null) && (RouteTable[EventType].Count >= 1));
		}

		private static bool ScopeIsValid(GameObject Scope)
		{
			return (Scope != null);
		}

		private static bool ScopeIsValid(Component Scope)
		{
			return (Scope != null);
		}
	}
}