using UnityEngine;
using RouterMessagingSystem;

public class PrivateRouteTest : MonoBehaviour
{
	private Router RTR = RouterBox.GetRouter();
	private Route RT = new Route();

	public void Awake()
	{
		RT = new Route(this, Dummy, RoutingEvent.Test1);
		RTR.AddRoute(RT);
	}

	private void Dummy()
	{
		Debug.Log("Private function called from Router!");
	}
}
