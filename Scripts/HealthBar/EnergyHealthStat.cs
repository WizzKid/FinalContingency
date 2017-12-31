using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnergyHealthStat
{
	//EnergyBar refers to script of same name
	[SerializeField]
	private EnergyHealthBar bar;

	[SerializeField]
	private float maxVal;

	[SerializeField]
	private float currentVal;

	//Updated when player shoots
	public float CurrentVal
	{
		get
		{
			return currentVal;
		}

		set
		{
			this.currentVal = Mathf.Clamp(value, 0, MaxVal);
            if (bar != null)
            {
                bar.Value = currentVal;
            }
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

	//Initializes maxVal as energy cap on player start
	public void Initialize()
	{
		this.MaxVal = maxVal;
		this.CurrentVal = currentVal;
	}
}
