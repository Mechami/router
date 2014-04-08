using UnityEngine;
using System;

namespace RouterMessagingSystem
{
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
			return ((Obj is Route) && (this.Subscriber == ((Route)Obj).Subscriber) && (this.Address == ((Route)Obj).Address) && (this.RouteEvent == ((Route)Obj).RouteEvent));
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
			return ("[" + Address.Target + ", " + RouteEvent + ", " + Address.Method + "]");
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
}