using UnityEngine;
using RouterMessagingSystem;
using System.Collections.Generic;

public class SendMsgR0 : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;

	public void Update()
	{
		Router<int>.RouteMessage(Event);
		Router<int>.RouteMessageActiveObjects(Event);
	}
}
