using UnityEngine;
using RouterMessagingSystem;

public class SendMsg : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;

	void Update()
	{
		Router.RouteMessage(Event);
	}
}
