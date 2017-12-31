using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ColossusHealthStat
{
	//ColossusBar refers to script of same name
	[SerializeField]
	private ColossusHealthBar bar;

	[SerializeField]
	private float maxVal;

	[SerializeField]
	private float currentVal;

	//Updated when player takes damage
	public float CurrentVal
	{
		get
		{
			return currentVal;
		}

		set
		{
			this.currentVal = Mathf.Clamp(value, 0, MaxVal);
			bar.Value = currentVal;
		}
	}

	public float MaxVal
	{
		get
		{
			return maxVal;
		}

		set
		{
			this.maxVal = value;
			bar.MaxValue = maxVal;
		}
	}

	//Initializes maxVal as health on player start
	public void Initialize()
	{
		this.MaxVal = maxVal;
		this.CurrentVal = currentVal;
	}
}
