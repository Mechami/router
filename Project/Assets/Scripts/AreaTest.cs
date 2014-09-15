using UnityEngine;
using RouterMessagingSystem;

[RequireComponent(typeof(SphereCollider))]
public class AreaTest : MonoBehaviour
{
	public RoutingEvent EventType = RoutingEvent.Null;
	public float Radius = 0.5f;
	public bool RayCheck = false;
	private AreaMessage AM;
	private SphereCollider SC = null;
	private Router RTR = RouterBox.GetRouter();

	public void Awake()
	{
		SC = this.GetComponent<SphereCollider>();
	}

	public void Update()
	{
		AM = new AreaMessage(this.transform.position, Radius, EventType);
		RTR.RouteMessageArea(AM, RayCheck);
		SC.radius = Radius;
	}
}