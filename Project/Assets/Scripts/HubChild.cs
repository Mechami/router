using UnityEngine;
using RouterMessagingSystem;

public class HubChild : MonoBehaviour
{
	private Hub SelfHB = null;

	public void Awake()
	{
		SelfHB = Hub.GetOrAddHub(this);
		SelfHB.AddRoute(new Route(this, Test, RoutingEvent.Test1));
	}

	public void Test()
	{
		Debug.Log("Test called from " + SelfHB + "!");
	}
}