using UnityEngine;
using System;
using RouterMessagingSystem;

public class ReceiveMsgRandom : MonoBehaviour
{
	private Route RT = new Route();
	private RoutingEvent Event = RoutingEvent.Null;
	private System.Random RNG = new System.Random();
	public int Min = 1, Max = 1;

	public void Awake()
	{
		Event = (RoutingEvent)Enum.Parse(typeof(RoutingEvent), RNG.Next(Min, Max).ToString());
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
		Debug.Log(this + " (" + Event.ToString() + ")" + " [" + this.GetInstanceID() + "]");
	}
}
