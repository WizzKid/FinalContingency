using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	//refers to HealthStat script
	[SerializeField]
	private HealthStat health;

	//calls function in HealthStat to initialize maxValue as health
	private void Awake()
	{
		health.Initialize();
	}

	//called by the leech/enemy who collides with the player
	public void doDamage (int damage) {

		health.CurrentVal -= damage;

	}
}
