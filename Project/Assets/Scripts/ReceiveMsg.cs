using UnityEngine;
using RouterMessagingSystem;

public class ReceiveMsg : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	private Route<Component> RT = new Route<Component>(), RT2 = new Route<Component>();

	public void Awake()
	{
		RT = new Route<Component>(this, Test, Event);
		RT2 = new Route<Component>(this, Test2, Event);
	}

	public void OnEnable()
	{
		Router<Component>.AddRoute(RT);
		Router<Component>.AddRoute(RT2);
	}

	public void OnDisable()
	{
		Router<Component>.RemoveRoute(RT);
		Router<Component>.RemoveRoute(RT2);
	}

	public Component Test()
	{
		return this;
	}

	public Component Test2()
	{
		return this;
	}
}
