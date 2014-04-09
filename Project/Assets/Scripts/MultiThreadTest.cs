using UnityEngine;
using RouterMessagingSystem;
using System.Collections;
using System.Threading;

public class MultiThreadTest : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Null;
	private Thread Thread1 = null;
	private Thread Thread2 = null;
	private bool Playing = false;

	public void Awake()
	{
		Debug.Log("Starting new thread!");
		Playing = Application.isPlaying;
		Thread1 = new Thread(new ThreadStart(ThreadTest));
		Thread2 = new Thread(new ThreadStart(ThreadTest));
		Thread1.Name = "Thread 1";
		Thread2.Name = "Thread 2";
		Thread1.Start();
		Thread2.Start();
	}

	public void OnDisable()
	{
		Playing = false;
	}

	public void ThreadTest()
	{
		while (Playing)
		{
			Debug.Log("Routing from " + Thread.CurrentThread.Name);
			Router.RouteMessage(Event);
			Thread.Sleep(1000);
		}
	}
}
