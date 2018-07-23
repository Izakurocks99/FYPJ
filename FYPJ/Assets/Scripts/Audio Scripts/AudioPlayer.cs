using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour {

	public AudioSampler _audioSampler;
	public float _ftDelay;

	void Update () {
		if (this.GetComponent<AudioSource>().clip != _audioSampler.transform.GetComponent<AudioSource>().clip) {
			this.GetComponent<AudioSource>().clip = _audioSampler.transform.GetComponent<AudioSource>().clip;
			this.GetComponent<AudioSource>().PlayDelayed(_ftDelay);
		}
	}
}
