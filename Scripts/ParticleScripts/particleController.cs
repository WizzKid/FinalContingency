using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleController : MonoBehaviour {

	private ParticleSystem part;

	// Use this for initialization
	void Awake () {
		part = GetComponent<ParticleSystem> ();
		part.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!part.isEmitting) {
			Destroy (gameObject);
		}
	}
}
