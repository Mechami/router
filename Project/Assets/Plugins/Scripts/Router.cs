/** \mainpage Router Messaging System
\details A replacement messaging system for Unity Engine built around not using blasted strings.
\author Philip Muzzall
\version 1.0.0
\date 3/17/2014
\copyright Property of Philip Muzzall
\details Each Component must register with the Router before it can receive messages.\n
\details To do this create a Route and register it with the Router using Router.AddRoute(Route NewRoute).\n
\details Components can register multiple Routes with the Router to subscribe to multiple events.
\note Router internally maintains routing tables for all registered Routes.\n
\note These tables do not exists until the first Route has been registered.\n
\note If all Routes are removed then Router will destroy these tables and recreate them again when they are needed. */

using UnityEngine;
using System;
using System.Collections.Generic;

/** \brief Namespace for Router Messaging System */
namespace RouterMessagingSystem
{
	/** \brief Enumeration for event types. */
	public enum RoutingEvent : byte
	{
		Null = 0, ///< Default event type
		Test1 = 1, ///< Debug event 1
		Test2 = 2, ///< Debug event 2
		Test3 = 3, ///< Debug event 3
		Test4 = 4, ///< Debug event 4
		Test5 = 5, ///< Debug event 5
		Test6 = 6, ///< Debug event 6
		Test7 = 7, ///< Debug event 7
		Test8 = 8, ///< Debug event 8
		Test9 = 9, ///< Debug event 9
		Test10 = 10, ///< Debug event 10
		Test11 = 11, ///< Debug event 11
		Test12 = 12, ///< Debug event 12
		Test13 = 13, ///< Debug event 13
		Test14 = 14, ///< Debug event 14
		Test15 = 15, ///< Debug event 15
		Test16 = 16, ///< Debug event 16
	}

	/** \brief RoutePointer that doesn't accept or return any parameters. */
	public delegate void RoutePointer();

	/** \brief RoutePointer that returns an object of type R. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R>();

	/** \brief RoutePointer that returns an object and accepts one parameter. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R, in T1>(T1 P1 /**< Input parameter of type T. */);

	/** \brief RoutePointer that returns an object and accepts two parameters. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R, in T1, in T2>(T1 P1 /**< Input parameter of type T1. */, T2 P2 /**< Input parameter of type T2. */);

	/** \brief RoutePointer that returns an object and accepts three parameters. */
	/// \returns Returns object of type R.
	public delegate R RoutePointer<out R, in T1, in T2, in T3>(T1 P1 /**< Input parameter of type T1. */, T2 P2 /**< Input parameter of type T2. */, T3 P3 /**< Input parameter of type T3. */);

	/** \brief Route struct for use with Router Messaging System */
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

	/** \brief Struct that encapsulates two parameters to be passed to message recipients. */
	public struct RouteParameters<T1, T2>
	{
		/// The first parameter for the message.
		public readonly T1 FirstParameter;
		/// The second parameter for the message.
		public readonly T2 SecondParameter;

		/// \brief Constructor that accepts three parameters.
		public RouteParameters(T1 First /**< First parameter. */, T2 Second /**< Second parameter. */) : this()
		{
			FirstParameter = First;
			SecondParameter = Second;
		}

		/// \brief Returns a string listing this struct's parameters.
		/// \returns A string containing the parameters.
		public override string ToString()
		{
			return ("Parameter 1: " + FirstParameter + " | Parameter 2: " + SecondParameter);
		}
	}

	/** \brief Struct that encapsulates three parameters to be passed to message recipients. */
	public struct RouteParameters<T1, T2, T3>
	{
		/// The first parameter for the message.
		public readonly T1 FirstParameter;
		/// The second parameter for the message.
		public readonly T2 SecondParameter;
		/// The third parameter for the message.
		public readonly T3 ThirdParameter;

		/// \brief Constructor that accepts three parameters.
		public RouteParameters(T1 First /**< First parameter. */, T2 Second /**< Second parameter. */, T3 Third /**< Third parameter. */) : this()
		{
			FirstParameter = First;
			SecondParameter = Second;
			ThirdParameter = Third;
		}

		/// \brief Returns a string listing this struct's parameters.
		/// \returns A string containing the parameters.
		public override string ToString()
		{
			return ("Parameter 1: " + FirstParameter + " | Parameter 2: " + SecondParameter + " | Parameter 3: " + ThirdParameter);
		}
	}


	/** \brief Router that calls basic functions only. */
	/// \todo Redocument this.
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
		/// \warning Any Routes with null attributes will not be registered.
		public static void AddRoute(Route NewRoute /**< Route to be registered. */)
		{
			TablesExist = (TablesExist? TablesExist : ConstructTables());

			if (RouteIsValid(NewRoute) && !RouteIsRegistered(NewRoute))
			{
				RegisterRoute(NewRoute);
				AttachAddress(NewRoute);
			}
		}

		/** \brief Removes a Route from routing table. */
		public static void RemoveRoute(Route OldRoute /**< Route to be removed. */)
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

		/** \brief Returns the total amount of Routes registered with the Router. */
		/// \returns Uint representing the Routes registered.
		public static uint RouteCount()
		{
			if (TablesExist)
			{
				uint TotalRoutes = 0u;

				foreach (KeyValuePair<RoutingEvent, List<Route>> KVP in RouteTable)
				{
					TotalRoutes += (uint)KVP.Value.Count;
				}

				return TotalRoutes;
			}
			else
			{
				return 0u;
			}
		}

		/** \brief Routes a message of the specified event to all subscribers. */
		public static void RouteMessage(RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && KeyHasValue(EventType) && EventIsPopulated(EventType))
			{
				PointerTable[EventType]();
			}
		}

		/** \brief Routes a message of the specified event to the specified GameObject and its children. */
		/// Both direct and indirect children of the specified GameObject receive the event.\n
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
		/// Accepts a Component that is used to derive a reference to the target GameObject.\n
		/// \note Only works for subscribed GameObjects. Children must be subscribed as-well in order to receive the event.
		public static void RouteMessageDescendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid(Scope) && EventIsPopulated(EventType))
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
		/// Accepts a Component that is used to derive a reference to the target GameObject.\n
		/// \note Only works for subscribed GameObjects. Parents must be subscribed as-well in order to receive the event.
		public static void RouteMessageAscendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component.*/, RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid(Scope) && EventIsPopulated(EventType))
			{
				List<Route> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// Uses the specified GameObject as the origin point.\n
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageArea(GameObject Origin /**< GameObject specifying the origin of the event radius.\n Does not need to be subscribed unless it also is to receive the event. */, float Radius /**< Radius of the event in meters. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid(Origin) && EventIsPopulated(EventType))
			{
				decimal RadiusD = new Decimal(Radius);
				List<Route> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) <= RadiusD));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers in a radius. */
		/// Uses the specified Component as the origin point.\n
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageArea(Component Origin /**< Component specifying the origin of the event radius.\n Can be of any type derived from Component.\n Does not need to be subscribed unless it also is to receive the event. */, float Radius /**< Radius of the event in meters. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid(Origin) && EventIsPopulated(EventType))
			{
				decimal RadiusD = new Decimal(Radius);
				List<Route> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) <= RadiusD));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers outside the specified radius. */
		/// Uses the specified GameObject as the origin point.\n
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageAreaInverse(GameObject Origin /**< GameObject specifying the origin of the event radius.\n Does not need to be subscribed unless it also is to receive the event. */, float Radius /**< Radius of the event in meters. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid(Origin) && EventIsPopulated(EventType))
			{
				decimal RadiusD = new Decimal(Radius);
				List<Route> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) > RadiusD));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers outside the specified radius. */
		/// Uses the specified Component as the origin point.\n
		/// \note Only works for subscribed GameObjects.
		public static void RouteMessageAreaInverse(Component Origin /**< Component specifying the origin of the event radius.\n Can be of any type derived from Component.\n Does not need to be subscribed unless it also is to receive the event. */, float Radius /**< Radius of the event in meters. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid(Origin) && EventIsPopulated(EventType))
			{
				decimal RadiusD = new Decimal(Radius);
				List<Route> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) > RadiusD));
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers between an inner and outer radius. */
		/// Uses the specified Component as the origin point.\n
		/// \note Only works for subscribed GameObjects.
		/** \todo Clean up the parameter list. */
		public static void RouteMessageAreaBand(GameObject Origin /**< Component specifying the origin of the event radius.\n Can be of any type derived from Component.\n Does not need to be subscribed unless it also is to receive the event. */, float InnerRadius /**< Radius of the event in meters. */, float OuterRadius /**< Radius of the event in meters. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid(Origin) && EventIsPopulated(EventType))
			{
				decimal InnerRadiusD = new Decimal(InnerRadius), OuterRadiusD = new Decimal(OuterRadius);
				List<Route> RT = RouteTable[EventType].FindAll(x => { decimal Distance = new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)); return ((Distance >= InnerRadiusD) && (Distance <= OuterRadiusD)); });
				RT.ForEach(x => x.Address());
			}
		}

		/** \brief Routes a message of the specified event to all subscribers between an inner and outer radius. */
		/// Uses the specified Component as the origin point.\n
		/// \note Only works for subscribed GameObjects.
		/** \todo Clean up the parameter list. */
		public static void RouteMessageAreaBand(Component Origin /**< Component specifying the origin of the event radius.\n Can be of any type derived from Component.\n Does not need to be subscribed unless it also is to receive the event. */, float InnerRadius /**< Radius of the event in meters. */, float OuterRadius /**< Radius of the event in meters. */, RoutingEvent EventType /**< Type of event to send. */)
		{
			CleanDeadRoutes(EventType);

			if (TablesExist && ScopeIsValid(Origin) && EventIsPopulated(EventType))
			{
				decimal InnerRadiusD = new Decimal(InnerRadius), OuterRadiusD = new Decimal(OuterRadius);
				List<Route> RT = RouteTable[EventType].FindAll(x => { decimal Distance = new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)); return ((Distance >= InnerRadiusD) && (Distance <= OuterRadiusD)); });
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
			return ((RouteTable != null) && (PointerTable != null));
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
				RouteTable[RT.RouteEvent].Add(RT);
			}
			else
			{
				RouteTable[RT.RouteEvent].Add(RT);
			}
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
				PointerTable[RT.RouteEvent] =  (PointerTable[RT.RouteEvent] + RT.Address);
			}
		}

		private static void DetachAddress(Route RT)
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
			// Without the check this is supposedly at most a 3(O(n)) operation.
			// With the check this ranges from a max of 4(O(n)) to 1 O(n) operation.
			if (TablesExist && EventIsPopulated(EventType) && !RouteTable[EventType].TrueForAll(x => x.Subscriber != null))
			{
				Route[] DeadRoutes = RouteTable[EventType].ToArray();
				DeadRoutes = Array.FindAll(DeadRoutes, x => x.Subscriber == null);
				Array.ForEach(DeadRoutes, x => {DeregisterRoute(x); DetachAddress(x);});
				TablesExist = (TablesExist? DeconstructTables() : TablesExist);
			}
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

		private static bool RouteAddressIsAttached(Route RT)
		{
			return Array.Exists(PointerTable[RT.RouteEvent].GetInvocationList(), x => (x == RT.Address));
		}

		private static bool KeyHasValue(RoutingEvent EventType)
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

		private static bool ScopeIsValid(Component Scope)
		{
			return (Scope != null);
		}
	}

	/** \brief Router that returns R from sending messages. */
	/// \todo Redocument this.
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

	/** \brief Router that returns R from sending messages and accepts one parameter. */
	/// \todo Redocument this.
	public static class Router<R, T>
	{
		private static Dictionary<RoutingEvent, RoutePointer<R, T>> PointerTable = null;
		private static Dictionary<RoutingEvent, List<Route<R, T>>> RouteTable = null;
		private static bool TablesExist = false;

		static Router()
		{
			Debug.Log("Router<" + typeof(R) + ", " + typeof(T) + "> initialized!");
		}

		/** \brief Registers a new Route with the Router. */
		/// \warning Any Routes with null attributes will not be registered.
		public static void AddRoute(Route<R, T> NewRoute /**< Route to be registered. */)
		{
			TablesExist = (TablesExist? TablesExist : ConstructTables());

			if ((NewRoute.Subscriber != null) && (NewRoute.Address != null) && (NewRoute.RouteEvent != RoutingEvent.Null))
			{
				RegisterRoute(NewRoute);
				AttachAddress(NewRoute);
			}
		}

		/** \brief Removes a Route from routing table. */
		public static void RemoveRoute(Route<R, T> OldRoute /**< Route to be removed. */)
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
		public static List<R> RouteMessage(RoutingEvent EventType /**< Type of event to send. */, T Parameter)
		{
			if (TablesExist && KeyHasValue(EventType) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				Delegate[] RPL = PointerTable[EventType].GetInvocationList();
				R[] Results = new R[RPL.Length];

				for (int i = 0; i < RPL.Length; i++)
				{
					Results[i] = (RPL[i] as RoutePointer<R, T>)(Parameter);
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
		public static List<R> RouteMessageDescendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */, T Parameter)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T>> RT = RouteTable[EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameter);
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
		public static List<R> RouteMessageDescendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component. */, RoutingEvent EventType /**< Type of event to send. */, T Parameter)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T>> RT = RouteTable[EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameter);
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
		public static List<R> RouteMessageAscendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */, T Parameter)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T>> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameter);
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
		public static List<R> RouteMessageAscendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component.*/, RoutingEvent EventType /**< Type of event to send. */, T Parameter)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T>> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameter);
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
		public static List<R> RouteMessageArea(GameObject Origin /**< GameObject specifying the origin of the event radius.\n Does not need to be subscribed unless it also is to receive the event. */, RoutingEvent EventType /**< Type of event to send. */, float Radius /**< Radius of the event in meters. */, T Parameter)
		{
			if (TablesExist && ScopeIsValid(Origin) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);
				decimal RadiusD = (new Decimal(Radius));

				List<Route<R, T>> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) <= RadiusD));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameter);
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
		public static List<R> RouteMessageArea(Component Origin /**< Component specifying the origin of the event radius.\n Can be of any type derived from Component.\n Does not need to be subscribed unless it also is to receive the event. */, RoutingEvent EventType /**< Type of event to send. */, float Radius /**< Radius of the event in meters. */, T Parameter)
		{
			if (TablesExist && ScopeIsValid(Origin.gameObject) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);
				decimal RadiusD = (new Decimal(Radius));

				List<Route<R, T>> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) <= RadiusD));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameter);
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
			RouteTable = new Dictionary<RoutingEvent, List<Route<R, T>>>();
			PointerTable = new Dictionary<RoutingEvent, RoutePointer<R, T>>();
			return ((RouteTable != null) && (PointerTable != null));
		}

		private static bool DeconstructTables()
		{
			RouteTable = ((RouteTable.Count > 0)? RouteTable : null);
			PointerTable = ((PointerTable.Count > 0)? PointerTable : null);
			return ((RouteTable != null) && (PointerTable != null));
		}

		/// \internal Registration Functions

		private static void RegisterRoute(Route<R, T> RT)
		{
			if (!RouteTable.ContainsKey(RT.RouteEvent))
			{
				RouteTable.Add(RT.RouteEvent, new List<Route<R, T>>());
			}
			else
			{
				RouteTable[RT.RouteEvent].Add(RT);
			}
		}

		private static void DeregisterRoute(Route<R, T> RT)
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

		private static void AttachAddress(Route<R, T> RT)
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

		private static void DetachAddress(Route<R, T> RT)
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
				Route<R, T>[] DeadRoutes = RouteTable[EventType].ToArray();
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

	/** \brief Router that returns R from sending messages and accepts two parameters. */
	/// \todo Redocument this.
	public static class Router<R, T1, T2>
	{
		private static Dictionary<RoutingEvent, RoutePointer<R, T1, T2>> PointerTable = null;
		private static Dictionary<RoutingEvent, List<Route<R, T1, T2>>> RouteTable = null;
		private static bool TablesExist = false;

		static Router()
		{
			Debug.Log("Router<" + typeof(R) + ", " + typeof(T1) +  ", " + typeof(T2) + "> initialized!");
		}

		/** \brief Registers a new Route with the Router. */
		/// \warning Any Routes with null attributes will not be registered.
		public static void AddRoute(Route<R, T1, T2> NewRoute /**< Route to be registered. */)
		{
			TablesExist = (TablesExist? TablesExist : ConstructTables());

			if ((NewRoute.Subscriber != null) && (NewRoute.Address != null) && (NewRoute.RouteEvent != RoutingEvent.Null))
			{
				RegisterRoute(NewRoute);
				AttachAddress(NewRoute);
			}
		}

		/** \brief Removes a Route from routing table. */
		public static void RemoveRoute(Route<R, T1, T2> OldRoute /**< Route to be removed. */)
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
		public static List<R> RouteMessage(RoutingEvent EventType /**< Type of event to send. */, RouteParameters<T1, T2> Parameters)
		{
			if (TablesExist && KeyHasValue(EventType) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				Delegate[] RPL = PointerTable[EventType].GetInvocationList();
				R[] Results = new R[RPL.Length];

				for (int i = 0; i < RPL.Length; i++)
				{
					Results[i] = (RPL[i] as RoutePointer<R, T1, T2>)(Parameters.FirstParameter, Parameters.SecondParameter);
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
		public static List<R> RouteMessageDescendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */, RouteParameters<T1, T2> Parameters)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T1, T2>> RT = RouteTable[EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter);
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
		public static List<R> RouteMessageDescendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component. */, RoutingEvent EventType /**< Type of event to send. */, RouteParameters<T1, T2> Parameters)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T1, T2>> RT = RouteTable[EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter);
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
		public static List<R> RouteMessageAscendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */, RouteParameters<T1, T2> Parameters)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T1, T2>> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter);
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
		public static List<R> RouteMessageAscendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component.*/, RoutingEvent EventType /**< Type of event to send. */, RouteParameters<T1, T2> Parameters)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T1, T2>> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter);
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
		public static List<R> RouteMessageArea(GameObject Origin /**< GameObject specifying the origin of the event radius.\n Does not need to be subscribed unless it also is to receive the event. */, RoutingEvent EventType /**< Type of event to send. */, float Radius /**< Radius of the event in meters. */, RouteParameters<T1, T2> Parameters)
		{
			if (TablesExist && ScopeIsValid(Origin) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);
				decimal RadiusD = (new Decimal(Radius));

				List<Route<R, T1, T2>> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) <= RadiusD));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter);
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
		public static List<R> RouteMessageArea(Component Origin /**< Component specifying the origin of the event radius.\n Can be of any type derived from Component.\n Does not need to be subscribed unless it also is to receive the event. */, RoutingEvent EventType /**< Type of event to send. */, float Radius /**< Radius of the event in meters. */, RouteParameters<T1, T2> Parameters)
		{
			if (TablesExist && ScopeIsValid(Origin.gameObject) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);
				decimal RadiusD = (new Decimal(Radius));

				List<Route<R, T1, T2>> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) <= RadiusD));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter);
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
			RouteTable = new Dictionary<RoutingEvent, List<Route<R, T1, T2>>>();
			PointerTable = new Dictionary<RoutingEvent, RoutePointer<R, T1, T2>>();
			return ((RouteTable != null) && (PointerTable != null));
		}

		private static bool DeconstructTables()
		{
			RouteTable = ((RouteTable.Count > 0)? RouteTable : null);
			PointerTable = ((PointerTable.Count > 0)? PointerTable : null);
			return ((RouteTable != null) && (PointerTable != null));
		}

		/// \internal Registration Functions

		private static void RegisterRoute(Route<R, T1, T2> RT)
		{
			if (!RouteTable.ContainsKey(RT.RouteEvent))
			{
				RouteTable.Add(RT.RouteEvent, new List<Route<R, T1, T2>>());
			}
			else
			{
				RouteTable[RT.RouteEvent].Add(RT);
			}
		}

		private static void DeregisterRoute(Route<R, T1, T2> RT)
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

		private static void AttachAddress(Route<R, T1, T2> RT)
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

		private static void DetachAddress(Route<R, T1, T2> RT)
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
				Route<R, T1, T2>[] DeadRoutes = RouteTable[EventType].ToArray();
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

	/** \brief Router that returns R from sending messages and accepts three parameters. */
	/// \todo Redocument this.
	public static class Router<R, T1, T2, T3>
	{
		private static Dictionary<RoutingEvent, RoutePointer<R, T1, T2, T3>> PointerTable = null;
		private static Dictionary<RoutingEvent, List<Route<R, T1, T2, T3>>> RouteTable = null;
		private static bool TablesExist = false;

		static Router()
		{
			Debug.Log("Router<" + typeof(R) + ", " + typeof(T1) +  ", " + typeof(T2) +  ", " + typeof(T3) + "> initialized!");
		}

		/** \brief Registers a new Route with the Router. */
		/// \warning Any Routes with null attributes will not be registered.
		public static void AddRoute(Route<R, T1, T2, T3> NewRoute /**< Route to be registered. */)
		{
			TablesExist = (TablesExist? TablesExist : ConstructTables());

			if ((NewRoute.Subscriber != null) && (NewRoute.Address != null) && (NewRoute.RouteEvent != RoutingEvent.Null))
			{
				RegisterRoute(NewRoute);
				AttachAddress(NewRoute);
			}
		}

		/** \brief Removes a Route from routing table. */
		public static void RemoveRoute(Route<R, T1, T2, T3> OldRoute /**< Route to be removed. */)
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
		public static List<R> RouteMessage(RoutingEvent EventType /**< Type of event to send. */, RouteParameters<T1, T2, T3> Parameters /**< Struct containing parameters to pass to recipients. */)
		{
			if (TablesExist && KeyHasValue(EventType) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				Delegate[] RPL = PointerTable[EventType].GetInvocationList();
				R[] Results = new R[RPL.Length];

				for (int i = 0; i < RPL.Length; i++)
				{
					Results[i] = (RPL[i] as RoutePointer<R, T1, T2, T3>)(Parameters.FirstParameter, Parameters.SecondParameter, Parameters.ThirdParameter);
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
		public static List<R> RouteMessageDescendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */, RouteParameters<T1, T2, T3> Parameters /**< Struct containing parameters to pass to recipients. */)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T1, T2, T3>> RT = RouteTable[EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter, Parameters.ThirdParameter);
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
		public static List<R> RouteMessageDescendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component. */, RoutingEvent EventType /**< Type of event to send. */, RouteParameters<T1, T2, T3> Parameters /**< Struct containing parameters to pass to recipients. */)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T1, T2, T3>> RT = RouteTable[EventType].FindAll(x => x.Subscriber.transform.IsChildOf(Scope.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter, Parameters.ThirdParameter);
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
		public static List<R> RouteMessageAscendants(GameObject Scope /**< GameObject specifying the scope of the message. */, RoutingEvent EventType /**< Type of event to send. */, RouteParameters<T1, T2, T3> Parameters /**< Struct containing parameters to pass to recipients. */)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T1, T2, T3>> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter, Parameters.ThirdParameter);
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
		public static List<R> RouteMessageAscendants(Component Scope /**< Component specifying the scope of the message.\n Can be of any type derived from Component.*/, RoutingEvent EventType /**< Type of event to send. */, RouteParameters<T1, T2, T3> Parameters /**< Struct containing parameters to pass to recipients. */)
		{
			if (TablesExist && ScopeIsValid(Scope) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);

				List<Route<R, T1, T2, T3>> RT = RouteTable[EventType].FindAll(x => Scope.transform.IsChildOf(x.Subscriber.transform));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter, Parameters.ThirdParameter);
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
		public static List<R> RouteMessageArea(GameObject Origin /**< GameObject specifying the origin of the event radius.\n Does not need to be subscribed unless it also is to receive the event. */, RoutingEvent EventType /**< Type of event to send. */, float Radius /**< Radius of the event in meters. */, RouteParameters<T1, T2, T3> Parameters /**< Struct containing parameters to pass to recipients. */)
		{
			if (TablesExist && ScopeIsValid(Origin) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);
				decimal RadiusD = (new Decimal(Radius));

				List<Route<R, T1, T2, T3>> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) <= RadiusD));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter, Parameters.ThirdParameter);
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
		public static List<R> RouteMessageArea(Component Origin /**< Component specifying the origin of the event radius.\n Can be of any type derived from Component.\n Does not need to be subscribed unless it also is to receive the event. */, RoutingEvent EventType /**< Type of event to send. */, float Radius /**< Radius of the event in meters. */, RouteParameters<T1, T2, T3> Parameters /**< Struct containing parameters to pass to recipients. */)
		{
			if (TablesExist && ScopeIsValid(Origin.gameObject) && EventIsRegistered(EventType))
			{
				CleanDeadRoutes(EventType);
				decimal RadiusD = (new Decimal(Radius));

				List<Route<R, T1, T2, T3>> RT = RouteTable[EventType].FindAll(x => (new Decimal(Vector3.Distance(Origin.transform.position, x.Subscriber.transform.position)) <= RadiusD));
				R[] Results = new R[RT.Count];

				for (int i = 0; i < RT.Count; i++)
				{
					Results[i] = RT[i].Address(Parameters.FirstParameter, Parameters.SecondParameter, Parameters.ThirdParameter);
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
			RouteTable = new Dictionary<RoutingEvent, List<Route<R, T1, T2, T3>>>();
			PointerTable = new Dictionary<RoutingEvent, RoutePointer<R, T1, T2, T3>>();
			return ((RouteTable != null) && (PointerTable != null));
		}

		private static bool DeconstructTables()
		{
			RouteTable = ((RouteTable.Count > 0)? RouteTable : null);
			PointerTable = ((PointerTable.Count > 0)? PointerTable : null);
			return ((RouteTable != null) && (PointerTable != null));
		}

		/// \internal Registration Functions

		private static void RegisterRoute(Route<R, T1, T2, T3> RT)
		{
			if (!RouteTable.ContainsKey(RT.RouteEvent))
			{
				RouteTable.Add(RT.RouteEvent, new List<Route<R, T1, T2, T3>>());
			}
			else
			{
				RouteTable[RT.RouteEvent].Add(RT);
			}
		}

		private static void DeregisterRoute(Route<R, T1, T2, T3> RT)
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

		private static void AttachAddress(Route<R, T1, T2, T3> RT)
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

		private static void DetachAddress(Route<R, T1, T2, T3> RT)
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
				Route<R, T1, T2, T3>[] DeadRoutes = RouteTable[EventType].ToArray();
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