using UnityEngine;
using System.Threading;
using RouterMessagingSystem;

public class ReceiveMsgActive : MonoBehaviour
{
	public RoutingEvent Event = RoutingEvent.Test1;
	private Route RT = new Route();
	private System.Random RNG = new System.Random();
	private Thread StateThread = null;
	private bool Playing = false;
	private ulong TimesCalled = 0ul;

	public void Awake()
	{
		RT = new Route(this, Target, Event);
		Router.AddRoute(RT);
		StateThread = new Thread(new ThreadStart(ChangeState));
		StateThread.Name = (this + " [State Thread]");
		this.gameObject.SetActive(RNG.Next(0, 2) != 1);
		Playing = true;
		StateThread.Start();
	}

	public void OnDestroy()
	{
		Playing = false;
		StateThread.Join();
	}

	public void Target()
	{
		TimesCalled++;
	}

	public void ChangeState()
	{
		while (Playing)
		{
			lock (this.gameObject)
			{
				this.gameObject.SetActive(RNG.Next(0, 2) != 1);
			}
			Thread.Sleep(1000);
		}
	}
}
