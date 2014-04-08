using UnityEngine;
using System;

namespace RouterMessagingSystem
{
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
			return ("[" + FirstParameter + ", " + SecondParameter + ", " + ThirdParameter + "]");
		}
	}
}