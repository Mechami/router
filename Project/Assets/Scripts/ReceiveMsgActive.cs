using UnityEngine;
using RouterMessagingSystem;

public class ReceiveMsgActive : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	private Route RT = new Route();

	public void Awake()
	{
		RT = new Route(this, Test, Event);
		Router.AddRoute(RT);
	}

	public void Test()
	{
		Debug.Log(this);
	}
}
