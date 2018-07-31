using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDelay : MonoBehaviour {

	public GameObject AudioVizualiser;

	public float delay = 1;
	// Use this for initialization
	void Start ()
	{
		gameObject.GetComponent<AudioSource>().clip = AudioVizualiser.GetComponent<AudioSource>().clip;
		GetComponent<AudioSource>().PlayDelayed(delay+3);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
