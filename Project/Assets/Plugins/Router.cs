/** \brief Router Messaging System for Unity Engine */
/// \details A replacement messaging system for Unity Engine built around not using strings.
/// \author Philip Muzzall
/// \version 1.0
/// \date 3/17/2014
/// \copyright Property of Philip Muzzall
/// \internal The above block statements are repeated below for Doxygen purposes.

using UnityEngine;
using System;
using System.Collections.Generic;

/** \brief Namespace for Router Messaging System */
/// \details Provides encapsulation for RoutePointer, Route, RoutingEvent and Router.
/// \author Philip Muzzall
/// \version 1.0
/// \date 3/17/2014
/// \copyright Property of Philip Muzzall
namespace RouterMessagingSystem
{
	/** \brief Enumeration for event types. */
	public enum RoutingEvent
	{
		Null = 0, ///< Unused event type
		Test1 = 1, ///< Debug event 1
		Test2 = 2, ///< Debug event 2
	}

	/** \brief Delegate used by Route and Router for RouteEvent addressing. */
	public delegate void RoutePointer();

	/** \brief Route struct for use with Router Messaging System */
	/// \author Philip Muzzall
	/// \version 1.0
	/// \date 3/17/2014
	/// \copyright Property of Philip Muzzall
	/// \details Routes encapsulate the subscribing GameObject, the subscribed function and the subscribed event all in one object.\n
	/// \details Multiple Routes can be used to subscribe a GameObject to multiple events at once.\n
	/// \note It is not advised to pass a multicast delegate as a RoutePointer.
	public struct Route
	{
		/// Reference to the originator of this route.
		public readonly GameObject Subscriber;
		/// Reference to the function this route calls.
		public readonly RoutePointer Address;
		/// Value for the RouteEvent that calls this Route's address.
		public readonly RoutingEvent RouteEvent;

		/// Constructor that accepts a GameObject, a RoutePointer and a RoutingEvent.\n
		/// \warning Do not pass null as any parameters.\n
		/// \warning Router will discard any Routes with null attributes.
		public Route(GameObject RouteSubscriber /**< Reference to the subscribing GameObject. */, RoutePointer RoutingAddress /**< Reference to a function that this route calls.\n Must return void and accept no parameters. */, RoutingEvent Event /**< Value stating which event calls this Route. */) : this()
		{
			Subscriber = RouteSubscriber;
			Address = RoutingAddress;
			RouteEvent = Event;
		}

		/// Constructor that accepts a Component, a RoutePointer and a RoutingEvent.\n
		/// \warning Do not pass null as any parameters.\n
		/// \warning Router will discard any Routes with null attributes.
		public Route(Component RouteSubscriber /**< Used to derive a reference to the subscribing GameObject.\n Can be of any type derived from Component. */, RoutePointer RoutingAddress /**< Reference to a function that this route calls.\n Must return void and accept no parameters. */, RoutingEvent Event /**< Value stating which event calls this Route. */) : this()
		{
			Subscriber = RouteSubscriber.gameObject;
			Address = RoutingAddress;
			RouteEvent = Event;
		}

		/// Returns a string listing this Route's routing data.
		/// \returns A string containing the subscribing Component, the GameObject parent of the Component, the subscribing event and the callback function.
		public override string ToString()
		{
			return ("Subscriber: " + Address.Target + " | Event: " + RouteEvent.ToString() + " | Function: " + Address.Method);
		}
	}

	/** \brief Router Messaging System for Unity Engine */
	/// \details A replacement messaging system for Unity Engine built around not using blasted strings.
	/// \author Philip Muzzall
	/// \version 1.0
	/// \date 3/17/2014
	/// \copyright Property of Philip Muzzall
	/// \details Each GameObject must register with the Router before it can receive messages.\n
	/// To do this create a Route and register it with the Router using Router.AddRoute(Route NewRoute).\n
	/// GameObjects can register multiple Routes with the Router to subscribe to multiple events.
	/// \note Router internally maintains routing tables for all registered Routes.\n
	/// \note These tables do not exists until the first Route has been registered.\n
	/// \note If all Routes are removed then Router will destroy these tables and recreate them again when they are needed.
	public static class Router
	{
		private static Dictionary<RoutingEvent, RoutePointer> RouteTable = null;
		private static List<Route> ManifestTable = null, DeadRoutes = null;
		private static bool TablesExist = false;

		/** \brief Registers a new Route with the Router. */
		/// \warning Any Routes with null attributes will not be registered.
		public static void AddRoute(Route NewRoute /**< Route to be registered. */)
		{
			TablesExist = (TablesExist? TablesExist : ConstructTables());

			bool KeyPresent = RouteTable.ContainsKey(NewRoute.RouteEvent), ValidRoute = ((NewRoute.Subscriber != null) && (NewRoute.Address != null) && (NewRoute.RouteEvent != RoutingEvent.Null));

			if (ValidRoute && !ManifestTable.Contains(NewRoute))
			{
				ManifestTable.Add(NewRoute);
			}

			if (ValidRoute && KeyPresent)
			{
				AttachAddress(NewRoute);
			}

			if (ValidRoute && !KeyPresent)
			{
				RouteTable.Add(NewRoute.RouteEvent, NewRoute.Address);
			}
		}

		/** \brief Removes a Route from routing table. */
		public static void RemoveRoute(Route OldRoute /**< Route to be removed. */)
		{
			bool KeyPresent = false;

			if (TablesExist)
			{
				KeyPresent = RouteTable.ContainsKey(OldRoute.RouteEvent);
				ManifestTable.Remove(OldRoute);
			}

			if (TablesExist && KeyPresent && KeyHasValue(OldRoute.RouteEvent))
			{
				RemoveAddress(OldRoute);
			}

			if (TablesExist && KeyPresent && !KeyHasValue(OldRoute.RouteEvent))
			{
				RouteTable.Remove(OldRoute.RouteEvent);
			}

			TablesExist = (TablesExist? DestroyTables() : TablesExist);
		}

		/** \brief Flushes all Routes from the routing table. */
		/// \warning Only use this if you absolutely need to reset the entire routing table at runtime.\n
		/// \warning Every subscriber will need to re-register with the Router after the tables are flushed.\n
		/// \warning This is a relatively slow operation, depending on the amount of subscribers that need to be re-added.
		public static void FlushRoutes()
		{
			ManifestTable = null;
			RouteTable = null;
			DeadRoutes = null;
			TablesExist = false;
		}

		/** \brief Routes a message of the specified event to all subscribers. */
		public static void RouteMessage(RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && KeyHasValue(EventType))
			{
				CleanDeadRoutes();
				RouteTable[EventType]();
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.\n
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static void RouteMessageDownstream(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope))
			{
				CleanDeadRoutes();
				foreach (Route RT in ManifestTable)
				{
					RouteDownwardsIfParent(Scope, EventType, RT);
				}
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.\n
		/// Accepts a Component that is used to derive a reference to the target GameObject.\n
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static void RouteMessageDownstream(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope.gameObject))
			{
				CleanDeadRoutes();
				foreach (Route RT in ManifestTable)
				{
					RouteDownwardsIfParent(Scope.gameObject, EventType, RT);
				}
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.\n
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static void RouteMessageUpstream(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope))
			{
				CleanDeadRoutes();
				foreach (Route RT in ManifestTable)
				{
					RouteUpwardsIfChild(Scope, EventType, RT);
				}
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.\n
		/// Accepts a Component that is used to derive a reference to the target GameObject.\n
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static void RouteMessageUpstream(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component.*/, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope.gameObject))
			{
				CleanDeadRoutes();
				foreach (Route RT in ManifestTable)
				{
					RouteUpwardsIfChild(Scope.gameObject, EventType, RT);
				}
			}
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// Uses the specified GameObject as the origin point.\n
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageArea(GameObject Origin /**< GameObject specifying the origin of the event radius.\n Does not need to be subscribed unless it also is to receive the event. */, float Radius /**< Radius of the event in meters. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Origin))
			{
				CleanDeadRoutes();
				foreach (Route RT in ManifestTable)
				{
					RouteIfInRadius(Origin, EventType, RT, Radius);
				}
			}
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// Uses the specified Component as the origin point.\n
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageArea(Component Origin /**< Component specifying the origin of the event radius.\n Can be of any type derived from Component.\n Does not need to be subscribed unless it also is to receive the event. */, float Radius /**< Radius of the event in meters. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Origin.gameObject))
			{
				CleanDeadRoutes();
				foreach (Route RT in ManifestTable)
				{
					RouteIfInRadius(Origin.gameObject, EventType, RT, Radius);
				}
			}
		}

		/** \internal *******************/
		/** \internal Utility Functions */
		/** \internal *******************/

		/// \internal Structors

		private static bool ConstructTables()
		{
			ManifestTable = (new List<Route>());
			RouteTable = (new Dictionary<RoutingEvent, RoutePointer>());
			DeadRoutes = (new List<Route>());
			return ((ManifestTable != null) && (RouteTable != null) && (DeadRoutes != null));
		}

		private static bool DestroyTables()
		{
			ManifestTable = ((ManifestTable.Count > 0)? ManifestTable : null);
			RouteTable = ((RouteTable.Count > 0)? RouteTable : null);
			DeadRoutes = ((ManifestTable != null) && (RouteTable != null))? DeadRoutes : null;
			return ((ManifestTable != null) && (RouteTable != null) && (DeadRoutes != null));
		}

		/// \internal Registration Functions

		private static void AttachAddress(Route RT)
		{
			//RouteTable[RT.RouteEvent] = (RouteTable[RT.RouteEvent] + RT.Address);
			RouteTable[RT.RouteEvent] = (Delegate.Combine(RouteTable[RT.RouteEvent], RT.Address) as RoutePointer);
		}

		private static void RemoveAddress(Route RT)
		{
			//RouteTable[RT.RouteEvent] = (RouteTable[RT.RouteEvent] - RT.Address);
			RouteTable[RT.RouteEvent] = (Delegate.RemoveAll(RouteTable[RT.RouteEvent], RT.Address) as RoutePointer);
		}

		/// \internal Routing Functions

		private static void RouteDownwardsIfParent(GameObject Scope, RoutingEvent EventType, Route RT)
		{
			if ((RT.RouteEvent == EventType) && RT.Subscriber.transform.IsChildOf(Scope.transform))
			{
				RT.Address();
			}
		}

		private static void RouteUpwardsIfChild(GameObject Scope, RoutingEvent EventType, Route RT)
		{
			if ((RT.RouteEvent == EventType) && Scope.transform.IsChildOf(RT.Subscriber.transform))
			{
				RT.Address();
			}
		}

		private static void RouteIfInRadius(GameObject Scope, RoutingEvent EventType, Route RT, float Radius)
		{
			if ((RT.RouteEvent == EventType) && ((new Decimal(Vector3.Distance(Scope.transform.position, RT.Subscriber.transform.position))) < (new Decimal(Radius))))
			{
				RT.Address();
			}
		}

		/// \internal Janitorial Functions

		private static void CleanDeadRoutes()
		{
			DeadRoutes = ManifestTable.FindAll(x => x.Subscriber == null);

			ManifestTable.RemoveAll(x => x.Subscriber == null);

			foreach (Route RT in DeadRoutes)
			{
				PruneRoutes(RT);
			}

			DeadRoutes.Clear();
			TablesExist = (TablesExist? DestroyTables() : TablesExist);
		}

		private static void PruneRoutes(Route RT)
		{
			bool KeyPresent = RouteTable.ContainsKey(RT.RouteEvent);

			if (KeyPresent && KeyHasValue(RT.RouteEvent))
			{
				RemoveAddress(RT);
			}

			if (KeyPresent && !KeyHasValue(RT.RouteEvent))
			{
				RouteTable.Remove(RT.RouteEvent);
			}
		}

		/// \internal Misc Functions

		private static bool KeyHasValue(RoutingEvent EventType)
		{
			return (RouteTable.ContainsKey(EventType) && (RouteTable[EventType] != null));
		}

		private static bool ScopeIsValid(GameObject Scope)
		{
			return (Scope != null);
		}
	}
}