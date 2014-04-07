using UnityEngine;
using RouterMessagingSystem;

public class RouteEqualTest : MonoBehaviour
{
	private Route RT1 = new Route(), RT2 = new Route();
	private Route<int> RTI = new Route<int>();
	private Route<int, float, double> RTF = new Route<int, float, double>();

	public void Awake()
	{
		RT1 = new Route(this, Dummy, RoutingEvent.Null);
		RT2 = new Route(this, Dummy, RoutingEvent.Null);
		RTI = new Route<int>(this, Many, RoutingEvent.Null);
		RTF = new Route<int, float, double>(this, Few, RoutingEvent.Null);
		CheckRoute(RT1, RT2);
		RT1 = new Route(this, Smarty, RoutingEvent.Null);
		CheckRoute(RT1, RT2);

		CheckObj(RT2, (System.Object)RT1);
		CheckObj(RT2, (System.Object)RTI);
		CheckObj(RT1, (System.Object)RTF);
	}

	public void CheckRoute(Route RT, Route ORT)
	{
		if (RT.Equals(ORT))
		{
			Debug.Log(RT + " and " + ORT + " are same Route.");
		}
		else
		{
			Debug.Log(RT + " and " + ORT + " are different Routes.");
		}
	}

	public void CheckObj(Route RT, System.Object Obj)
	{
		if (RT.Equals(Obj))
		{
			Debug.Log(RT + " and " + Obj + " are same object.");
		}
		else
		{
			Debug.Log(RT + " and " + Obj + " are different objects.");
		}
	}

	public void Dummy()
	{
		
	}

	public void Smarty()
	{
		
	}

	public int Many()
	{
		return 5;
	}

	public int Few(float Input, double Input2)
	{
		return (int)(Input * Input2);
	}
}
