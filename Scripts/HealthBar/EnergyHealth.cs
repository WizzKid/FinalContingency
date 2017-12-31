using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyHealth : MonoBehaviour
{
	//refers to ColossusHealthStat script
	[SerializeField]
	private EnergyHealthStat energy;

	//calls function in ColossusHealthStat to initialize maxValue as health
	private void Awake()
	{
		energy.Initialize();
	}

	//Pulls heat stat from rayCastShoot
	public void adjHeat (float heatMeter)
	{
		energy.CurrentVal = heatMeter;
	}
}
