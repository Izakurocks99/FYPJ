using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerDiscEffect : MonoBehaviour {

	public GameObject[] _goDisc;
	public GameObject _goAudio;
	float[] _ftBandBuffer;
	float _ftBandMax;

	void Start () {
		_ftBandBuffer = new float[AudioSampler._ftMaxbuffer.Length];
	}
	
	void Update () {
		if (_goAudio.GetComponent<AudioSource>().clip != null &&
			_goAudio.GetComponent<AudioSource>().isPlaying == true)
		{
			GetTotal();
			DiscScaling();
		}
	}

	void DiscScaling() {
		for (int i = 0; i < _goDisc.Length; i++) {
			if (((_ftBandMax / 45) * 0.25f) < 0.2f)
				_goDisc[i].transform.localScale = new Vector3(1, 1, 1) +
												  new Vector3(1, 1, 1) * ((_ftBandMax / 45) * 0.25f);
		}
	}

	void GetTotal() {
		_ftBandBuffer = AudioSampler._ftMaxbuffer;
		_ftBandMax = 0.0f;

		for (int i = 0; i < _ftBandBuffer.Length; i++)
			_ftBandMax += _ftBandBuffer[i];
	}
}
