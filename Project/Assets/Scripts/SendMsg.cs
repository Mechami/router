using UnityEngine;
using RouterMessagingSystem;

public class SendMsg : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;

	public void Update()
	{
		Component[] c = Router<Component>.RouteMessage(Event);

		foreach (Component c2 in c)
		{
			Debug.Log(c2);
		}
	}
}
