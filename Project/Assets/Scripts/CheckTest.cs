using UnityEngine;
using RouterMessagingSystem;

public class CheckTest : MonoBehaviour
{
	private MessageTarget MT;
	private Router RTR = RouterBox.GetRouter();

	public void Awake()
	{
		RTR.AddRoute(new Route(this, Temp, RoutingEvent.Test1));
		MT = new MessageTarget(this, RoutingEvent.Test1);
	}

	public void Update()
	{
		RTR.RouteMessageAscendants(MT);
	}

	public void Temp()
	{
	}
}
