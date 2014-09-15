using UnityEngine;
using System;
using RouterMessagingSystem;

public class SendMsgRandom : MonoBehaviour
{
	private RoutingEvent Event = RoutingEvent.Null;
	private Router RTR = RouterBox.GetRouter();
	private System.Random RNG = new System.Random();

	public void Update()
	{
		Event = (RoutingEvent)Enum.Parse(typeof(RoutingEvent), RNG.Next(1, 17).ToString());
		RTR.RouteMessage(Event);
	}
}
