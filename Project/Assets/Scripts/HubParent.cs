using UnityEngine;
using RouterMessagingSystem;

public class HubParent : MonoBehaviour
{
	private Hub SelfHB = null;

	public void Awake()
	{
		SelfHB = Hub.GetOrAddHub(this);
	}

	public void Update()
	{
		SelfHB.BroadcastDownwards(RoutingEvent.Test1);
	}
}
