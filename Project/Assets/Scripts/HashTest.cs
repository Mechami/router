using UnityEngine;
using RouterMessagingSystem;

public class HashTest : MonoBehaviour
{
	public void Awake()
	{
		Route RT = new Route(null, null, RoutingEvent.Test1);
		Debug.Log(RT.GetHashCode());
		RT = new Route(this, null, RoutingEvent.Test1);
		Debug.Log(RT.GetHashCode());
		RT = new Route(null, Dummy, RoutingEvent.Test1);
		Debug.Log(RT.GetHashCode());
		RT = new Route(this, Dummy, RoutingEvent.Test1);
		Debug.Log(RT.GetHashCode());
	}

	public void Dummy()
	{
		
	}
}