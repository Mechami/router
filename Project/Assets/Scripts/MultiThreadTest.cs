using UnityEngine;
using RouterMessagingSystem;
using System.Threading;

public class MultiThreadTest : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Null;
	private Thread Thread1 = null;
	private bool Playing = false;

	public void Awake()
	{
		Debug.Log("Starting new thread!");
		Playing = Application.isPlaying;
		Thread1 = new Thread(new ThreadStart(ThreadTest));
		Thread1.Start();
	}

	public void OnDisable()
	{
		Playing = false;
	}

	public void ThreadTest()
	{
		while (Playing)
		{
			Router.RouteMessage(Event);
		}
	}
}
