using System;

namespace RouterMessagingSystem
{
	/// \brief Interface defining routable objects.
	interface IRoutable<T> : IEquatable<T>
	{
		/// Property representing the current status of a routable object.
		bool IsDead
		{
			get;
		}
	}
}
