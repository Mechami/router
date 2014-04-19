using UnityEngine;
using System;

namespace RouterMessagingSystem
{
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
			return ((Obj is Route<R, T1, T2, T3>) && (this.Subscriber == ((Route<R, T1, T2, T3>)Obj).Subscriber) && (this.Address == ((Route<R, T1, T2, T3>)Obj).Address) && (this.RouteEvent == ((Route<R, T1, T2, T3>)Obj).RouteEvent));
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
			return ("[" + ((Subscriber != null)? Subscriber.ToString() : "Null") + ", " + RouteEvent.ToString() + ", " + ((Address != null)? Address.Method.ToString() : "Null") + "]");
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
}