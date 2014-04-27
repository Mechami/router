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

		/// \brief Registers a new route with the hub.
		/// Prints an error if the specified route cannot be registered.
		public void AddRoute(Route NewRoute)
		{
			// This check is pretty long; Consider breaking it up.
			bool Dead = NewRoute.IsDead, External = Dead? true : ((NewRoute.Subscriber.gameObject != this.gameObject) || (NewRoute.Address != null)? ((NewRoute.Address.Target as Component).gameObject != this.gameObject) : true);
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

		/// \brief Removes a route from the hub.
		/// Prints an error if the specified route is cannot be removed.
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

		/// \brief Broadcasts a message to all registered components on this GameObject.
		public void Broadcast(RoutingEvent EventType)
		{
			if (TablesExist && KeyHasAddress(EventType))
			{
				PointerTable[EventType]();
			}
		}

		/// \brief Broadcasts a message to this and all children of this GameObject.
		/// \note This function is recursive.
		public void BroadcastDownwards(RoutingEvent EventType)
		{
			if (TablesExist && KeyHasAddress(EventType))
			{
				PointerTable[EventType]();
			}

			for (int i = 0; i < this.transform.childCount; i++)
			{
				Hub ChildHub = Hub.GetHub(this.transform.GetChild(i));

				if (ChildHub != null)
				{
					ChildHub.BroadcastDownwards(EventType);
				}
			}
		}

		/// \brief Broadcasts a message to this and all parents of this GameObject.
		/// \note This function is recursive.
		public void BroadcastUpwards(RoutingEvent EventType)
		{
			if (TablesExist && KeyHasAddress(EventType))
			{
				PointerTable[EventType]();
			}

			Hub ParentHub = Hub.GetHub(this.transform.parent);
			if (ParentHub != null)
			{
				ParentHub.BroadcastUpwards(EventType);
			}
		}

		/// \brief Returns the hub associated with the component's GameObject.
		/// \return Returns null if null is passed as parameter.
		public static Hub GetHub(Component CP)
		{
			return (CP != null)? CP.GetComponent<Hub>() : null;
		}

		/// \brief Returns the hub associated with the component's GameObject.
		/// \return Attaches and returns a new hub if one isn't attached.\n
		/// \return Returns null if null is passed as parameter.
		public static Hub GetOrAddHub(Component CP)
		{
			return (CP != null)? (CP.GetComponent<Hub>()?? CP.gameObject.AddComponent<Hub>()) : null;
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
	}
}