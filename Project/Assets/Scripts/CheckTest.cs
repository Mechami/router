using UnityEngine;
using RouterMessagingSystem;

public class CheckTest : MonoBehaviour
{
	private MessageTarget MT = new MessageTarget();

	public void Awake()
	{
		Router.AddRoute(new Route(this, Temp, RoutingEvent.Test1));
	}

	public void Update()
	{
		Router.RouteMessageAscendants(MT, RoutingEvent.Test1);
	}

	public void Temp()
	{
	}
}
