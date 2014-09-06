using UnityEngine;
using RouterMessagingSystem;

public class CheckTest : MonoBehaviour
{
	private MessageTarget MT;

	public void Awake()
	{
		Router.AddRoute(new Route(this, Temp, RoutingEvent.Test1));
		MT = new MessageTarget(this, RoutingEvent.Test1);
	}

	public void Update()
	{
		Router.RouteMessageAscendants(MT);
	}

	public void Temp()
	{
	}
}
