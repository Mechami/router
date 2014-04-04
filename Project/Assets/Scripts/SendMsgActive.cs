using UnityEngine;
using RouterMessagingSystem;

public class SendMsgActive : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	public bool RouteActive = false;

	public void Update()
	{
		if (RouteActive)
		{
			Router.RouteMessageActiveObjects(Event);
		}
		else
		{
			Router.RouteMessageInactiveObjects(Event);
		}
	}
}
