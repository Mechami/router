using UnityEngine;
using RouterMessagingSystem;

public class ValidityTest : MonoBehaviour
{
	private Route RT1 = new Route(), RT2 = new Route(), RT3 = new Route(), RT4 = new Route();
	private Router RTR = RouterBox.GetRouter();

	public void Awake()
	{
		RT1 = new Route(this, Dummy, RoutingEvent.Null);
		RT2 = new Route(this, Dummy, RoutingEvent.Test1);
		RT3 = new Route(this, Dummy, RoutingEvent.Test1);
		RT4 = new Route(null, null, RoutingEvent.Null);
		RTR.AddRoute(RT1);
		RTR.AddRoute(RT2);
		RTR.AddRoute(RT3);
		RTR.AddRoute(RT4);
		RTR.RemoveRoute(RT1);
		RTR.RemoveRoute(RT2);
		RTR.RemoveRoute(RT3);
		RTR.RemoveRoute(RT4);
	}

	public void Dummy()
	{
		
	}
}
