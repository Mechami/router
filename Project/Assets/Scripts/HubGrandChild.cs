using UnityEngine;
using RouterMessagingSystem;

public class HubGrandChild : MonoBehaviour
{
	private Hub SelfHB = null;

	public void Awake()
	{
		SelfHB = Hub.GetOrAddHub(this);
	}

	public void Update()
	{
		SelfHB.BroadcastUpwards(RoutingEvent.Test1);
	}
}
