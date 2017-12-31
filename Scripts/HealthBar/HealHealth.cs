using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealHealth : MonoBehaviour
{
	//refers to ColossusHealthStat script
	[SerializeField]
	private HealHealthStat heal;

	//calls function in ColossusHealthStat to initialize maxValue as health
	private void Awake()
	{
		heal.Initialize();
	}

	//Pulls heat stat from rayCastShoot
	public void adjHeal (float healBeam)
	{
		heal.CurrentVal = healBeam;
	}
}
