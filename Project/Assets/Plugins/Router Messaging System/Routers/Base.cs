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
	/// \todo Find a way to do area messaging with only 1 O(n) operation.
	public class Router
	{
		private Dictionary<RoutingEvent, List<Route>> RouteTable = null;
		private bool TableExists = false;
		public string Name = string.Empty;

		public Router()
		{
			Name = "Router";
			Debug.Log("[" + Name + "] Router initialized!");
		}

		public Router(string RouterName)
		{
			Name = RouterName;
			Debug.Log("[" + Name + "] Router initialized!");
		}

		/** \brief Registers a new Route with the Router. */
		/// \note Prints an error if the specified Route is invalid or cannot be registered.\n
		/// \note An invalid Route contains null properties.
		public void AddRoute(Route NewRoute /**< Route to be registered. */)
		{
			if (!TableExists && NewRoute.IsValid)
			{
				RouteTable = new Dictionary<RoutingEvent, List<Route>>();
				TableExists = true;
			}

			if (NewRoute.IsValid && !RouteIsRegistered(NewRoute))
			{
				RegisterRoute(NewRoute);
			}
			else
			{
				Debug.LogError("[" + Name + "] Cannot register " +
							  (NewRoute.IsValid? "duplicate" : "invalid") +
							   " route " + NewRoute + ".", NewRoute.Subscriber);
			}
		}

		/** \brief Removes a Route from routing table. */
		/// \note Prints an error if the specified Route cannot be removed.
		public void RemoveRoute(Route OldRoute /**< Route to be removed. */)
		{
			if (TableExists && OldRoute.IsValid && RouteIsRegistered(OldRoute))
			{
				DeregisterRoute(OldRoute);
			}
			else
			{
				Debug.LogError("[" + Name + "] Cannot remove " +
							  (OldRoute.IsValid? "non-existant" : "invalid") +
							   " route " + OldRoute + ".", OldRoute.Subscriber);
			}

			DeleteEmptyTable();
		}

		/** \brief Flushes all Routes from the routing table. */
		/// \warning Only use this if you absolutely need to reset the entire routing table at runtime.\n
		/// \warning Every subscriber will need to re-register with the Router after the tables are flushed.\n
		/// \warning This can potentially be a slow operation, depending on the amount of subscribers that need to be re-registered.
		public void FlushRoutes()
		{
			if (TableExists)
			{
				RouteTable = null;
				TableExists = false;
				Debug.LogWarning("[" + Name + "] Flushed routing table!");
			}
		}

		/** \brief Returns the amount of Routes registered with the Router under the specified event. */
		/// \returns Returns the quantity of routes registered under the passed event type.
		/// \returns If Null is passed as event type then the total quantity for all events is returned.
		public int RouteCount(RoutingEvent EventType)
		{
			int EventRoutes = 0;

			if (TableExists &&
			   (EventType != RoutingEvent.Null) &&
				EventIsPopulated(EventType))
			{
				EventRoutes = RouteTable[EventType].Count;
			}

			if (TableExists && (EventType == RoutingEvent.Null))
			{
				List<Route>[] Lists = new List<Route>[RouteTable.Values.Count];
				RouteTable.Values.CopyTo(Lists, 0);

				Array.ForEach(Lists, x => EventRoutes += x.Count);
			}

			return EventRoutes;
		}

		/** \brief Routes a message of the specified event to all subscribers. */
		public void RouteMessage(RoutingEvent EventType /**< Type of event to send. */)
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
		public void RouteMessageDescendants(MessageTarget Parameters /**< MessageTarget specifying the scope of the message. */)
		{
			CleanDeadRoutes(Parameters.EventType);

			if (TableExists &&
				Parameters.IsValid &&
				EventIsPopulated(Parameters.EventType))
			{
				RouteTable[Parameters.EventType].ForEach(
									x => SendChild(x, Parameters.Recipient));
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.\n
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public void RouteMessageAscendants(MessageTarget Parameters /**< MessageTarget specifying the scope of the message. */)
		{
			CleanDeadRoutes(Parameters.EventType);

			if (TableExists &&
				Parameters.IsValid &&
				EventIsPopulated(Parameters.EventType))
			{
				RouteTable[Parameters.EventType].ForEach(
									x => SendParent(x, Parameters.Recipient));
			}
		}

		/** \brief Routes a message of the specified event to all subscribers inside the specified radius. */
		/** If UseInverse is true then a message will be routed to all subscribers outside the specified radius. */
		/// \note Only works for subscribed GameObjects.
		public void RouteMessageArea(AreaMessage Parameters /**< Struct containing parameters for the area message. */, bool DoRayCheck = false /**< Send messages only to entities that pass a Line-Of-Sight check. */ /*, int LM = 0 /**< Layermask value for LoS check.\nNot used if DoRayCheck is false. */)
		{
			CleanDeadRoutes(Parameters.EventType);

			if (TableExists && EventIsPopulated(Parameters.EventType))
			{
				Action<Route> Mailman = DoRayCheck?
					new Action<Route>(x => SendAreaChecked(x, Parameters)) :
					new Action<Route>(x => SendArea(x, Parameters));

				RouteTable[Parameters.EventType].ForEach(Mailman);
			}
		}

		/** \brief Routes a message to active subscribers. */
		public void RouteMessageActiveObjects(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TableExists && EventIsPopulated(EventType))
			{
				RouteTable[EventType].ForEach(SendActive);
			}
		}

		/** \brief Routes a message to inactive subscribers. */
		public void RouteMessageInactiveObjects(RoutingEvent EventType /**< Type of event to send. */)
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

		private void DeleteEmptyTable()
		{
			if (TableExists && (RouteTable.Count < 1))
			{
				RouteTable = null;
				TableExists = false;
			}
		}

		/// \internal Bookkeeping Functions

		private void RegisterRoute(Route RT)
		{
			if (!RouteTable.ContainsKey(RT.RouteEvent))
			{
				RouteTable.Add(RT.RouteEvent, new List<Route>());
			}

			RouteTable[RT.RouteEvent].Add(RT);
		}

		private void DeregisterRoute(Route RT)
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

		private bool RouteIsRegistered(Route RT)
		{
			return EventIsPopulated(RT.RouteEvent) &&
				   RouteTable[RT.RouteEvent].Contains(RT);
		}

		private bool EventIsPopulated(RoutingEvent EventType)
		{
			return RouteTable.ContainsKey(EventType) &&
				  (RouteTable[EventType].Count >= 1);
		}

		private void CleanDeadRoutes(RoutingEvent EventType)
		{
			if (TableExists && EventIsPopulated(EventType))
			{
				RouteTable[EventType].ForEach(PruneDead);
			}

			DeleteEmptyTable();
		}

		private void PruneDead(Route RT)
		{
			if (RT.IsDead)
			{
				DeregisterRoute(RT);
			}
		}

		/// \internal Routing Functions

		private void SendActive(Route RT)
		{
			if (RT.Subscriber.gameObject.activeInHierarchy)
			{
				RT.Address();
			}
		}

		private void SendInactive(Route RT)
		{
			if (!RT.Subscriber.gameObject.activeInHierarchy)
			{
				RT.Address();
			}
		}

		private void SendChild(Route Source, Transform Target)
		{
			if (Source.Subscriber.transform.IsChildOf(Target))
			{
				Source.Address();
			}
		}

		private void SendParent(Route Source, Transform Target)
		{
			if (Target.IsChildOf(Source.Subscriber.transform))
			{
				Source.Address();
			}
		}

		private void SendArea(Route RT, AreaMessage AM)
		{
			decimal Distance = new Decimal(
				Vector3.Distance(AM.Origin, RT.Subscriber.transform.position));
			decimal Radius = new Decimal(AM.Radius);

			if (AM.UseInverse && (Distance >= Radius))
			{
				RT.Address();
			}

			if (!AM.UseInverse && (Distance < Radius))
			{
				RT.Address();
			}
		}

		private void SendAreaChecked(Route RT, AreaMessage AM)
		{
			Vector3 Position = RT.Subscriber.transform.position;
			decimal Distance = new Decimal(
										Vector3.Distance(AM.Origin, Position));
			decimal Radius = new Decimal(AM.Radius);

			if (AM.UseInverse &&
			   (Distance >= Radius) && !Physics.Linecast(AM.Origin, Position))
			{
				RT.Address();
			}

			if (!AM.UseInverse &&
			   (Distance < Radius) && !Physics.Linecast(AM.Origin, Position))
			{
				RT.Address();
			}
		}
	}
}