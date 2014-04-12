using UnityEngine;
using RouterMessagingSystem;
using System.Collections.Generic;

public class SendMsg : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;

	public void Update()
	{
		Router.RouteMessage(Event);
	}
}
