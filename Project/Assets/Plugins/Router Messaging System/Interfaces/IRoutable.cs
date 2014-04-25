using System;

namespace RouterMessagingSystem
{
	interface IRoutable<T> : IEquatable<T>
	{
		bool IsDead();
	}
}