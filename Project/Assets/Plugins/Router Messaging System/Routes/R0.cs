using UnityEngine;
using System;

namespace RouterMessagingSystem
{
	/// \brief A route to a function that accepts no parameters and returns an object of type R.
	public struct Route<R> : IRoutable<Route<R>>
	{
		/// Reference to the originator of this route.
		public readonly Component Subscriber;
		/// Reference to the function this route calls.
		public readonly RoutePointer<R> Address;
		/// Value for the RouteEvent that calls this route's address.
		public readonly RoutingEvent RouteEvent;
		/// Value representing the validity of this route at creation.
		public readonly bool IsValid;
		/// Value representing the current status of this route.
		public bool IsDead
		{
			get
			{
				return (this.Subscriber != null);
			}
		}

		/// \brief Constructor that accepts a Component, a RoutePointer<R> and a RoutingEvent.
		/// \warning Do not pass null as any parameters.\n
		/// \warning Router will discard any Routes with null attributes.
		public Route(Component RouteSubscriber /**< Reference to the subscribing Component.\n Can be of any type derived from Component. */, RoutePointer<R> RoutingAddress /**< Reference to a function that this route calls.\n R is return type. */, RoutingEvent Event /**< Value stating which event calls this Route. */) : this()
		{
			Subscriber = RouteSubscriber;
			Address = (RoutePointer<R>)(RoutingAddress.GetInvocationList()[0]);
			RouteEvent = Event;
			IsValid = ((this.Subscriber != null) && (this.Address != null) && (this.RouteEvent != RoutingEvent.Null));
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
			bool Result = false;

			if (Obj is Route<R>)
			{
				Route<R> That = (Route<R>)Obj;
				Result = ((this.Subscriber == That.Subscriber) && (this.Address == That.Address) && (this.RouteEvent == That.RouteEvent));
			}

			return Result;
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
			return ("[" + ((Subscriber != null)? Subscriber.ToString() : "Null") + ", " + RouteEvent.ToString() + ", " + ((Address != null)? Address.Method.ToString() : "Null") + "]");
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
}