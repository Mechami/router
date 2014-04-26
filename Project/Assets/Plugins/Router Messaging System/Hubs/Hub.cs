using UnityEngine;
using System;
using System.Collections.Generic;

namespace RouterMessagingSystem
{
	/// \brief A router for passing messages to one-self.
	/// \todo Implement cleanup system for dead components.
	[AddComponentMenu("Router Messaging System/Hub")]
	public sealed class Hub : MonoBehaviour
	{
		private Dictionary<RoutingEvent, RoutePointer> PointerTable = null;
		private bool TablesExist = false;

		public void AddRoute(Route NewRoute)
		{
			bool Dead = NewRoute.IsDead, External = Dead? true : (NewRoute.Subscriber.gameObject != this.gameObject);
			TablesExist = ((NewRoute.IsValid && !External && !TablesExist)? ConstructTable() : TablesExist);

			if (External || RouteIsRegistered(NewRoute))
			{
				Debug.LogError("[" + this + "] Cannot register " + (Dead? "dead" : (External? "external" : (NewRoute.IsValid? "duplicate" : "invalid"))) + " route " + NewRoute + ".", this);
			}
			else
			{
				AttachAddress(NewRoute);
			}
		}

		public void RemoveRoute(Route OldRoute)
		{
			bool Dead = OldRoute.IsDead, External = Dead? true : (OldRoute.Subscriber.gameObject != this.gameObject);

			if (TablesExist && OldRoute.IsValid && RouteIsRegistered(OldRoute))
			{
				DetachAddress(OldRoute);
			}
			else
			{
				Debug.LogError("[" + this + "] Cannot remove " + (Dead? "dead" : (External? "external" : (OldRoute.IsValid? "non-existant" : "invalid"))) + " route " + OldRoute + ".", this);
			}

			TablesExist = (TablesExist? DeconstructTable() : TablesExist);
		}

		public void Broadcast(RoutingEvent EventType)
		{
			if (TablesExist && KeyHasAddress(EventType))
			{
				PointerTable[EventType]();
			}
		}

		private bool ConstructTable()
		{
			PointerTable = new Dictionary<RoutingEvent, RoutePointer>();
			return true;
		}

		private bool DeconstructTable()
		{
			PointerTable = ((PointerTable.Count > 0)? PointerTable : null);
			return (PointerTable != null);
		}

		private void AttachAddress(Route RT)
		{
			if (!PointerTable.ContainsKey(RT.RouteEvent))
			{
				PointerTable.Add(RT.RouteEvent, RT.Address);
			}
			else
			{
				PointerTable[RT.RouteEvent] = (PointerTable[RT.RouteEvent] + RT.Address);
			}
		}

		private void DetachAddress(Route RT)
		{
			if (PointerTable.ContainsKey(RT.RouteEvent) && (PointerTable[RT.RouteEvent] != null))
			{
				PointerTable[RT.RouteEvent] = (PointerTable[RT.RouteEvent] - RT.Address);
			}
			else
			{
				PointerTable.Remove(RT.RouteEvent);
			}
		}

		private bool RouteIsRegistered(Route RT)
		{
			bool Exists = false;

			if (KeyHasAddress(RT.RouteEvent))
			{
				Delegate[] RPL = PointerTable[RT.RouteEvent].GetInvocationList();
				Exists = Array.Exists<Delegate>(RPL, x => (x as RoutePointer) == RT.Address);
			}

			return Exists;
		}

		private bool KeyHasAddress(RoutingEvent EventType)
		{
			return (PointerTable.ContainsKey(EventType) && (PointerTable[EventType] != null));
		}

		public static Hub GetHub(Component CP)
		{
			return CP.GetComponent<Hub>();
		}
	}
}