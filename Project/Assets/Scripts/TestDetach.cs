using UnityEngine;
using RouterMessagingSystem;

public class TestDetach : MonoBehaviour
{
	private Route RT = new Route();
	private Router RTR = RouterBox.GetRouter();

	public void Awake()
	{
		RT = new Route(this, DetachmentTest, RoutingEvent.Test1);
	}

	public void Update()
	{
		Debug.Log(RTR.RouteCount());
		RTR.RemoveRoute(RT);
	}

	public void DetachmentTest()
	{
		
	}
}
