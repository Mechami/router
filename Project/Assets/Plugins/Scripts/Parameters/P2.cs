using UnityEngine;
using System;

namespace RouterMessagingSystem
{
	/** \brief Struct that encapsulates two parameters to be passed to message recipients. */
	public struct RouteParameters<T1, T2>
	{
		/// The first parameter for the message.
		public readonly T1 FirstParameter;
		/// The second parameter for the message.
		public readonly T2 SecondParameter;

		/// \brief Constructor that accepts two parameters.
		public RouteParameters(T1 First /**< First parameter. */, T2 Second /**< Second parameter. */) : this()
		{
			FirstParameter = First;
			SecondParameter = Second;
		}

		/// \brief Returns a string listing this struct's parameters.
		/// \returns A string containing the parameters.
		public override string ToString()
		{
			return ("[" + FirstParameter + ", " + SecondParameter + "]");
		}
	}
}