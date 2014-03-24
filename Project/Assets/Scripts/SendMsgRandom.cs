using UnityEngine;
using System;
using RouterMessagingSystem;

public class SendMsgRandom : MonoBehaviour
{
	RoutingEvent Event = RoutingEvent.Null;
	System.Random RNG = new System.Random();

	public void Update()
	{
		Event = (RoutingEvent)Enum.Parse(typeof(RoutingEvent), RNG.Next(1, 17).ToString());
		Router.RouteMessage(Event);
	}
}
