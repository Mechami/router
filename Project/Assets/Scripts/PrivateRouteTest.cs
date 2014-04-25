using UnityEngine;
using RouterMessagingSystem;

public class PrivateRouteTest : MonoBehaviour
{
	private Route RT = new Route();

	public void Awake()
	{
		RT = new Route(this, Dummy, RoutingEvent.Test1);
		Router.AddRoute(RT);
	}

	private void Dummy()
	{
		Debug.Log("Private function called from Router!");
	}
}
