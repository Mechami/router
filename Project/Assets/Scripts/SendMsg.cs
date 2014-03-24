using UnityEngine;
using RouterMessagingSystem;
using System.Collections.Generic;

public class SendMsg : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;

	public void Update()
	{
		List<Component> Components = Router<Component>.RouteMessage(Event);
		Debug.Log(Components);

		foreach (Component Com in Components)
		{
			Debug.Log(Com);
		}
	}
}
