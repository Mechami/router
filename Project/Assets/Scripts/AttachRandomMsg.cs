using UnityEngine;

public class AttachRandomMsg : MonoBehaviour
{
	private int Count = 0;
	public int Max = 0;

	void LateUpdate()
	{
		this.gameObject.AddComponent<ReceiveMsgRandom>();

		Count++;

		if (Count >= Max)
		{
			this.enabled = false;
			Debug.Log("Done.");
		}
	}
}
