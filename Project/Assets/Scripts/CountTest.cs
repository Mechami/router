using UnityEngine;
using RouterMessagingSystem;

public class CountTest : MonoBehaviour
{
	public void Update()
	{
		Debug.Log(Router.RouteCount());
	}
}
