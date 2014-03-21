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
/// \version 1.0.0
/// \date 3/17/2014
/// \copyright Property of Philip Muzzall
namespace RouterMessagingSystem
{
	/** \brief Enumeration for event types. */
	public enum RoutingEvent : byte
	{
		Null = 0, ///< Unused event type
		Test1 = 1, ///< Debug event 1
		Test2 = 2, ///< Debug event 2
	}

	/** \brief RoutePointer that doesn't accept or return any parameters. */
	public delegate void RoutePointer();

	/** \brief RoutePointer that returns an object of type R. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R>();

	/** \brief RoutePointer that returns an object and accepts one parameter. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R, in T>(T P1 /**< Input parameter of type T. */);

	/** \brief RoutePointer that returns an object and accepts two parameters. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R, in T1, in T2>(T1 P1 /**< Input parameter of type T1. */, T2 P2 /**< Input parameter of type T2. */);

	/** \brief RoutePointer that returns an object and accepts three parameters. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R, in T1, in T2, in T3>(T1 P1 /**< Input parameter of type T1. */, T2 P2 /**< Input parameter of type T2. */, T3 P3 /**< Input parameter of type T3. */);

	/** \brief Route struct for use with Router Messaging System */
	/// \author Philip Muzzall
	/// \version 1.0.0
	/// \date 3/17/2014
	/// \copyright Property of Philip Muzzall
	/// \details Routes encapsulate the subscribing Component, the subscribed function and the subscribed event all in one object.\n
	/// \details Multiple Routes can be used to subscribe a Component to multiple events at once.\n
	/// \note It is not advised to pass a multicast delegate as a RoutePointer.
	public struct Route : IEquatable<Route>
	{
		/// Reference to the originator of this route.
		public readonly Component Subscriber;
		/// Reference to the function this route calls.
		public readonly RoutePointer Address;
		/// Value for the RouteEvent that calls this Route's address.
		public readonly RoutingEvent RouteEvent;

		/// \brief Constructor that accepts a Component, a RoutePointer and a RoutingEvent.
		/// \warning Do not pass null as any parameters.\n
		/// \warning Router will discard any Routes with null attributes.
		public Route(Component RouteSubscriber /**< Reference to the subscribing Component.\n Can be of any type derived from Component. */, RoutePointer RoutingAddress /**< Reference to a function that this route calls.\n Must return void and accept no parameters. */, RoutingEvent Event /**< Value stating which event calls this Route. */) : this()
		{
			Subscriber = RouteSubscriber;
			Address = RoutingAddress;
			RouteEvent = Event;
		}

		/// \brief Checks if the attributes of the passed Route are the same as the calling Route's attributes.
		/// \return Returns true if all attributes are the same, otherwise false.
		public bool Equals(Route RT /**< Route to compare with the calling Route. */)
		{
			return ((this.Subscriber == RT.Subscriber) && (this.Address == RT.Address) && (this.RouteEvent == RT.RouteEvent));
		}

		/// \brief Checks if the passed object is the same as the calling Route.
		/// \return Returns true if Obj is a Route and all attributes are the same as the calling Route.\n
		/// \return Immediately returns false if Obj is not a Route at all.\n
		public override bool Equals(System.Object Obj /**< Object to check for equivalency. */)
		{
			return ((Obj == typeof(Route)) && (this.Subscriber == ((Route)Obj).Subscriber) && (this.Address == ((Route)Obj).Address) && (this.RouteEvent == ((Route)Obj).RouteEvent));
		}

		/// \brief Returns a hash of this Route.
		/// \returns Hash generated from combined member attribute hashes.
		public override int GetHashCode()
		{
			return (this.Subscriber.GetHashCode() + this.Address.GetHashCode() + this.RouteEvent.GetHashCode());
		}

		/// \brief Returns a string listing this Route's routing data.
		/// \returns A string containing the subscribing Component, the subscribing event and the callback function.
		public override string ToString()
		{
			return ("Subscriber: " + Address.Target + " | Event: " + RouteEvent.ToString() + " | Function: " + Address.Method);
		}

		/// \brief Compares two Routes for equivalent attributes.
		/// \returns True if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		/// \returns False if any attribute of the left-side operand is different from the respective attribute of the right-side operand.
		public static bool operator ==(Route RT1 /**< Left-side operand */, Route RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber == RT2.Subscriber) && (RT1.Address == RT2.Address) && (RT1.RouteEvent == RT2.RouteEvent));
		}

		/// \brief Contrasts two Routes for differing attributes.
		/// \returns True if any attribute of the left-side operand is different from the respective attribute of the right-side operand.\n
		/// \returns False if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		public static bool operator !=(Route RT1 /**< Left-side operand */, Route RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber != RT2.Subscriber) || (RT1.Address != RT2.Address) || (RT1.RouteEvent != RT2.RouteEvent));
		}
	}

	/** \brief Route that returns a value of type R. */
	public struct Route<R> : IEquatable<Route<R>>
	{
		/// Reference to the originator of this route.
		public readonly Component Subscriber;
		/// Reference to the function this route calls.
		public readonly RoutePointer<R> Address;
		/// Value for the RouteEvent that calls this Route's address.
		public readonly RoutingEvent RouteEvent;

		/// \brief Constructor that accepts a Component, a RoutePointer<R> and a RoutingEvent.
		/// \warning Do not pass null as any parameters.\n
		/// \warning Router will discard any Routes with null attributes.
		public Route(Component RouteSubscriber /**< Reference to the subscribing Component.\n Can be of any type derived from Component. */, RoutePointer<R> RoutingAddress /**< Reference to a function that this route calls.\n R is return type. */, RoutingEvent Event /**< Value stating which event calls this Route. */) : this()
		{
			Subscriber = RouteSubscriber;
			Address = RoutingAddress;
			RouteEvent = Event;
		}

		/// \brief Checks if the attributes of the passed Route<R> are the same as the calling Route<R>'s attributes.
		/// \return Returns true if all attributes are the same, otherwise false.
		public bool Equals(Route<R> RT /**< Route<R> to compare with the calling Route<R>. */)
		{
			return ((this.Subscriber == RT.Subscriber) && (this.Address == RT.Address) && (this.RouteEvent == RT.RouteEvent));
		}

		/// \brief Checks if the passed object is the same as the calling Route<R>.
		/// \return Returns true if Obj is a Route<R> and all attributes are the same as the calling Route.\n
		/// \return Immediately returns false if Obj is not a Route<R> at all.\n
		public override bool Equals(System.Object Obj /**< Object to check for equivalency. */)
		{
			return ((Obj == typeof(Route<R>)) && (this.Subscriber == ((Route<R>)Obj).Subscriber) && (this.Address == ((Route<R>)Obj).Address) && (this.RouteEvent == ((Route<R>)Obj).RouteEvent));
		}

		/// \brief Returns a hash of this Route<R>.
		/// \returns Hash generated from combined member attribute hashes.
		public override int GetHashCode()
		{
			return (this.Subscriber.GetHashCode() + this.Address.GetHashCode() + this.RouteEvent.GetHashCode());
		}

		/// \brief Returns a string listing this Route<R>'s routing data.
		/// \returns A string containing the subscribing Component, the subscribing event and the callback function.
		public override string ToString()
		{
			return ("Subscriber: " + Address.Target + " | Event: " + RouteEvent.ToString() + " | Function: " + Address.Method);
		}

		/// \brief Compares two Route<R>'s for equivalent attributes.
		/// \returns True if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		/// \returns False if any attribute of the left-side operand is different from the respective attribute of the right-side operand.
		public static bool operator ==(Route<R> RT1 /**< Left-side operand */, Route<R> RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber == RT2.Subscriber) && (RT1.Address == RT2.Address) && (RT1.RouteEvent == RT2.RouteEvent));
		}

		/// \brief Contrasts two Route<R>'s for differing attributes.
		/// \returns True if any attribute of the left-side operand is different from the respective attribute of the right-side operand.\n
		/// \returns False if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		public static bool operator !=(Route<R> RT1 /**< Left-side operand */, Route<R> RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber != RT2.Subscriber) || (RT1.Address != RT2.Address) || (RT1.RouteEvent != RT2.RouteEvent));
		}
	}

	/** \brief Route that returns a value of type R and accepts a parameter of type T. */
	public struct Route<R, T> : IEquatable<Route<R, T>>
	{
		/// Reference to the originator of this route.
		public readonly Component Subscriber;
		/// Reference to the function this route calls.
		public readonly RoutePointer<R, T> Address;
		/// Value for the RouteEvent that calls this Route's address.
		public readonly RoutingEvent RouteEvent;

		/// \brief Constructor that accepts a Component, a RoutePointer<R, T> and a RoutingEvent.
		/// \warning Do not pass null as any parameters.\n
		/// \warning Router will discard any Routes with null attributes.
		public Route(Component RouteSubscriber /**< Reference to the subscribing Component.\n Can be of any type derived from Component. */, RoutePointer<R, T> RoutingAddress /**< Reference to a function that this route calls.\n R is return type, T is parameter type. */, RoutingEvent Event /**< Value stating which event calls this Route. */) : this()
		{
			Subscriber = RouteSubscriber;
			Address = RoutingAddress;
			RouteEvent = Event;
		}

		/// \brief Checks if the attributes of the passed Route<R, T> are the same as the calling Route<R, T>'s attributes.
		/// \return Returns true if all attributes are the same, otherwise false.
		public bool Equals(Route<R, T> RT /**< Route<R, T> to compare with the calling Route<R, T>. */)
		{
			return ((this.Subscriber == RT.Subscriber) && (this.Address == RT.Address) && (this.RouteEvent == RT.RouteEvent));
		}

		/// \brief Checks if the passed object is the same as the calling Route.
		/// \return Returns true if Obj is a Route<R, T> and all attributes are the same as the calling Route<R, T>.\n
		/// \return Immediately returns false if Obj is not a Route<R, T> at all.\n
		public override bool Equals(System.Object Obj /**< Object to check for equivalency. */)
		{
			return ((Obj == typeof(Route<R, T>)) && (this.Subscriber == ((Route<R, T>)Obj).Subscriber) && (this.Address == ((Route<R, T>)Obj).Address) && (this.RouteEvent == ((Route<R, T>)Obj).RouteEvent));
		}

		/// \brief Returns a hash of this Route<R, T>.
		/// \returns Hash generated from combined member attribute hashes.
		public override int GetHashCode()
		{
			return (this.Subscriber.GetHashCode() + this.Address.GetHashCode() + this.RouteEvent.GetHashCode());
		}

		/// \brief Returns a string listing this Route<R, T>'s routing data.
		/// \returns A string containing the subscribing Component, the subscribing event and the callback function.
		public override string ToString()
		{
			return ("Subscriber: " + Address.Target + " | Event: " + RouteEvent.ToString() + " | Function: " + Address.Method);
		}

		/// \brief Compares two Route<R, T>'s for equivalent attributes.
		/// \returns True if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		/// \returns False if any attribute of the left-side operand is different from the respective attribute of the right-side operand.
		public static bool operator ==(Route<R, T> RT1 /**< Left-side operand */, Route<R, T> RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber == RT2.Subscriber) && (RT1.Address == RT2.Address) && (RT1.RouteEvent == RT2.RouteEvent));
		}

		/// \brief Contrasts two Route<R, T>'s for differing attributes.
		/// \returns True if any attribute of the left-side operand is different from the respective attribute of the right-side operand.\n
		/// \returns False if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		public static bool operator !=(Route<R, T> RT1 /**< Left-side operand */, Route<R, T> RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber != RT2.Subscriber) || (RT1.Address != RT2.Address) || (RT1.RouteEvent != RT2.RouteEvent));
		}
	}

	/** \brief Route that returns a value of type R and accepts one parameter of type T1 and one parameter of T2. */
	public struct Route<R, T1, T2> : IEquatable<Route<R, T1, T2>>
	{
		/// Reference to the originator of this route.
		public readonly Component Subscriber;
		/// Reference to the function this route calls.
		public readonly RoutePointer<R, T1, T2> Address;
		/// Value for the RouteEvent that calls this Route's address.
		public readonly RoutingEvent RouteEvent;

		/// \brief Constructor that accepts a Component, a RoutePointer<R, T1, T2> and a RoutingEvent.
		/// \warning Do not pass null as any parameters.\n
		/// \warning Router will discard any Routes with null attributes.
		public Route(Component RouteSubscriber /**< Reference to the subscribing Component.\n Can be of any type derived from Component. */, RoutePointer<R, T1, T2> RoutingAddress /**< Reference to a function that this route calls.\n R is return type, T1 and T2 are parameter types. */, RoutingEvent Event /**< Value stating which event calls this Route. */) : this()
		{
			Subscriber = RouteSubscriber;
			Address = RoutingAddress;
			RouteEvent = Event;
		}

		/// \brief Checks if the attributes of the passed Route<R, T1, T2> are the same as the calling Route<R, T1, T2>'s attributes.
		/// \return Returns true if all attributes are the same, otherwise false.
		public bool Equals(Route<R, T1, T2> RT /**< Route to compare with the calling Route. */)
		{
			return ((this.Subscriber == RT.Subscriber) && (this.Address == RT.Address) && (this.RouteEvent == RT.RouteEvent));
		}

		/// \brief Checks if the passed object is the same as the calling Route<R, T1, T2>.
		/// \return Returns true if Obj is a Route<R, T1, T2> and all attributes are the same as the calling Route.\n
		/// \return Immediately returns false if Obj is not a Route<R, T1, T2> at all.\n
		public override bool Equals(System.Object Obj /**< Object to check for equivalency. */)
		{
			return ((Obj == typeof(Route<R, T1, T2>)) && (this.Subscriber == ((Route<R, T1, T2>)Obj).Subscriber) && (this.Address == ((Route<R, T1, T2>)Obj).Address) && (this.RouteEvent == ((Route<R, T1, T2>)Obj).RouteEvent));
		}

		/// \brief Returns a hash of this Route<R, T1, T2>.
		/// \returns Hash generated from combined member attribute hashes.
		public override int GetHashCode()
		{
			return (this.Subscriber.GetHashCode() + this.Address.GetHashCode() + this.RouteEvent.GetHashCode());
		}

		/// \brief Returns a string listing this Route<R, T1, T2>'s routing data.
		/// \returns A string containing the subscribing Component, the subscribing event and the callback function.
		public override string ToString()
		{
			return ("Subscriber: " + Address.Target + " | Event: " + RouteEvent.ToString() + " | Function: " + Address.Method);
		}

		/// \brief Compares two Route<R, T1, T2>'s for equivalent attributes.
		/// \returns True if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		/// \returns False if any attribute of the left-side operand is different from the respective attribute of the right-side operand.
		public static bool operator ==(Route<R, T1, T2> RT1 /**< Left-side operand */, Route<R, T1, T2> RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber == RT2.Subscriber) && (RT1.Address == RT2.Address) && (RT1.RouteEvent == RT2.RouteEvent));
		}

		/// \brief Contrasts two Route<R, T1, T2>'s for differing attributes.
		/// \returns True if any attribute of the left-side operand is different from the respective attribute of the right-side operand.\n
		/// \returns False if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		public static bool operator !=(Route<R, T1, T2> RT1 /**< Left-side operand */, Route<R, T1, T2> RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber != RT2.Subscriber) || (RT1.Address != RT2.Address) || (RT1.RouteEvent != RT2.RouteEvent));
		}
	}

	/** \brief Route that returns a value of type R and accepts one parameter of type T1, one of T2 and one of T3. */
	public struct Route<R, T1, T2, T3> : IEquatable<Route<R, T1, T2, T3>>
	{
		/// Reference to the originator of this route.
		public readonly Component Subscriber;
		/// Reference to the function this route calls.
		public readonly RoutePointer<R, T1, T2, T3> Address;
		/// Value for the RouteEvent that calls this Route's address.
		public readonly RoutingEvent RouteEvent;

		/// \brief Constructor that accepts a Component, a RoutePointer<R, T1, T2, T3> and a RoutingEvent.
		/// \warning Do not pass null as any parameters.\n
		/// \warning Router will discard any Routes with null attributes.
		public Route(Component RouteSubscriber /**< Reference to the subscribing Component.\n Can be of any type derived from Component. */, RoutePointer<R, T1, T2, T3> RoutingAddress /**< Reference to a function that this route calls.\n R is return type while T1, T2 and T3 are parameter types. */, RoutingEvent Event /**< Value stating which event calls this Route. */) : this()
		{
			Subscriber = RouteSubscriber;
			Address = RoutingAddress;
			RouteEvent = Event;
		}

		/// \brief Checks if the attributes of the passed Route<R, T1, T2, T3> are the same as the calling Route's attributes.
		/// \return Returns true if all attributes are the same, otherwise false.
		public bool Equals(Route<R, T1, T2, T3> RT /**< Route to compare with the calling Route. */)
		{
			return ((this.Subscriber == RT.Subscriber) && (this.Address == RT.Address) && (this.RouteEvent == RT.RouteEvent));
		}

		/// \brief Checks if the passed object is the same as the calling Route.
		/// \return Returns true if Obj is a Route<R, T1, T2, T3> and all attributes are the same as the calling Route<R, T1, T2, T3>.\n
		/// \return Immediately returns false if Obj is not a Route<R, T1, T2, T3> at all.\n
		public override bool Equals(System.Object Obj /**< Object to check for equivalency. */)
		{
			return ((Obj == typeof(Route<R, T1, T2, T3>)) && (this.Subscriber == ((Route<R, T1, T2, T3>)Obj).Subscriber) && (this.Address == ((Route<R, T1, T2, T3>)Obj).Address) && (this.RouteEvent == ((Route<R, T1, T2, T3>)Obj).RouteEvent));
		}

		/// \brief Returns a hash of this Route<R, T1, T2, T3>.
		/// \returns Hash generated from combined member attribute hashes.
		public override int GetHashCode()
		{
			return (this.Subscriber.GetHashCode() + this.Address.GetHashCode() + this.RouteEvent.GetHashCode());
		}

		/// \brief Returns a string listing this Route<R, T1, T2, T3>'s routing data.
		/// \returns A string containing the subscribing Component, the subscribing event and the callback function.
		public override string ToString()
		{
			return ("Subscriber: " + Address.Target + " | Event: " + RouteEvent.ToString() + " | Function: " + Address.Method);
		}

		/// \brief Compares two Route<R, T1, T2, T3>'s for equivalent attributes.
		/// \returns True if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		/// \returns False if any attribute of the left-side operand is different from the respective attribute of the right-side operand.
		public static bool operator ==(Route<R, T1, T2, T3> RT1 /**< Left-side operand */, Route<R, T1, T2, T3> RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber == RT2.Subscriber) && (RT1.Address == RT2.Address) && (RT1.RouteEvent == RT2.RouteEvent));
		}

		/// \brief Contrasts two Route<R, T1, T2, T3>'s for differing attributes.
		/// \returns True if any attribute of the left-side operand is different from the respective attribute of the right-side operand.\n
		/// \returns False if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		public static bool operator !=(Route<R, T1, T2, T3> RT1 /**< Left-side operand */, Route<R, T1, T2, T3> RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber != RT2.Subscriber) || (RT1.Address != RT2.Address) || (RT1.RouteEvent != RT2.RouteEvent));
		}
	}

	/** \brief Router Messaging System for Unity Engine */
	/// \details A replacement messaging system for Unity Engine built around not using blasted strings.
	/// \author Philip Muzzall
	/// \version 1.0.0
	/// \date 3/17/2014
	/// \copyright Property of Philip Muzzall
	/// \details Each Component must register with the Router before it can receive messages.\n
	/// To do this create a Route and register it with the Router using Router.AddRoute(Route NewRoute).\n
	/// Components can register multiple Routes with the Router to subscribe to multiple events.
	/// \note Router internally maintains routing tables for all registered Routes.\n
	/// \note These tables do not exists until the first Route has been registered.\n
	/// \note If all Routes are removed then Router will destroy these tables and recreate them again when they are needed.
	/// \todo Implement generic routes for passing and returning data.
	public static class Router
	{
		private static Dictionary<RoutingEvent, RoutePointer> RouteTable = null;
		private static HashSet<Route> ManifestTable = null;
		private static Stack<Route> DeadRoutes = null;
		private static bool TablesExist = false;

		/** \brief Registers a new Route with the Router. */
		/// \warning Any Routes with null attributes will not be registered.
		public static void AddRoute(Route NewRoute /**< Route to be registered. */)
		{
			TablesExist = (TablesExist? TablesExist : ConstructTables());

			bool KeyPresent = RouteTable.ContainsKey(NewRoute.RouteEvent), ValidRoute = ((NewRoute.Subscriber != null) && (NewRoute.Address != null) && (NewRoute.RouteEvent != RoutingEvent.Null));

			if (ValidRoute)
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

			TablesExist = (TablesExist? DeconstructTables() : TablesExist);
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
		public static void RouteMessageDescendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */)
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
		public static void RouteMessageDescendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component. */, RoutingEvent EventType /**< Type of event to send. */)
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
		public static void RouteMessageAscendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */)
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
		public static void RouteMessageAscendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component.*/, RoutingEvent EventType /**< Type of event to send. */)
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
			ManifestTable = (new HashSet<Route>());
			RouteTable = (new Dictionary<RoutingEvent, RoutePointer>());
			DeadRoutes = (new Stack<Route>());
			return ((ManifestTable != null) && (RouteTable != null) && (DeadRoutes != null));
		}

		private static bool DeconstructTables()
		{
			ManifestTable = ((ManifestTable.Count > 0)? ManifestTable : null);
			RouteTable = ((RouteTable.Count > 0)? RouteTable : null);
			DeadRoutes = ((ManifestTable != null) && (RouteTable != null))? DeadRoutes : null;
			return ((ManifestTable != null) && (RouteTable != null) && (DeadRoutes != null));
		}

		/// \internal Registration Functions

		private static void AttachAddress(Route RT)
		{
			RouteTable[RT.RouteEvent] = (RouteTable[RT.RouteEvent] + RT.Address);
		}

		private static void RemoveAddress(Route RT)
		{
			RouteTable[RT.RouteEvent] = (RouteTable[RT.RouteEvent] - RT.Address);

			if (!KeyHasValue(RT.RouteEvent))
			{
				RouteTable.Remove(RT.RouteEvent);
			}
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
			foreach (Route RT in ManifestTable)
			{
				if (RT.Subscriber == null)
				{
					DeadRoutes.Push(RT);
				}
			}

			ManifestTable.RemoveWhere(x => x.Subscriber == null);

			while (DeadRoutes.Count > 0)
			{
				PruneRoutes(DeadRoutes.Pop());
			}

			TablesExist = (TablesExist? DeconstructTables() : TablesExist);
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

	/** \brief Router that returns R from sending messages. */
	public static class Router<R>
	{
		private static Dictionary<RoutingEvent, RoutePointer<R>> RouteTable = null;
		private static HashSet<Route<R>> ManifestTable = null;
		private static Stack<Route<R>> DeadRoutes = null;
		private static bool TablesExist = false;

		/** \brief Registers a new Route with the Router. */
		/// \warning Any Routes with null attributes will not be registered.
		public static void AddRoute(Route<R> NewRoute /**< Route to be registered. */)
		{
			TablesExist = (TablesExist? TablesExist : ConstructTables());

			bool KeyPresent = RouteTable.ContainsKey(NewRoute.RouteEvent), ValidRoute = ((NewRoute.Subscriber != null) && (NewRoute.Address != null) && (NewRoute.RouteEvent != RoutingEvent.Null));

			if (ValidRoute)
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
		public static void RemoveRoute(Route<R> OldRoute /**< Route to be removed. */)
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

			TablesExist = (TablesExist? DeconstructTables() : TablesExist);
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

		/** \brief Routes a message of the specified event to all subscribers and returns their values. */
		/// \returns Returns an array of type R from all subscribers of this event type.
		/// \returns If the message fails to send then returns the default of R[].
		public static R[] RouteMessage(RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && KeyHasValue(EventType))
			{
				CleanDeadRoutes();

				RoutePointer<R>[] RPL = (RouteTable[EventType].GetInvocationList() as RoutePointer<R>[]);
				R[] Results = new R[RPL.Length];

				for (int i = 0; i < RPL.Length; i++)
				{
					Results[i] = RPL[i]();
				}

				return Results;
			}
			else
			{
				return default(R[]);
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.\n
		/// \returns Returns an array of type R from all subscribers of this event type.
		/// \returns If the message fails to send then returns the default of R[].
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static R[] RouteMessageDescendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope))
			{
				CleanDeadRoutes();

				List<R> Results = new List<R>(ManifestTable.Count);

				foreach (Route<R> RT in ManifestTable)
				{
					Results.Add(RouteDownwardsIfParent(Scope, EventType, RT));
				}

				Results.RemoveAll(x => (Equals(x, default(R))));

				return Results.ToArray();
			}
			else
			{
				return default(R[]);
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.\n
		/// Accepts a Component that is used to derive a reference to the target GameObject.\n
		/// \returns Returns an array of type R from all subscribers of this event type.
		/// \returns If the message fails to send then returns the default of R[].
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static R[] RouteMessageDescendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope.gameObject))
			{
				CleanDeadRoutes();

				List<R> Results = new List<R>(ManifestTable.Count);

				foreach (Route<R> RT in ManifestTable)
				{
					Results.Add(RouteDownwardsIfParent(Scope.gameObject, EventType, RT));
				}

				Results.RemoveAll(x => (Equals(x, default(R))));

				return Results.ToArray();
			}
			else
			{
				return default(R[]);
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.\n
		/// \returns Returns an array of type R from all subscribers of this event type.
		/// \returns If the message fails to send then returns the default of R[].
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static R[] RouteMessageAscendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope))
			{
				CleanDeadRoutes();

				List<R> Results = new List<R>(ManifestTable.Count);

				foreach (Route<R> RT in ManifestTable)
				{
					Results.Add(RouteUpwardsIfChild(Scope, EventType, RT));
				}

				Results.RemoveAll(x => (Equals(x, default(R))));

				return Results.ToArray();
			}
			else
			{
				return default(R[]);
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its parents. */
		/// Both direct and indirect parents of the specified GameObject receive the event.\n
		/// Accepts a Component that is used to derive a reference to the target GameObject.\n
		/// \returns Returns an array of type R from all subscribers of this event type.
		/// \returns If the message fails to send then returns the default of R[].
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static R[] RouteMessageAscendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component.*/, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Scope.gameObject))
			{
				CleanDeadRoutes();

				List<R> Results = new List<R>(ManifestTable.Count);

				foreach (Route<R> RT in ManifestTable)
				{
					Results.Add(RouteUpwardsIfChild(Scope.gameObject, EventType, RT));
				}

				Results.RemoveAll(x => (Equals(x, default(R))));

				return Results.ToArray();
			}
			else
			{
				return default(R[]);
			}
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// Uses the specified GameObject as the origin point.\n
		/// \returns Returns an array of type R from all subscribers of this event type.
		/// \returns If the message fails to send then returns the default of R[].
		/// \note Only works for subscribed GameObjects.
		public static R[] RouteMessageArea(GameObject Origin /**< GameObject specifying the origin of the event radius.\n Does not need to be subscribed unless it also is to receive the event. */, float Radius /**< Radius of the event in meters. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Origin))
			{
				CleanDeadRoutes();

				List<R> Results = new List<R>(ManifestTable.Count);

				foreach (Route<R> RT in ManifestTable)
				{
					Results.Add(RouteIfInRadius(Origin, EventType, RT, Radius));
				}

				Results.RemoveAll(x => (Equals(x, default(R))));

				return Results.ToArray();
			}
			else
			{
				return default(R[]);
			}
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// Uses the specified Component as the origin point.\n
		/// \returns Returns an array of type R from all subscribers of this event type.
		/// \returns If the message fails to send then returns the default of R[].
		/// \note Only works for subscribed GameObjects.
		public static R[] RouteMessageArea(Component Origin /**< Component specifying the origin of the event radius.\n Can be of any type derived from Component.\n Does not need to be subscribed unless it also is to receive the event. */, float Radius /**< Radius of the event in meters. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			if (TablesExist && ScopeIsValid(Origin.gameObject))
			{
				CleanDeadRoutes();

				List<R> Results = new List<R>(ManifestTable.Count);

				foreach (Route<R> RT in ManifestTable)
				{
					Results.Add(RouteIfInRadius(Origin.gameObject, EventType, RT, Radius));
				}

				Results.RemoveAll(x => (Equals(x, default(R))));

				return Results.ToArray();
			}
			else
			{
				return default(R[]);
			}
		}

		/** \internal *******************/
		/** \internal Utility Functions */
		/** \internal *******************/

		/// \internal Structors

		private static bool ConstructTables()
		{
			ManifestTable = (new HashSet<Route<R>>());
			RouteTable = (new Dictionary<RoutingEvent, RoutePointer<R>>());
			DeadRoutes = (new Stack<Route<R>>());
			return ((ManifestTable != null) && (RouteTable != null) && (DeadRoutes != null));
		}

		private static bool DeconstructTables()
		{
			ManifestTable = ((ManifestTable.Count > 0)? ManifestTable : null);
			RouteTable = ((RouteTable.Count > 0)? RouteTable : null);
			DeadRoutes = ((ManifestTable != null) && (RouteTable != null))? DeadRoutes : null;
			return ((ManifestTable != null) && (RouteTable != null) && (DeadRoutes != null));
		}

		/// \internal Registration Functions

		private static void AttachAddress(Route<R> RT)
		{
			RouteTable[RT.RouteEvent] = (RouteTable[RT.RouteEvent] + RT.Address);
		}

		private static void RemoveAddress(Route<R> RT)
		{
			RouteTable[RT.RouteEvent] = (RouteTable[RT.RouteEvent] - RT.Address);

			if (!KeyHasValue(RT.RouteEvent))
			{
				RouteTable.Remove(RT.RouteEvent);
			}
		}

		/// \internal Routing Functions

		private static R RouteDownwardsIfParent(GameObject Scope, RoutingEvent EventType, Route<R> RT)
		{
			return (((RT.RouteEvent == EventType) && RT.Subscriber.transform.IsChildOf(Scope.transform))? RT.Address() : default(R));
		}

		private static R RouteUpwardsIfChild(GameObject Scope, RoutingEvent EventType, Route<R> RT)
		{
			return (((RT.RouteEvent == EventType) && Scope.transform.IsChildOf(RT.Subscriber.transform))? RT.Address() : default(R));
		}

		private static R RouteIfInRadius(GameObject Scope, RoutingEvent EventType, Route<R> RT, float Radius)
		{
			return (((RT.RouteEvent == EventType) && ((new Decimal(Vector3.Distance(Scope.transform.position, RT.Subscriber.transform.position))) <= (new Decimal(Radius))))? RT.Address() : default(R));
		}

		/// \internal Janitorial Functions

		private static void CleanDeadRoutes()
		{
			foreach (Route<R> RT in ManifestTable)
			{
				if (RT.Subscriber == null)
				{
					DeadRoutes.Push(RT);
				}
			}

			ManifestTable.RemoveWhere(x => x.Subscriber == null);

			while (DeadRoutes.Count > 0)
			{
				PruneRoutes(DeadRoutes.Pop());
			}

			TablesExist = (TablesExist? DeconstructTables() : TablesExist);
		}

		private static void PruneRoutes(Route<R> RT)
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