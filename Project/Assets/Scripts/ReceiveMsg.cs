using UnityEngine;
using RouterMessagingSystem;

public class ReceiveMsg : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	private Router RTR = RouterBox.GetRouter();
	private Route RT = new Route();

	public void Awake()
	{
		RT = new Route(this, Test, Event);
	}

	public void OnEnable()
	{
		RTR.AddRoute(RT);
	}

	public void OnDisable()
	{
		RTR.RemoveRoute(RT);
	}

	public void Test()
	{
		int i = (1 + 1);
		i = ~i;
		Debug.Log(this + ": " + i.ToString());
	}
}
