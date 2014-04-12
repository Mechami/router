using UnityEngine;
using RouterMessagingSystem;

public class SendMsgActive : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	private bool RouteActive = false;

	public void Update()
	{
		if (RouteActive)
		{
			Router.RouteMessageActiveObjects(Event);
			RouteActive = !RouteActive;
		}
		else
		{
			Router.RouteMessageInactiveObjects(Event);
			RouteActive = !RouteActive;
		}
	}
}
