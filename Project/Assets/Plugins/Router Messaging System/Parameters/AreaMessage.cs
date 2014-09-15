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
		public readonly RoutingEvent EventType;
		/** \brief Specify whether this area message should check for occlusion before sending messages. */
		public bool DoRayCheck;
		/** \brief Check outside the specified radius instead of inside. */
		public bool UseInverse;

		/** \brief Standard constructor for new AreaMessages. */
		public AreaMessage(Vector3 OriginCoord, float AreaRadius, RoutingEvent Event) : this()
		{
			float AbsR = Mathf.Abs(AreaRadius);
			Origin = OriginCoord;
			Radius = (AbsR < 0.5f)? 0.5f : AbsR;
			EventType = Event;
			DoRayCheck = false;
			UseInverse = false;
		}

		/** \brief Constructor that constructs a new AreaMessage from a Vector3 and another AreaMessage. */
		public AreaMessage(Vector3 OriginCoord, AreaMessage AM) : this()
		{
			Origin = OriginCoord;
			Radius = AM.Radius;
			EventType = AM.EventType;
			DoRayCheck = AM.DoRayCheck;
			UseInverse = AM.UseInverse;
		}

		/** \brief Constructor that constructs a new AreaMessage from a float and another AreaMessage. */
		public AreaMessage(float AreaRadius, AreaMessage AM) : this()
		{
			float AbsR = Mathf.Abs(AreaRadius);
			Origin = AM.Origin;
			Radius = (AbsR < 0.5f)? 0.5f : AbsR;
			EventType = AM.EventType;
			DoRayCheck = AM.DoRayCheck;
			UseInverse = AM.UseInverse;
		}

		/** \brief Constructor that constructs a new AreaMessage from a RoutingEvent and another AreaMessage. */
		public AreaMessage(RoutingEvent Event, AreaMessage AM) : this()
		{
			Origin = AM.Origin;
			Radius = AM.Radius;
			EventType = Event;
			DoRayCheck = AM.DoRayCheck;
			UseInverse = AM.UseInverse;
		}

		/// \brief Compares the calling AreaMessage's attributes to the specified AreaMessage's attributes.
		/// \return Returns true if all attributes are the same, otherwise false.
		public bool Equals(AreaMessage AM /**< AreaMessage to compare with the calling AreaMessage. */)
		{
			return ((this.Origin == AM.Origin) && (new Decimal(this.Radius) == new Decimal(AM.Radius)) && (this.EventType == AM.EventType));
		}

		/// \brief Compares the passed object to the calling AreaMessage.
		/// \return Returns true if Obj is an AreaMessage and all attributes are equivalent.\n
		/// \return Immediately returns false if Obj is not an AreaMessage at all.
		public override bool Equals(System.Object Obj /**< Object to check for equivalency. */)
		{
			return ((Obj is AreaMessage) && (this.Origin == ((AreaMessage)Obj).Origin) && (new Decimal(this.Radius) == new Decimal(((AreaMessage)Obj).Radius)) && (this.EventType == ((AreaMessage)Obj).EventType));
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
			return (this.Origin.GetHashCode() + this.Radius.GetHashCode() + this.EventType.GetHashCode());
		}

		/// \brief Returns a string listing this AreaMessage's parameters.
		/// \returns A string containing the origin coordinate, the message radius and the event type.
		public override string ToString()
		{
			return ("[" + Origin + ", " + Radius + ", " + EventType + "]");
		}

		/// \brief Compares two AreaMessages for equivalent attributes.
		/// \returns True if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.\n
		/// \returns False if any attribute of the left-side operand is different from the respective attribute of the right-side operand.
		public static bool operator ==(AreaMessage AM1 /**< Left-side operand */, AreaMessage AM2 /**< Right-side operand */)
		{
			return ((AM1.Origin == AM2.Origin) && (new Decimal(AM1.Radius) == new Decimal(AM2.Radius)) && (AM1.EventType == AM2.EventType));
		}

		/// \brief Contrasts two AreaMessages for differing attributes.
		/// \returns True if any attribute of the left-side operand is different from the respective attribute of the right-side operand.\n
		/// \returns False if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		public static bool operator !=(AreaMessage AM1 /**< Left-side operand */, AreaMessage AM2 /**< Right-side operand */)
		{
			return ((AM1.Origin != AM2.Origin) || (new Decimal(AM1.Radius) != new Decimal(AM2.Radius)) || (AM1.EventType != AM2.EventType));
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