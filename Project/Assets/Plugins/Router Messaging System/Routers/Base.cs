using UnityEngine;
using System;
using System.Collections.Generic;

/// \brief Namespace for Router Messaging System
/// Contains all APIs and objects needed by RMS
namespace RouterMessagingSystem
{
	/// \brief A router that operates on standard routes.
	/// \todo Implement RouteMessageContinuously. <- Reconsider this
	/// \todo Determine if MonoBehaviour->Component boxing is as costly as Struct->Object boxing.
	/// \todo Find a way to prevent Router from calling private functions. <- Reconsider this
	/// \todo Consider changing Router to non-static to allow for "team" routing.
	/// \todo Find a way to do descendant/ancestor/area messaging with only 1 O(n) operation.
	public static class Router
	{
		private static Dictionary<RoutingEvent, List<Route>> RouteTable = null;
		private static bool TableExists = false;

		static Router()
		{
			Debug.Log("Router initialized!");
		}

		/** \brief Registers a new Route with the Router. */
		/// \note Prints an error if the specified Route is invalid or cannot be registered.\n
		/// \note An invalid Route contains null properties.
		public static void AddRoute(Route NewRoute /**< Route to be registered. */)
		{
			if (!TableExists && NewRoute.IsValid)
			{
				CreateTable();
			}

			if (NewRoute.IsValid && !RouteIsRegistered(NewRoute))
			{
				RegisterRoute(NewRoute);
			}
			else
			{
				Debug.LogError("[Router] Cannot register " + (NewRoute.IsValid? "duplicate" : "invalid") + " route " + NewRoute + ".", NewRoute.Subscriber);
			}
		}

		/** \brief Removes a Route from routing table. */
		/// \note Prints an error if the specified Route cannot be removed.
		public static void RemoveRoute(Route OldRoute /**< Route to be removed. */)
		{
			if (TableExists && OldRoute.IsValid && RouteIsRegistered(OldRoute))
			{
				DeregisterRoute(OldRoute);
			}
			else
			{
				Debug.LogError("[Router] Cannot remove " + (OldRoute.IsValid? "non-existant" : "invalid") + " route " + OldRoute + ".", OldRoute.Subscriber);
			}

			DeleteEmptyTable();
		}

		/** \brief Flushes all Routes from the routing table. */
		/// \warning Only use this if you absolutely need to reset the entire routing table at runtime.\n
		/// \warning Every subscriber will need to re-register with the Router after the tables are flushed.\n
		/// \warning This can potentially be a slow operation, depending on the amount of subscribers that need to be re-registered.
		public static void FlushRoutes()
		{
			/*	Is there a point to checking if the table exists
				if we're just going to destroy it anyways? */
			if (TableExists)
			{
				RouteTable = null;
				TableExists = false;
				Debug.LogWarning("[Router] Routing table has been flushed!");
			}
		}

		/** \brief Returns the total amount of Routes registered with the Router. */
		/// \returns Int representing the Routes registered.
		public static int RouteCount()
		{
			int TotalRoutes = 0;

			if (TableExists)
			{
				List<Route>[] Lists = new List<Route>[RouteTable.Values.Count];
				RouteTable.Values.CopyTo(Lists, 0);

				Array.ForEach(Lists, x => TotalRoutes += x.Count);
			}

			return TotalRoutes;
		}

		/** \brief Routes a message of the specified event to all subscribers. */
		public static void RouteMessage(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TableExists && EventIsPopulated(EventType))
			{
				RouteTable[EventType].ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static void RouteMessageDescendants(MessageTarget Parameters /**< MessageTarget specifying the scope of the message. */)
		{
			CleanDeadRoutes(Parameters.EventType);

			if (TableExists && Parameters.IsValid && EventIsPopulated(Parameters.EventType))
			{
				RouteTable[Parameters.EventType].ForEach(x => SendChild(x, Parameters.Recipient));
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.\n
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static void RouteMessageAscendants(MessageTarget Parameters /**< MessageTarget specifying the scope of the message. */)
		{
			CleanDeadRoutes(Parameters.EventType);

			if (TableExists && Parameters.IsValid && EventIsPopulated(Parameters.EventType))
			{
				RouteTable[Parameters.EventType].ForEach(x => SendParent(x, Parameters.Recipient));
			}
		}

		/** \brief Routes a message of the specified event to all subscribers inside the specified radius. */
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageArea(AreaMessage Parameters /**< Struct containing parameters for the area message. */)
		{
			CleanDeadRoutes(Parameters.EventType);

			if (TableExists && EventIsPopulated(Parameters.EventType) && !Parameters.IsPoint)
			{
				decimal RadiusD = new Decimal(Parameters.Radius);
				List<Route> RT = RouteTable[Parameters.EventType].FindAll(x => (new Decimal(Vector3.Distance(Parameters.Origin, x.Subscriber.transform.position)) <= RadiusD));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers outside the specified radius. */
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageAreaInverse(AreaMessage Parameters /**< Struct containing parameters for the area message. */)
		{
			CleanDeadRoutes(Parameters.EventType);

			if (TableExists && EventIsPopulated(Parameters.EventType) && !Parameters.IsPoint)
			{
				decimal RadiusD = new Decimal(Parameters.Radius);
				List<Route> RT = RouteTable[Parameters.EventType].FindAll(x => (new Decimal(Vector3.Distance(Parameters.Origin, x.Subscriber.transform.position)) > RadiusD));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers inside a ring specified by an inner and outer radius. */
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageAreaBand(AreaBandMessage Parameters)
		{
			CleanDeadRoutes(Parameters.EventType);

			if (TableExists && EventIsPopulated(Parameters.EventType) && Parameters.HasVolume)
			{
				decimal InnerRadiusD = new Decimal(Parameters.InnerRadius), OuterRadiusD = new Decimal(Parameters.OuterRadius);
				List<Route> RT = RouteTable[Parameters.EventType].FindAll(x => { decimal Distance = new Decimal(Vector3.Distance(Parameters.Origin, x.Subscriber.transform.position)); return ((Distance >= InnerRadiusD) && (Distance <= OuterRadiusD)); });
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers outside a ring specified by an inner and outer radius. */
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageInverseAreaBand(AreaBandMessage Parameters)
		{
			CleanDeadRoutes(Parameters.EventType);

			if (TableExists && EventIsPopulated(Parameters.EventType) && Parameters.HasVolume)
			{
				decimal InnerRadiusD = new Decimal(Parameters.InnerRadius), OuterRadiusD = new Decimal(Parameters.OuterRadius);
				List<Route> RT = RouteTable[Parameters.EventType].FindAll(x => { decimal Distance = new Decimal(Vector3.Distance(Parameters.Origin, x.Subscriber.transform.position)); return ((Distance < InnerRadiusD) || (Distance > OuterRadiusD)); });
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message to active subscribers. */
		public static void RouteMessageActiveObjects(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TableExists && EventIsPopulated(EventType))
			{
				RouteTable[EventType].ForEach(SendActive);
			}
		}

		/** \brief Routes a message to inactive subscribers. */
		public static void RouteMessageInactiveObjects(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TableExists && EventIsPopulated(EventType))
			{
				RouteTable[EventType].ForEach(SendInactive);
			}
		}

		/** \internal **********/
		/** \internal Utilites */
		/** \internal **********/

		/// \internal Structors

		private static void CreateTable()
		{
			RouteTable = new Dictionary<RoutingEvent, List<Route>>();
			TableExists = true;
		}

		private static void DeleteEmptyTable()
		{
			if (TableExists && (RouteTable.Count < 1))
			{
				RouteTable = null;
			}

			TableExists = (RouteTable != null);
		}

		/// \internal Bookkeeping Functions

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

		private static bool RouteIsRegistered(Route RT)
		{
			return (EventIsPopulated(RT.RouteEvent) && RouteTable[RT.RouteEvent].Contains(RT));
		}

		private static bool EventIsPopulated(RoutingEvent EventType)
		{
			return (RouteTable.ContainsKey(EventType) && (RouteTable[EventType].Count >= 1));
		}

		private static void CleanDeadRoutes(RoutingEvent EventType)
		{
			if (TableExists && EventIsPopulated(EventType))
			{
				RouteTable[EventType].ForEach(PruneDead);
			}

			DeleteEmptyTable();
		}

		private static void PruneDead(Route RT)
		{
			if (RT.IsDead)
			{
				DeregisterRoute(RT);
			}
		}

		/// \internal Routing Functions

		private static void SendActive(Route RT)
		{
			if (RT.Subscriber.gameObject.activeInHierarchy)
			{
				RT.Address();
			}
		}

		private static void SendInactive(Route RT)
		{
			if (!RT.Subscriber.gameObject.activeInHierarchy)
			{
				RT.Address();
			}
		}

		private static void SendChild(Route Source, Transform Target)
		{
			if (Source.Subscriber.transform.IsChildOf(Target))
			{
				Source.Address();
			}
		}

		private static void SendParent(Route Source, Transform Target)
		{
			if (Target.IsChildOf(Source.Subscriber.transform))
			{
				Source.Address();
			}
		}
	}
}