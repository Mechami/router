using UnityEngine;
using System;

namespace RouterMessagingSystem
{
	/** \brief Struct for passing parameters to Router for broadcasting area messages. */
	/** \todo Redocument functions changed to compare area bands. */
	public struct AreaBandMessage : IEquatable<AreaBandMessage>, IComparable<AreaBandMessage>
	{
		/** \brief Vector3 representing the origin coordinate for the area. */
		public readonly Vector3 Origin;
		/** \brief Inner radius of the area in meters. */
		public readonly float InnerRadius;
		/** \brief Outer radius of the area in meters. */
		public readonly float OuterRadius;
		/** \brief Event occuring in the area. */
		public readonly RoutingEvent AreaEvent;

		/** \brief Standard constructor for new AreaBandMessages. */
		public AreaBandMessage(ref Vector3 OriginCoord, ref float InnerAreaRadius, ref float OuterAreaRadius, ref RoutingEvent Event)
		{
			Origin = OriginCoord;
			decimal ID = new Decimal(Mathf.Abs(InnerAreaRadius)), OD = new Decimal(Mathf.Abs(OuterAreaRadius));
			InnerRadius = (ID >= OD)? Mathf.Abs(OuterAreaRadius) : Mathf.Abs(InnerAreaRadius);
			OuterRadius = (OD <= ID)? Mathf.Abs(InnerAreaRadius) : Mathf.Abs(OuterAreaRadius);
			AreaEvent = Event;
		}

		/** \brief Constructor that constructs a new AreaBandMessage from a Vector3 and another AreaBandMessage. */
		public AreaBandMessage(ref Vector3 OriginCoord, ref AreaBandMessage ABM)
		{
			Origin = OriginCoord;
			InnerRadius = ABM.InnerRadius;
			OuterRadius = ABM.OuterRadius;
			AreaEvent = ABM.AreaEvent;
		}

		/** \brief Constructor that constructs a new AreaBandMessage from a float and another AreaBandMessage. */
		public AreaBandMessage(ref float InnerAreaRadius, ref AreaBandMessage ABM)
		{
			Origin = ABM.Origin;
			decimal ID = new Decimal(Mathf.Abs(InnerAreaRadius)), OD = new Decimal(ABM.OuterRadius);
			InnerRadius = (ID >= OD)? ABM.OuterRadius : Mathf.Abs(InnerAreaRadius);
			OuterRadius = (OD <= ID)? Mathf.Abs(InnerAreaRadius) : ABM.OuterRadius;
			AreaEvent = ABM.AreaEvent;
		}

		/** \brief Constructor that constructs a new AreaBandMessage from a float and another AreaBandMessage. */
		public AreaBandMessage(ref AreaBandMessage ABM, ref float OuterAreaRadius)
		{
			Origin = ABM.Origin;
			decimal ID = new Decimal(ABM.InnerRadius), OD = new Decimal(Mathf.Abs(OuterAreaRadius));
			InnerRadius = (ID >= OD)? Mathf.Abs(OuterAreaRadius) : ABM.InnerRadius;
			OuterRadius = (OD <= ID)? ABM.InnerRadius : Mathf.Abs(OuterAreaRadius);
			AreaEvent = ABM.AreaEvent;
		}

		/** \brief Constructor that constructs a new AreaBandMessage from a RoutingEvent and another AreaBandMessage. */
		public AreaBandMessage(ref RoutingEvent Event, ref AreaBandMessage ABM)
		{
			Origin = ABM.Origin;
			InnerRadius = ABM.InnerRadius;
			OuterRadius = ABM.OuterRadius;
			AreaEvent = Event;
		}

		/// \brief Compares the calling AreaBandMessage's attributes to the specified AreaBandMessage's attributes.
		/// \return Returns true if all attributes are the sABMe, otherwise false.
		public bool Equals(AreaBandMessage ABM /**< AreaBandMessage to compare with the calling AreaBandMessage. */)
		{
			return ((this.Origin == ABM.Origin) && (new Decimal(this.InnerRadius) == new Decimal(ABM.InnerRadius)) && (new Decimal(this.OuterRadius) == new Decimal(ABM.OuterRadius)) && (this.AreaEvent == ABM.AreaEvent));
		}

		/// \brief Compares the passed object to the calling AreaBandMessage.
		/// \return Returns true if Obj is an AreaBandMessage and all attributes are equivalent.\n
		/// \return Immediately returns false if Obj is not a AreaBandMessage at all.
		public override bool Equals(System.Object Obj /**< Object to check for equivalency. */)
		{
			return ((Obj is AreaBandMessage) && (this.Origin == ((AreaBandMessage)Obj).Origin) && (new Decimal(this.InnerRadius) == new Decimal(((AreaBandMessage)Obj).InnerRadius)) && (new Decimal(this.OuterRadius) == new Decimal(((AreaBandMessage)Obj).OuterRadius)) && (this.AreaEvent == ((AreaBandMessage)Obj).AreaEvent));
		}

		/// \brief Compares the calling AreaBandMessage's radius to the specified AreaBandMessage's radius.
		/// \return Returns positive if the caller has a larger radius, negative if a smaller radius and zero if equivalent.
		public int CompareTo(AreaBandMessage ABM)
		{
			decimal OurRadius = (new Decimal(this.OuterRadius) - new Decimal(this.InnerRadius)), TheirRadius = (new Decimal(ABM.OuterRadius) - new Decimal(ABM.InnerRadius));
			return ((OurRadius < TheirRadius)? -1 : ((OurRadius > TheirRadius)? 1 : 0));
		}

		/// \brief Returns a hash of this AreaBandMessage.
		/// \returns Hash generated from combined member attribute hashes.
		public override int GetHashCode()
		{
			return (this.Origin.GetHashCode() + this.InnerRadius.GetHashCode() + this.OuterRadius.GetHashCode() + this.AreaEvent.GetHashCode());
		}

		/// \brief Returns a string listing this AreaBandMessage's parameters.
		/// \returns A string containing the origin coordinate, the message radius and the event type.
		public override string ToString()
		{
			return ("[" + Origin + ", " + InnerRadius + ", " + OuterRadius + ", " + AreaEvent + "]");
		}

		/// \brief Compares two AreaBandMessages for equivalent attributes.
		/// \returns True if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.\n
		/// \returns False if any attribute of the left-side operand is different from the respective attribute of the right-side operand.
		public static bool operator ==(AreaBandMessage ABM1 /**< Left-side operand */, AreaBandMessage ABM2 /**< Right-side operand */)
		{
			return ((ABM1.Origin == ABM2.Origin) && (new Decimal(ABM1.InnerRadius) == new Decimal(ABM2.InnerRadius)) && (new Decimal(ABM1.OuterRadius) == new Decimal(ABM2.OuterRadius)) && (ABM1.AreaEvent == ABM2.AreaEvent));
		}

		/// \brief Contrasts two AreaBandMessages for differing attributes.
		/// \returns True if any attribute of the left-side operand is different from the respective attribute of the right-side operand.\n
		/// \returns False if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		public static bool operator !=(AreaBandMessage ABM1 /**< Left-side operand */, AreaBandMessage ABM2 /**< Right-side operand */)
		{
			return ((ABM1.Origin != ABM2.Origin) || (new Decimal(ABM1.InnerRadius) != new Decimal(ABM2.InnerRadius)) || (new Decimal(ABM1.OuterRadius) != new Decimal(ABM2.OuterRadius)) || (ABM1.AreaEvent != ABM2.AreaEvent));
		}

		/// \brief Compares two AreaBandMessages for the smaller radius.
		/// \returns True if the left-side operand is less than the right-side operand, otherwise false.
		public static bool operator <(AreaBandMessage ABM1 /**< Left-side operand */, AreaBandMessage ABM2 /**< Right-side operand */)
		{
			decimal ABM1Radius = (new Decimal(ABM1.OuterRadius) - new Decimal(ABM1.InnerRadius)), ABM2Radius = (new Decimal(ABM2.OuterRadius) - new Decimal(ABM2.InnerRadius));
			return (ABM1Radius < ABM2Radius);
		}

		/// \brief Compares two AreaBandMessages for the larger radius.
		/// \returns True if the left-side operand is greater than the right-side operand, otherwise false.
		public static bool operator >(AreaBandMessage ABM1 /**< Left-side operand */, AreaBandMessage ABM2 /**< Right-side operand */)
		{
			decimal ABM1Radius = (new Decimal(ABM1.OuterRadius) - new Decimal(ABM1.InnerRadius)), ABM2Radius = (new Decimal(ABM2.OuterRadius) - new Decimal(ABM2.InnerRadius));
			return (ABM1Radius > ABM2Radius);
		}

		/// \brief Compares two AreaBandMessages for an equivalent or smaller radius.
		/// \returns True if the left-side operand is less than or equal to the right-side operand, otherwise false.
		public static bool operator <=(AreaBandMessage ABM1 /**< Left-side operand */, AreaBandMessage ABM2 /**< Right-side operand */)
		{
			decimal ABM1Radius = (new Decimal(ABM1.OuterRadius) - new Decimal(ABM1.InnerRadius)), ABM2Radius = (new Decimal(ABM2.OuterRadius) - new Decimal(ABM2.InnerRadius));
			return (ABM1Radius <= ABM2Radius);
		}

		/// \brief Compares two AreaBandMessages for an equivalent or larger radius.
		/// \returns True if the left-side operand is greater than or equal to the right-side operand, otherwise false.
		public static bool operator >=(AreaBandMessage ABM1 /**< Left-side operand */, AreaBandMessage ABM2 /**< Right-side operand */)
		{
			decimal ABM1Radius = (new Decimal(ABM1.OuterRadius) - new Decimal(ABM1.InnerRadius)), ABM2Radius = (new Decimal(ABM2.OuterRadius) - new Decimal(ABM2.InnerRadius));
			return (ABM1Radius >= ABM2Radius);
		}
	}
}