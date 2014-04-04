using UnityEngine;
using RouterMessagingSystem;

public class CheckTest : MonoBehaviour
{
	public Component Com = null;

	public void Awake()
	{
		Router.AddRoute(new Route(this, Temp, RoutingEvent.Test1));
	}

	public void Update()
	{
		Router.RouteMessageAscendants((null as Component), RoutingEvent.Test1);
	}

	public void Temp()
	{
	}
}
