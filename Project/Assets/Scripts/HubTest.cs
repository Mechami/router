using UnityEngine;
using RouterMessagingSystem;

public class HubTest : MonoBehaviour
{
	public CountTest CT = null;
	private Hub SelfHB = null;

	public void Awake()
	{
		SelfHB = Hub.GetOrAddHub(this);
		SelfHB.AddRoute(new Route(this, Dummy, RoutingEvent.Test1));
		SelfHB.AddRoute(new Route(this, Dummy, RoutingEvent.Test2));
		SelfHB.AddRoute(new Route(this, Dummy, RoutingEvent.Test1));
		SelfHB.AddRoute(new Route(null, Dummy, RoutingEvent.Test1));
		SelfHB.AddRoute(new Route(CT, CT.Update, RoutingEvent.Test1));
		SelfHB.AddRoute(new Route(this, CT.Update, RoutingEvent.Test1));
		SelfHB.RemoveRoute(new Route(this, null, RoutingEvent.Test1));
		SelfHB.RemoveRoute(new Route(null, Dummy, RoutingEvent.Test1));
		SelfHB.RemoveRoute(new Route(this, Dummy, RoutingEvent.Null));
		SelfHB.RemoveRoute(new Route(this, Dummy, RoutingEvent.Test2));
		SelfHB.RemoveRoute(new Route(this, Dummy, RoutingEvent.Test2));
		SelfHB.Broadcast(RoutingEvent.Test1);
	}

	public void Dummy()
	{
		Debug.Log("Dummy called from " + SelfHB + "!");
	}
}