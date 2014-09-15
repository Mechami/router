using UnityEngine;
using System.Threading;
using RouterMessagingSystem;

public class ReceiveMsgActive : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	public bool Active = false;
	private Router RTR = RouterBox.GetRouter();
	private Route RT = new Route();
	private ulong TimesCalled = 0ul;

	public void Awake()
	{
		RT = new Route(this, Target, Event);
		RTR.AddRoute(RT);
		this.gameObject.SetActive(Active);
	}

	public void Target()
	{
		TimesCalled++;
		Debug.Log("[" + this + "] Times called: " + TimesCalled);
	}
}
