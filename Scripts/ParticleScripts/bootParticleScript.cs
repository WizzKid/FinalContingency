using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bootParticleScript : MonoBehaviour {

	private changegravity gravScript;
	private ParticleSystem particle;
	private bool particleOn;

	// Use this for initialization
	void Awake () {
		gravScript = GetComponentInParent<changegravity> ();
		particle = GetComponent<ParticleSystem> ();
		particleOn = false;
	}

	// Update is called once per frame
	void Update () {
		if (gravScript.useBoots && !particleOn) {
			particleOn = !particleOn;
			particle.Play ();
		} else if (!gravScript.useBoots && particleOn) {
			particleOn = !particleOn;
			particle.Stop ();
		}
	}
}
