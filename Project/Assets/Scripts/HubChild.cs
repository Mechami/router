using UnityEngine;
using RouterMessagingSystem;

public class HubChild : MonoBehaviour
{
	private Hub SelfHB = null;
	private Route RT = new Route();

	public void Awake()
	{
		RT = new Route(this, Test, RoutingEvent.Test1);
		SelfHB = Hub.GetOrAddHub(this);
		SelfHB.AddRoute(RT);
	}

	public void OnDestroy()
	{
		SelfHB.RemoveRoute(RT);
	}

	public void Test()
	{
		Debug.Log("[" + this + "] Test called from " + SelfHB + "!");
	}
}