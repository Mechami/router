using UnityEngine;
using RouterMessagingSystem;

public class ReceiveMsg : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	private Route<Component> RT = new Route<Component>();

	public void Awake()
	{
		RT = new Route<Component>(this, Test, Event);
	}

	public void OnEnable()
	{
		Router<Component>.AddRoute(RT);
	}

	public void OnDisable()
	{
		Router<Component>.RemoveRoute(RT);
	}

	public Component Test()
	{
		return this;
	}
}
