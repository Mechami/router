using UnityEngine;
using System;
using System.Collections.Generic;

public class StringTest : MonoBehaviour
{
	public string TestString = "";

	public void Awake()
	{
		//List<int> Temp = ConvertToInt("Shazb0t!");
		List<int> Temp = ConvertToInt(TestString);

		foreach(int i in Temp)
		{
			Debug.Log(i);
		}
	}

	public List<int> ConvertToInt(string Input)
	{
		List<int> Results = new List<int>(Input.Length);

		for (int i = 0; i < Input.Length; i++)
		{
			if (char.IsDigit(Input[i]))
			{
				Results.Add(Convert.ToInt32(char.GetNumericValue(Input[i])));
			}
		}

		Results.TrimExcess();

		return Results;
	}
}