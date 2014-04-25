using UnityEngine;
using System;

namespace RouterMessagingSystem
{
	/// \brief Struct for passing a target for a message.
	public struct MessageTarget : IEquatable<MessageTarget>
	{
		/// \brief Recipient of the message.
		public readonly Transform Recipient;
		/// \brief Value stating if the recipient is valid.
		public bool IsValid
		{
			get
			{
				return (Recipient != null);
			}
		}

		/** \brief Constructor for new MessageTargets that accepts a GameObject. */
		public MessageTarget(GameObject MessageRecipient) : this()
		{
			this.Recipient = MessageRecipient.transform;
		}

		/** \brief Constructor for new MessageTargets that accepts a Component. */
		public MessageTarget(Component MessageRecipient) : this()
		{
			this.Recipient = MessageRecipient.transform;
		}

		/// \brief Compares the calling MessageTarget's attributes to the specified MessageTarget's attributes.
		/// \return Returns true if all attributes are the same, otherwise false.
		public bool Equals(MessageTarget MT /**< MessageTarget to compare with the calling MessageTarget. */)
		{
			return (this.Recipient == MT.Recipient);
		}

		/// \brief Compares the passed object to the calling MessageTarget.
		/// \return Returns true if Obj is a MessageTarget and recipients are equivalent.\n
		/// \return Immediately returns false if Obj is not a MessageTarget at all.
		public override bool Equals(System.Object Obj /**< Object to check for equivalency. */)
		{
			return ((Obj is MessageTarget) && (this.Recipient == ((MessageTarget)Obj).Recipient));
		}

		/// \brief Returns a hash of this MessageTarget.
		/// \returns Hashcode of Recipient.
		public override int GetHashCode()
		{
			return this.Recipient.GetHashCode();
		}

		/// \brief Returns a string listing this MessageTarget's parameters.
		/// \returns A string containing the recipient.
		public override string ToString()
		{
			return ("[" + ((Recipient != null)? Recipient.ToString() : "Null") + "]");
		}

		/// \brief Compares two MessageTargets for equivalent attributes.
		/// \returns True if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.\n
		/// \returns False if any attribute of the left-side operand is different from the respective attribute of the right-side operand.
		public static bool operator ==(MessageTarget MT1 /**< Left-side operand */, MessageTarget MT2 /**< Right-side operand */)
		{
			return (MT1.Recipient == MT2.Recipient);
		}

		/// \brief Contrasts two MessageTargets for differing attributes.
		/// \returns True if any attribute of the left-side operand is different from the respective attribute of the right-side operand.\n
		/// \returns False if all attributes of the left-side operand are the same as their respective attributes of the right-side operand.
		public static bool operator !=(MessageTarget MT1 /**< Left-side operand */, MessageTarget MT2 /**< Right-side operand */)
		{
			return (MT1.Recipient != MT2.Recipient);
		}
	}
}