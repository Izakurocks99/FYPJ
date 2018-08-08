using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDelay : MonoBehaviour {

	public GameObject AudioVizualiser;
	public float _ftDelay;

	void Start ()
	{
		_ftDelay = 1.0f;

		gameObject.GetComponent<AudioSource>().clip = AudioVizualiser.GetComponent<AudioSource>().clip;
		GetComponent<AudioSource>().PlayDelayed(_ftDelay + 3);
	}

	void Update () {
		Debug.Log(_ftDelay);
	}
}
