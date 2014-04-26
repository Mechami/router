using UnityEngine;
using RouterMessagingSystem;

public class HubTest : MonoBehaviour
{
	public CountTest CT = null;

	public void Awake()
	{
		Hub SelfHB = Hub.GetHub(this);
		SelfHB.AddRoute(new Route(this, Dummy, RoutingEvent.Test1));
		SelfHB.AddRoute(new Route(this, Dummy, RoutingEvent.Test1));
		SelfHB.AddRoute(new Route(null, Dummy, RoutingEvent.Test1));
		SelfHB.AddRoute(new Route(CT, CT.Update, RoutingEvent.Test1));
		SelfHB.RemoveRoute(new Route(this, null, RoutingEvent.Test1));
		SelfHB.RemoveRoute(new Route(null, Dummy, RoutingEvent.Test1));
		SelfHB.RemoveRoute(new Route(this, Dummy, RoutingEvent.Null));
		SelfHB.RemoveRoute(new Route(this, Dummy, RoutingEvent.Test1));
		SelfHB.RemoveRoute(new Route(this, Dummy, RoutingEvent.Test1));
	}

	public void Dummy()
	{
		
	}
}