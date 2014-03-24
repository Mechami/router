using UnityEngine;
using System;
using RouterMessagingSystem;

public class ReceiveMsgRandom : MonoBehaviour
{
	private Route RT = new Route();
	RoutingEvent Event = RoutingEvent.Null;
	System.Random RNG = new System.Random();

	public void Awake()
	{
		Event = (RoutingEvent)Enum.Parse(typeof(RoutingEvent), RNG.Next(1, 17).ToString());
		RT = new Route(this, Test, Event);
	}

	public void OnEnable()
	{
		Router.AddRoute(RT);
	}

	public void OnDisable()
	{
		Router.RemoveRoute(RT);
	}

	public void Test()
	{
		//Debug.Log(this + " (" + Event.ToString() + ")" + " [" + this.GetInstanceID() + "]");
	}
}
