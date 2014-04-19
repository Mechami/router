using UnityEngine;
using RouterMessagingSystem;

public class ValidityTest : MonoBehaviour
{
	Route RT1 = new Route(), RT2 = new Route(), RT3 = new Route(), RT4 = new Route();

	public void Awake()
	{
		RT1 = new Route(this, Dummy, RoutingEvent.Null);
		RT2 = new Route(this, Dummy, RoutingEvent.Test1);
		RT3 = new Route(this, Dummy, RoutingEvent.Test1);
		RT4 = new Route(null, null, RoutingEvent.Null);
		Router.AddRoute(RT1);
		Router.AddRoute(RT2);
		Router.AddRoute(RT3);
		Router.AddRoute(RT4);
		Router.RemoveRoute(RT1);
		Router.RemoveRoute(RT2);
		Router.RemoveRoute(RT3);
		Router.RemoveRoute(RT4);
	}

	public void Dummy()
	{
		
	}
}
