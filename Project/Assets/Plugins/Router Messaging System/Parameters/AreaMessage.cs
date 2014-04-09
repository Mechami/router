using UnityEngine;
using System;

namespace RouterMessagingSystem
{
	/** \brief Struct for passing parameters to Router for broadcasting area messages. */
	public struct AreaMessage : IEquatable<AreaMessage>, IComparable<AreaMessage>
	{
		/** \brief Vector3 representing the origin coordinate for the area. */
		public readonly Vector3 Origin;
		/** \brief Radius of the area in meters. */
		public readonly float Radius;
		/** \brief Event occuring in the area. */
		public readonly RoutingEvent AreaEvent;

		/** \brief Standard constructor for new AreaMessages. */
		AreaMessage(ref Vector3 OriginCoord, ref float AreaRadius, ref RoutingEvent Event)
		{
			Origin = OriginCoord;
			Radius = Mathf.Abs(AreaRadius);
			AreaEvent = Event;
		}

		/** \brief Constructor that constructs a new AreaMessage from a Vector3 and another AreaMessage. */
		AreaMessage(ref Vector3 OriginCoord, ref AreaMessage AM)
		{
			Origin = OriginCoord;
			Radius = AM.Radius;
			AreaEvent = AM.AreaEvent;
		}

		/** \brief Constructor that constructs a new AreaMessage from a float and another AreaMessage. */
		AreaMessage(ref float AreaRadius, ref AreaMessage AM)
		{
			Origin = AM.Origin;
			Radius = Mathf.Abs(AreaRadius);
			AreaEvent = AM.AreaEvent;
		}

		/** \brief Constructor that constructs a new AreaMessage from a RoutingEvent and another AreaMessage. */
		AreaMessage(ref RoutingEvent Event, ref AreaMessage AM)
		{
			Origin = AM.Origin;
			Radius = AM.Radius;
			AreaEvent = Event;
		}

		/// \brief Compares the calling AreaMessage's attributes to the specified AreaMessage's attributes.
		/// \return Returns true if all attributes are the same, otherwise false.
		public bool Equals(AreaMessage AM /**< AreaMessage to compare with the calling AreaMessage. */)
		{
			return ((this.Origin == AM.Origin) && (this.Radius == AM.Radius) && (this.AreaEvent == AM.AreaEvent));
		}

		/// \brief Compares the passed object to the calling AreaMessage.
		/// \return Returns true if Obj is an AreaMessage and all attributes are equivalent.\n
		/// \return Immediately returns false if Obj is not a AreaMessage at all.
		public override bool Equals(System.Object Obj /**< Object to check for equivalency. */)
		{
			return ((Obj is AreaMessage) && (this.Origin == ((AreaMessage)Obj).Origin) && (this.Radius == ((AreaMessage)Obj).Radius) && (this.AreaEvent == ((AreaMessage)Obj).AreaEvent));
		}

		/// \brief Compares the calling AreaMessage's radius to the specified AreaMessage's radius.
		/// \return Returns positive if the caller has a larger radius, negative if a smaller radius and zero if equivalent.
		public int CompareTo(AreaMessage AM)
		{
			decimal OurRadius = new Decimal(this.Radius), TheirRadius = new Decimal(AM.Radius);
			return ((OurRadius < TheirRadius)? -1 : ((OurRadius > TheirRadius)? 1 : 0));
		}

		/// \brief Returns a hash of this AreaMessage.
		/// \returns Hash generated from combined member attribute hashes.
		public override int GetHashCode()
		{
			return (this.Origin.GetHashCode() + this.Radius.GetHashCode() + this.AreaEvent.GetHashCode());
		}

		/// \brief Returns a string listing this AreaMessage's parameters.
		/// \returns A string containing the origin coordinate, the message radius and the event type.
		public override string ToString()
		{
			return ("[" + Origin + ", " + Radius + ", " + AreaEvent + "]");
		}

		/// \brief Compares two AreaMessages for equivalent attributes.
		/// \returns True if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.\n
		/// \returns False if any attribute of the left-side operand is different from the respective attribute of the right-side operand.
		public static bool operator ==(AreaMessage AM1 /**< Left-side operand */, AreaMessage AM2 /**< Right-side operand */)
		{
			return ((AM1.Origin == AM2.Origin) && (AM1.Radius == AM2.Radius) && (AM1.AreaEvent == AM2.AreaEvent));
		}

		/// \brief Contrasts two AreaMessages for differing attributes.
		/// \returns True if any attribute of the left-side operand is different from the respective attribute of the right-side operand.\n
		/// \returns False if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		public static bool operator !=(AreaMessage AM1 /**< Left-side operand */, AreaMessage AM2 /**< Right-side operand */)
		{
			return ((AM1.Origin != AM2.Origin) || (AM1.Radius != AM2.Radius) || (AM1.AreaEvent != AM2.AreaEvent));
		}

		/// \brief Compares two AreaMessages for the smaller radius.
		/// \returns True if the left-side operand is less than the right-side operand, otherwise false.
		public static bool operator <(AreaMessage AM1 /**< Left-side operand */, AreaMessage AM2 /**< Right-side operand */)
		{
			return (new Decimal(AM1.Radius) < new Decimal(AM2.Radius));
		}

		/// \brief Compares two AreaMessages for the larger radius.
		/// \returns True if the left-side operand is greater than the right-side operand, otherwise false.
		public static bool operator >(AreaMessage AM1 /**< Left-side operand */, AreaMessage AM2 /**< Right-side operand */)
		{
			return (new Decimal(AM1.Radius) > new Decimal(AM2.Radius));
		}

		/// \brief Compares two AreaMessages for an equivalent or smaller radius.
		/// \returns True if the left-side operand is less than or equal to the right-side operand, otherwise false.
		public static bool operator <=(AreaMessage AM1 /**< Left-side operand */, AreaMessage AM2 /**< Right-side operand */)
		{
			return (new Decimal(AM1.Radius) <= new Decimal(AM2.Radius));
		}

		/// \brief Compares two AreaMessages for an equivalent or larger radius.
		/// \returns True if the left-side operand is greater than or equal to the right-side operand, otherwise false.
		public static bool operator >=(AreaMessage AM1 /**< Left-side operand */, AreaMessage AM2 /**< Right-side operand */)
		{
			return (new Decimal(AM1.Radius) >= new Decimal(AM2.Radius));
		}
	}
}