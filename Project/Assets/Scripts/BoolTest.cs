using UnityEngine;
using RouterMessagingSystem;

public class BoolTest : MonoBehaviour
{
	void Awake()
	{
		Route RT = new Route();
		TestRT(RT);
		RT = new Route(null, Dummy, RoutingEvent.Test1);
		TestRT(RT);
		RT = new Route(this, null, RoutingEvent.Test1);
		TestRT(RT);
		RT = new Route(this, Dummy, RoutingEvent.Null);
		TestRT(RT);
		RT = new Route(this, Dummy, RoutingEvent.Test1);
		TestRT(RT);
	}

	private void TestRT(Route LRT)
	{
		if (LRT)
		{
			Debug.Log("Valid.");
		}
		else
		{
			Debug.Log("Invalid.");
		}
	}

	public void Dummy()
	{
		
	}
}
