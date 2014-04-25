using UnityEngine;
using RouterMessagingSystem;

public class ReceiveMsgR0 : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	private Route<int> RT = new Route<int>();

	public void Awake()
	{
		RT = new Route<int>(this, Test, Event);
	}

	public void OnEnable()
	{
		Router<int>.AddRoute(RT);
	}

	public void OnDisable()
	{
		Router<int>.RemoveRoute(RT);
	}

	public int Test()
	{
		return ~(1 + 1);
	}
}
