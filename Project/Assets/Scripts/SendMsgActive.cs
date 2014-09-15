using UnityEngine;
using RouterMessagingSystem;

public class SendMsgActive : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	private Router RTR = RouterBox.GetRouter();
	private bool RouteActive = false;

	public void Update()
	{
		if (RouteActive)
		{
			RTR.RouteMessageActiveObjects(Event);
			RouteActive = !RouteActive;
		}
		else
		{
			RTR.RouteMessageInactiveObjects(Event);
			RouteActive = !RouteActive;
		}
	}
}
