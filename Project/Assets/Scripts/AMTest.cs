using UnityEngine;
using System.Collections;
using RouterMessagingSystem;

public class AMTest : MonoBehaviour
{
	private AreaMessage AM = new AreaMessage();

	public void Awake()
	{
		Debug.Log(AM);
	}
}
