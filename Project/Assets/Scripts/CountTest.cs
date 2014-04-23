using UnityEngine;
using RouterMessagingSystem;

public class CountTest : MonoBehaviour
{
	void Update()
	{
		Debug.Log(Router.RouteCount());
	}
}
