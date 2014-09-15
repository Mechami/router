using UnityEngine;
using System;

namespace RouterMessagingSystem
{
	/// \brief A route to a function that accepts no parameters and returns nothing.
	/// \details This is the standard route.
	public struct Route : IRoutable<Route>
	{
		/// Reference to the originator of this route.
		public readonly Component Subscriber;
		/// Reference to the function this route calls.
		public readonly RoutePointer Address;
		/// Value for the RouteEvent that calls this route's address.
		public readonly RoutingEvent RouteEvent;
		/// Value representing the validity of this route at creation.
		public readonly bool IsValid;
		/// Value representing the current status of this route.
		public bool IsDead
		{
			get
			{
				return (this.Subscriber == null);
			}
		}

		/// \brief Constructor that accepts a Component, a RoutePointer and a RoutingEvent.
		/// \warning Do not pass null as any parameters.\n
		/// \warning Router will discard any Routes with null attributes.
		public Route(Component RouteSubscriber /**< Reference to the subscribing Component.\n Can be of any type derived from Component. */, RoutePointer RoutingAddress /**< Reference to a function that this route calls.\n Must return void and accept no parameters. */, RoutingEvent Event /**< Value stating which event calls this Route. */) : this()
		{
			Subscriber = RouteSubscriber;
			RouteEvent = Event;

			Address = (RoutingAddress != null)?
				(RoutePointer)(RoutingAddress.GetInvocationList()[0]) :
				null;

			IsValid = (this.Subscriber != null) &&
					  (this.Address != null) &&
					  (((Component)this.Address.Target) == this.Subscriber) &&
					  (this.RouteEvent != RoutingEvent.Null);
		}

		/// \brief Checks if the attributes of the passed Route are the same as the calling Route's attributes.
		/// \return Returns true if all attributes are the same, otherwise false.
		public bool Equals(Route RT /**< Route to compare with the calling Route. */)
		{
			return (this.Subscriber == RT.Subscriber) &&
				   (this.Address == RT.Address) &&
				   (this.RouteEvent == RT.RouteEvent);
		}

		/// \brief Checks if the passed object is the same as the calling Route.
		/// \return Returns true if Obj is a Route and all attributes are the same as the calling Route.\n
		/// \return Immediately returns false if Obj is not a Route at all.
		public override bool Equals(System.Object Obj /**< Object to check for equivalency. */)
		{
			bool Result = false;

			if (Obj is Route)
			{
				Route That = (Route)Obj;
				Result = (this.Subscriber == That.Subscriber) &&
						 (this.Address == That.Address) &&
						 (this.RouteEvent == That.RouteEvent);
			}

			return Result;
		}

		/// \brief Returns a hash of this Route.
		/// \returns Hash generated from combined member attribute hashes.
		public override int GetHashCode()
		{
			return ((Subscriber != null)? Subscriber.GetHashCode() : 0) + 
				   ((Address != null)? Address.GetHashCode() : 0) +
				     RouteEvent.GetHashCode();
		}

		/// \brief Returns a string listing this Route's routing data.
		/// \returns A string containing the subscribing Component, the subscribing event and the callback function.
		public override string ToString()
		{
			return "[" +
				   ((Subscriber != null)? Subscriber.ToString() : "Null") +
				   ", " + RouteEvent.ToString() + ", " +
				   ((Address != null)? Address.Method.ToString() : "Null") +
				   "]";
		}

		/// \brief Compares two Routes for equivalent attributes.
		/// \returns True if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		/// \returns False if any attribute of the left-side operand is different from the respective attribute of the right-side operand.
		public static bool operator ==(Route RT1 /**< Left-side operand */, Route RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber == RT2.Subscriber) &&
					(RT1.Address == RT2.Address) &&
					(RT1.RouteEvent == RT2.RouteEvent));
		}

		/// \brief Contrasts two Routes for differing attributes.
		/// \returns True if any attribute of the left-side operand is different from the respective attribute of the right-side operand.\n
		/// \returns False if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		public static bool operator !=(Route RT1 /**< Left-side operand */, Route RT2 /**< Right-side operand */)
		{
			return ((RT1.Subscriber != RT2.Subscriber) ||
					(RT1.Address != RT2.Address) ||
					(RT1.RouteEvent != RT2.RouteEvent));
		}
	}
}
