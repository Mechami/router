using UnityEngine;
using RouterMessagingSystem;
using System.Collections.Generic;

public class SendMsg : MonoBehaviour
{
	public MessageTarget MT;

	public void Awake()
	{
		MT = new MessageTarget(this, RoutingEvent.Test1);
	}

	public void Update()
	{
		Router.RouteMessage(RoutingEvent.Test1);
		Router.RouteMessageDescendants(MT);
		Router.RouteMessageAscendants(MT);
	}
}
