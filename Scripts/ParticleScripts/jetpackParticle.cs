using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetpackParticle : MonoBehaviour {

	private JetScript jet;
	private ParticleSystem particle;
	private bool particleOn;

	// Use this for initialization
	void Awake () {
		jet = GetComponentInParent<JetScript> ();
		particle = GetComponent<ParticleSystem> ();
		particleOn = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (jet.jetPacking && !particleOn) {
			particleOn = !particleOn;
			particle.Play ();
		} else if (!jet.jetPacking && particleOn) {
			particleOn = !particleOn;
			particle.Stop ();
		}
	}
}
