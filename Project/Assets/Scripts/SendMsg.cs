using UnityEngine;
using RouterMessagingSystem;
using System.Collections.Generic;

public class SendMsg : MonoBehaviour
{
	public MessageTarget MT;
	private Router RTR = RouterBox.GetRouter();

	public void Awake()
	{
		MT = new MessageTarget(this, RoutingEvent.Test1);
	}

	public void Update()
	{
		RTR.RouteMessage(RoutingEvent.Test1);
		RTR.RouteMessageDescendants(MT);
		RTR.RouteMessageAscendants(MT);
	}
}
