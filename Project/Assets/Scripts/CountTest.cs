using UnityEngine;
using RouterMessagingSystem;

public class CountTest : MonoBehaviour
{
	private Router RTR = RouterBox.GetRouter();

	public void Update()
	{
		Debug.Log(RTR.RouteCount());
	}
}
