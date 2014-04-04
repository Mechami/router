using UnityEngine;
using RouterMessagingSystem;

public class TestDetach : MonoBehaviour
{
	private Route RT = new Route();

	public void Awake()
	{
		RT = new Route(this, DetachmentTest, RoutingEvent.Test1);
	}

	public void Update()
	{
		Debug.Log(Router.RouteCount());
		Router.RemoveRoute(RT);
	}

	public void DetachmentTest()
	{
		
	}
}
