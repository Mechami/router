using UnityEngine;
using System;
using System.Collections.Generic;

namespace RouterMessagingSystem
{
	/// \brief A router that operates on routes that return an object of type R.
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
			TablesExist = ((NewRoute.IsValid && !TablesExist)? ConstructTables() : TablesExist);

			if (NewRoute.IsValid && !RouteIsRegistered(NewRoute))
			{
				RegisterRoute(NewRoute);
				AttachAddress(NewRoute);
			}
			else
			{
				Debug.LogError("[Router<" + typeof(R) + ">] Cannot register " + (NewRoute.IsValid? "duplicate" : "invalid") + " route " + NewRoute + ".", NewRoute.Subscriber);
			}
		}

		/** \brief Removes a Route from routing table. */
		/// \note Prints an error if the specified Route cannot be removed.
		public static void RemoveRoute(Route<R> OldRoute /**< Route to be removed. */)
		{
			if (TablesExist && OldRoute.IsValid && RouteIsRegistered(OldRoute))
			{
				DetachAddress(OldRoute);
				DeregisterRoute(OldRoute);
			}
			else
			{
				Debug.LogError("[Router<" + typeof(R) + ">] Cannot remove " + (OldRoute.IsValid? "non-existant" : "invalid") + " route " + OldRoute + ".", OldRoute.Subscriber);
			}

			TablesExist = (TablesExist? DeconstructTables() : TablesExist);
		}

		/** \brief Flushes all Routes from the routing table. */
		/// \warning Only use this if you absolutely need to reset the entire routing table at runtime.\n
		/// \warning Every subscriber will need to re-register with the Router after the tables are flushed.\n
		/// \warning This can potentially be a slow operation, depending on the amount of subscribers that need to be re-registered.
		public static void FlushRoutes()
		{
			if (TablesExist)
			{
				RouteTable = null;
				PointerTable = null;
				TablesExist = false;
				Debug.LogWarning("[Router<" + typeof(R) + ">] Routing tables have been flushed!");
			}
		}

		/** \brief Returns the total amount of Routes registered with the Router. */
		/// \returns Int representing the Routes registered.
		public static int RouteCount()
		{
			int TotalRoutes = 0;

			if (TablesExist)
			{
				List<Route<R>>[] Lists = new List<Route<R>>[RouteTable.Values.Count];
				RouteTable.Values.CopyTo(Lists, 0);

				Array.ForEach(Lists, x => TotalRoutes += x.Count);
			}

			return TotalRoutes;
		}

		/// \brief Routes a message of the specified event to all subscribers and returns their values.
		/// \returns Returns a List<R> containing all returned data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		public static List<R> RouteMessage(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			List<R> Results = null;

			if (TablesExist && KeyHasAddress(EventType) && EventIsRegistered(EventType))
			{
				Delegate[] RPL = PointerTable[EventType].GetInvocationList();
				Results = new List<R>(RPL.Length);

				for (int i = 0; i < RPL.Length; i++)
				{
					Results.Add((RPL[i] as RoutePointer<R>)());
				}

				Results.TrimExcess();
			}

			return Results;
		}

		/// \brief Routes a message of the specified event to all subscribers and returns their values in the specified list.
		/// \returns Adds all returned data from subscribers of this event type to the specified list.\n
		/// \returns Assigns null to OutputList if the message cannot be sent.
		public static void RouteMessage(RoutingEvent EventType /**< Type of event to send. */, out List<R> OutputList /**< List to output returned values into. */)
		{
			CleanDeadRoutes(EventType);
			OutputList = null;

			if (TablesExist && KeyHasAddress(EventType) && EventIsRegistered(EventType))
			{
				Delegate[] RPL = PointerTable[EventType].GetInvocationList();
				OutputList = new List<R>(RPL.Length);

				for (int i = 0; i < RPL.Length; i++)
				{
					OutputList.Add((RPL[i] as RoutePointer<R>)());
				}

				OutputList.TrimExcess();
			}
		}

		/// \brief Routes a message of the specified event to all subscribers and adds their values to the specified queue.
		/// \returns Enqueues all returned data from subscribers of this event type in the specified queue.\n
		/// \returns Enqueues nothing if the message cannot be sent.
		public static void RouteMessage(RoutingEvent EventType /**< Type of event to send. */, Queue<R> OutputQueue /**< Queue to output returned values into. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && KeyHasAddress(EventType) && EventIsRegistered(EventType))
			{
				Delegate[] RPL = PointerTable[EventType].GetInvocationList();

				for (int i = 0; i < RPL.Length; i++)
				{
					OutputQueue.Enqueue((RPL[i] as RoutePointer<R>)());
				}
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.
		/// \returns Returns a List<R> containing all returned data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static List<R> RouteMessageDescendants(MessageTarget Scope /**< MessageTarget specifying the scope and event of the message. */)
		{
			CleanDeadRoutes(Scope.EventType);

			List<R> Results = null;

			if (TablesExist && Scope.IsValid && EventIsPopulated(Scope.EventType))
			{
				List<Route<R>> RT = RouteTable[Scope.EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.Recipient));
				Results = new List<R>(RT.Count);

				for (int i = 0; i < RT.Count; i++)
				{
					Results.Add(RT[i].Address());
				}

				Results.TrimExcess();	
			}

			return Results;
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// \returns Adds all returned data from subscribers of this event type to the specified list.\n
		/// \returns Assigns null to OutputList if the message cannot be sent.
		public static void RouteMessageDescendants(MessageTarget Scope /**< MessageTarget specifying the scope and event of the message. */, out List<R> OutputList /**< List to output returned values into. */)
		{
			CleanDeadRoutes(Scope.EventType);
			OutputList = null;

			if (TablesExist && Scope.IsValid && EventIsPopulated(Scope.EventType))
			{
				List<Route<R>> RT = RouteTable[Scope.EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.Recipient));
				OutputList = new List<R>(RT.Count);

				for (int i = 0; i < RT.Count; i++)
				{
					OutputList.Add(RT[i].Address());
				}

				OutputList.TrimExcess();	
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// \returns Enqueues all returned data from subscribers of this event type in the specified queue.\n
		/// \returns Enqueues nothing if the message cannot be sent.
		public static void RouteMessageDescendants(MessageTarget Scope /**< MessageTarget specifying the scope and event of the message. */, Queue<R> OutputQueue /**< Queue to output returned values into. */)
		{
			CleanDeadRoutes(Scope.EventType);

			if (TablesExist && Scope.IsValid && EventIsPopulated(Scope.EventType))
			{
				List<Route<R>> RT = RouteTable[Scope.EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.Recipient));
				RT.ForEach(x => OutputQueue.Enqueue(x.Address()));
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.
		/// \returns Returns a List<R> containing all returned data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static List<R> RouteMessageAscendants(MessageTarget Scope /**< MessageTarget specifying the scope and event of the message. */)
		{
			CleanDeadRoutes(Scope.EventType);

			List<R> Results = null;

			if (TablesExist && Scope.IsValid && EventIsPopulated(Scope.EventType))
			{
				List<Route<R>> RT = RouteTable[Scope.EventType].FindAll(x => Scope.Recipient.IsChildOf(x.Subscriber.transform));
				Results = new List<R>(RT.Count);

				for (int i = 0; i < RT.Count; i++)
				{
					Results.Add(RT[i].Address());
				}

				Results.TrimExcess();	
			}

			return Results;
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// \returns Adds all returned data from subscribers of this event type to the specified list.\n
		/// \returns Assigns null to OutputList if the message cannot be sent.
		public static void RouteMessageAscendants(MessageTarget Scope /**< MessageTarget specifying the scope and event of the message. */, out List<R> OutputList /**< List to output returned values into. */)
		{
			CleanDeadRoutes(Scope.EventType);
			OutputList = null;

			if (TablesExist && Scope.IsValid && EventIsPopulated(Scope.EventType))
			{
				List<Route<R>> RT = RouteTable[Scope.EventType].FindAll(x => Scope.Recipient.IsChildOf(x.Subscriber.transform));
				OutputList = new List<R>(RT.Count);

				for (int i = 0; i < RT.Count; i++)
				{
					OutputList.Add(RT[i].Address());
				}

				OutputList.TrimExcess();	
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// \returns Enqueues all returned data from subscribers of this event type in the specified queue.\n
		/// \returns Enqueues nothing if the message cannot be sent.
		public static void RouteMessageAscendants(MessageTarget Scope /**< MessageTarget specifying the scope and event of the message. */, Queue<R> OutputQueue /**< Queue to output returned values into. */)
		{
			CleanDeadRoutes(Scope.EventType);

			if (TablesExist && Scope.IsValid && EventIsPopulated(Scope.EventType))
			{
				List<Route<R>> RT = RouteTable[Scope.EventType].FindAll(x => Scope.Recipient.IsChildOf(x.Subscriber.transform));
				RT.ForEach(x => OutputQueue.Enqueue(x.Address()));
			}
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// \returns Returns a List<R> containing all returned data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		/// \note Only works for subscribed GameObjects.
		/// \bug Seems to call some routes twice; Possibly present in other methods as-well. <- Test after re-implementation.
		public static List<R> RouteMessageArea(AreaMessage MessageParameters /**< Struct containing parameters for the area message. */)
		{
			CleanDeadRoutes(MessageParameters.EventType);

			List<R> Results = null;

			if (TablesExist && EventIsPopulated(MessageParameters.EventType))
			{
				decimal RadiusD = new Decimal(MessageParameters.Radius);
				List<Route<R>> RT = RouteTable[MessageParameters.EventType].FindAll(x => (new Decimal(Vector3.Distance(MessageParameters.Origin, x.Subscriber.transform.position)) <= RadiusD));
				Results = new List<R>(RT.Count);

				for (int i = 0; i < RT.Count; i++)
				{
					Results.Add(RT[i].Address());
				}

				Results.TrimExcess();	
			}

			return Results;
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// \returns Adds all returned data from subscribers of this event type to the specified list.\n
		/// \returns Assigns null to OutputList if the message cannot be sent.
		public static void RouteMessageArea(AreaMessage MessageParameters /**< Struct containing parameters for the area message. */, out List<R> OutputList /**< List to output returned values into. */)
		{
			CleanDeadRoutes(MessageParameters.EventType);
			OutputList = null;

			if (TablesExist && EventIsPopulated(MessageParameters.EventType))
			{
				decimal RadiusD = new Decimal(MessageParameters.Radius);
				List<Route<R>> RT = RouteTable[MessageParameters.EventType].FindAll(x => (new Decimal(Vector3.Distance(MessageParameters.Origin, x.Subscriber.transform.position)) <= RadiusD));
				OutputList = new List<R>(RT.Count);

				for (int i = 0; i < RT.Count; i++)
				{
					OutputList.Add(RT[i].Address());
				}

				OutputList.TrimExcess();	
			}
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// \returns Enqueues all returned data from subscribers of this event type in the specified queue.\n
		/// \returns Enqueues nothing if the message cannot be sent.
		public static void RouteMessageArea(AreaMessage MessageParameters /**< Struct containing parameters for the area message. */, Queue<R> OutputQueue /**< Queue to output returned values into. */)
		{
			CleanDeadRoutes(MessageParameters.EventType);

			if (TablesExist && EventIsPopulated(MessageParameters.EventType))
			{
				decimal RadiusD = new Decimal(MessageParameters.Radius);
				List<Route<R>> RT = RouteTable[MessageParameters.EventType].FindAll(x => (new Decimal(Vector3.Distance(MessageParameters.Origin, x.Subscriber.transform.position)) <= RadiusD));
				RT.ForEach(x => OutputQueue.Enqueue(x.Address()));
			}
		}

		/** \brief Routes a message of the specified event to all subscribers outside the specified radius. */
		/// \returns Returns a List<R> containing all returned data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		/// \note Only works for subscribed GameObjects.
		public static List<R> RouteMessageAreaInverse(AreaMessage MessageParameters /**< Struct containing parameters for the area message. */)
		{
			CleanDeadRoutes(MessageParameters.EventType);

			List<R> Results = null;

			if (TablesExist && EventIsPopulated(MessageParameters.EventType))
			{
				decimal RadiusD = new Decimal(MessageParameters.Radius);
				List<Route<R>> RT = RouteTable[MessageParameters.EventType].FindAll(x => (new Decimal(Vector3.Distance(MessageParameters.Origin, x.Subscriber.transform.position)) > RadiusD));
				Results = new List<R>(RT.Count);

				for (int i = 0; i < RT.Count; i++)
				{
					Results.Add(RT[i].Address());
				}

				Results.TrimExcess();	
			}

			return Results;
		}

		/** \brief Routes a message of the specified event to all subscribers outside the specified radius. */
		/// \returns Adds all returned data from subscribers of this event type to the specified list.\n
		/// \returns Assigns null to OutputList if the message cannot be sent.
		public static void RouteMessageAreaInverse(AreaMessage MessageParameters /**< Struct containing parameters for the area message. */, out List<R> OutputList /**< List to output returned values into. */)
		{
			CleanDeadRoutes(MessageParameters.EventType);
			OutputList = null;

			if (TablesExist && EventIsPopulated(MessageParameters.EventType))
			{
				decimal RadiusD = new Decimal(MessageParameters.Radius);
				List<Route<R>> RT = RouteTable[MessageParameters.EventType].FindAll(x => (new Decimal(Vector3.Distance(MessageParameters.Origin, x.Subscriber.transform.position)) > RadiusD));
				OutputList = new List<R>(RT.Count);

				for (int i = 0; i < RT.Count; i++)
				{
					OutputList.Add(RT[i].Address());
				}

				OutputList.TrimExcess();	
			}
		}

		/** \brief Routes a message of the specified event to all subscribers outside the specified radius. */
		/// \returns Enqueues all returned data from subscribers of this event type in the specified queue.\n
		/// \returns Enqueues nothing if the message cannot be sent.
		public static void RouteMessageAreaInverse(AreaMessage MessageParameters /**< Struct containing parameters for the area message. */, Queue<R> OutputQueue /**< Queue to output returned values into. */)
		{
			CleanDeadRoutes(MessageParameters.EventType);
	
			if (TablesExist && EventIsPopulated(MessageParameters.EventType))
			{
				decimal RadiusD = new Decimal(MessageParameters.Radius);
				List<Route<R>> RT = RouteTable[MessageParameters.EventType].FindAll(x => (new Decimal(Vector3.Distance(MessageParameters.Origin, x.Subscriber.transform.position)) > RadiusD));
				RT.ForEach(x => OutputQueue.Enqueue(x.Address()));
			}
		}

		/** \brief Routes a message to inactive subscribers. */
		/// \returns Returns a List<R> containing all returned data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		public static List<R> RouteMessageInactiveObjects(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			List<R> Results = null;

			if (TablesExist && EventIsPopulated(EventType))
			{
				List<Route<R>> RPL = RouteTable[EventType].FindAll(x => !x.Subscriber.gameObject.activeInHierarchy);
				Results = new List<R>(RPL.Count);

				for (int i = 0; i < RPL.Count; i++)
				{
					Results.Add(RPL[i].Address());
				}

				Results.TrimExcess();	
			}

			return Results;
		}

		/** \brief Routes a message to inactive subscribers. */
		/// \returns Adds all returned data from subscribers of this event type to the specified list.\n
		/// \returns Assigns null to OutputList if the message cannot be sent.
		public static void RouteMessageInactiveObjects(RoutingEvent EventType /**< Type of event to send. */, out List<R> OutputList /**< List to output returned values into. */)
		{
			CleanDeadRoutes(EventType);
			OutputList = null;

			if (TablesExist && EventIsPopulated(EventType))
			{
				List<Route<R>> RPL = RouteTable[EventType].FindAll(x => !x.Subscriber.gameObject.activeInHierarchy);
				OutputList = new List<R>(RPL.Count);

				for (int i = 0; i < RPL.Count; i++)
				{
					OutputList.Add(RPL[i].Address());
				}

				OutputList.TrimExcess();	
			}
		}

		/** \brief Routes a message to inactive subscribers. */
		/// \returns Enqueues all returned data from subscribers of this event type in the specified queue.\n
		/// \returns Enqueues nothing if the message cannot be sent.
		public static void RouteMessageInactiveObjects(RoutingEvent EventType /**< Type of event to send. */, Queue<R> OutputQueue /**< Queue to output returned values into. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && EventIsPopulated(EventType))
			{
				List<Route<R>> RPL = RouteTable[EventType].FindAll(x => !x.Subscriber.gameObject.activeInHierarchy);
				RPL.ForEach(x => OutputQueue.Enqueue(x.Address()));
			}
		}

		/** \brief Routes a message to active subscribers. */
		/// \returns Returns a List<R> containing all returned data from subscribers of this event type.\n
		/// \returns Otherwise returns null if the message cannot be sent.
		public static List<R> RouteMessageActiveObjects(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			List<R> Results = null;

			if (TablesExist && EventIsPopulated(EventType))
			{
				List<Route<R>> RPL = RouteTable[EventType].FindAll(x => x.Subscriber.gameObject.activeInHierarchy);
				Results = new List<R>(RPL.Count);

				for (int i = 0; i < RPL.Count; i++)
				{
					Results.Add(RPL[i].Address());
				}

				Results.TrimExcess();	
			}

			return Results;
		}

		/** \brief Routes a message to active subscribers. */
		/// \returns Adds all returned data from subscribers of this event type to the specified list.\n
		/// \returns Assigns null to OutputList if the message cannot be sent.
		public static void RouteMessageActiveObjects(RoutingEvent EventType /**< Type of event to send. */, out List<R> OutputList /**< List to output returned values into. */)
		{
			CleanDeadRoutes(EventType);
			OutputList = null;

			if (TablesExist && EventIsPopulated(EventType))
			{
				List<Route<R>> RPL = RouteTable[EventType].FindAll(x => x.Subscriber.gameObject.activeInHierarchy);
				OutputList = new List<R>(RPL.Count);

				for (int i = 0; i < RPL.Count; i++)
				{
					OutputList.Add(RPL[i].Address());
				}

				OutputList.TrimExcess();	
			}
		}

		/** \brief Routes a message to active subscribers. */
		/// \returns Enqueues all returned data from subscribers of this event type in the specified queue.\n
		/// \returns Enqueues nothing if the message cannot be sent.
		public static void RouteMessageActiveObjects(RoutingEvent EventType /**< Type of event to send. */, Queue<R> OutputQueue /**< Queue to output returned values into. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && EventIsPopulated(EventType))
			{
				List<Route<R>> RPL = RouteTable[EventType].FindAll(x => x.Subscriber.gameObject.activeInHierarchy);
				RPL.ForEach(x => OutputQueue.Enqueue(x.Address()));
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
			return true;
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

			RouteTable[RT.RouteEvent].Add(RT);
		}

		private static void DeregisterRoute(Route<R> RT)
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
			if (TablesExist && EventIsPopulated(EventType) && TableIsPolluted(EventType))
			{
				Route<R>[] DeadRoutes = Array.FindAll(RouteTable[EventType].ToArray(), x => x.IsDead);
				Array.ForEach(DeadRoutes, x => {DeregisterRoute(x); DetachAddress(x);});
				TablesExist = (TablesExist? DeconstructTables() : TablesExist);
			}
		}

		private static bool TableIsPolluted(RoutingEvent EventType)
		{
			return RouteTable[EventType].TrueForAll(x => !x.IsDead);
		}

		/// \internal Misc Functions

		private static bool RouteIsRegistered(Route<R> RT)
		{
			return (EventIsPopulated(RT.RouteEvent) && RouteTable[RT.RouteEvent].Contains(RT));
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
	}
}