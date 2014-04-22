using UnityEngine;
using RouterMessagingSystem;

public class ReceiveMsg : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	private Route RT = new Route();

	public void Awake()
	{
		RT = new Route(this, Test, Event);
	}

	public void OnEnable()
	{
		Router.AddRoute(RT);
	}

	public void OnDisable()
	{
		Router.RemoveRoute(RT);
	}

	public void Test()
	{
		int i = (1 + 1);
		i = ~i;
		//Debug.Log(i);
	}
}
