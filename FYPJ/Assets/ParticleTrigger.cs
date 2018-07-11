using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Activate()
	{
		gameObject.GetComponent<ParticleSystem>().Play();
		gameObject.GetComponentsInChildren<ParticleSystem>()[1].Play();
	}
}
