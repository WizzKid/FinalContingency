using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossusHealth : MonoBehaviour
{
	//refers to ColossusHealthStat script
	[SerializeField]
	private ColossusHealthStat health;

	//calls function in ColossusHealthStat to initialize maxValue as health
	private void Awake()
	{
		health.Initialize();
	}

	public void doDamage (int damage) {

		health.CurrentVal -= damage;

	}
}
