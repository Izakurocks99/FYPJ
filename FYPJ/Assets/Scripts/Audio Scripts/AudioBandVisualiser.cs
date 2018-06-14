using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBandVisualiser : MonoBehaviour {

	public GameObject[] _goAudioScales;
	public GameObject _goPrefab;
	public GameObject _goAudio;
	public GameObject _goPlayer;
	public Material[] _materials;
	public float _ftWait = 0.5f;
	public float _ftSpeed = 1f;
	float[] _ftPrevBuffer;
	float _ftTime;
	int _intBeats;
	int _intPreviousMaterial;
	int _intCurrentMaterial;

	void Start() {
		_ftPrevBuffer = new float[AudioSampler._ftMaxbuffer.Length];
		_ftTime = 0f;
		_intBeats = 0;
		_intPreviousMaterial = 0;
		_intCurrentMaterial = 10;
	}

	void Update () {
		for (int i = 0; i < _goAudioScales.Length; i++)
			_goAudioScales[i].transform.localScale = new Vector3(1, AudioSampler._ftBandbuffer[i] * 0.5f + 1, 1);

		switch (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty)
		{
			case 0: {
				_ftWait = 1.5f;
				break;
			}
			case 1: {
				_ftWait = 1.0f;
				break;
			}
			case 2: {
				_ftWait = 0.75f;
				break;
			}
			case 3: {
				_ftWait = 0.35f;
				break;
			}
			default: {
				_ftWait = 1.5f;
				break;
			}
		}
		
		if (_goAudio.GetComponent<AudioSource>().clip != null &&
			_goAudio.GetComponent<AudioSource>().isPlaying == true)
				InstantiateBeat();
	}

	void InstantiateBeat() {
		if ((_ftTime += 1 * Time.deltaTime * _ftSpeed) >= _ftWait) {

			if (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty != 0) {
				for (int i = _goAudioScales.Length - 1; i >= 0; i--) {
					if (_intBeats < 2) {

						if ((AudioSampler._ftMaxbuffer[i] - _ftPrevBuffer[i]) > 0.4f) {

							GameObject go = Instantiate(_goPrefab, _goAudioScales[i].transform.parent.transform, false);
							
							_intCurrentMaterial = Random.Range(0, _materials.Length);
							if (_intCurrentMaterial == _intPreviousMaterial)
							{
								if (_intPreviousMaterial != 0)
									_intCurrentMaterial -= 1;
								else if (_intPreviousMaterial == 0)
									_intCurrentMaterial = _materials.Length - 1;
							}
							go.transform.GetComponent<Renderer>().material = _materials[_intCurrentMaterial];
							_intPreviousMaterial = _intCurrentMaterial;

							go.name = "Beat " + i;
							go.SetActive(true);
							_intBeats++;
						}
					}
				}
			}
			for (int j = _goAudioScales.Length - 1; j >= 0; j--) {
				if (_intBeats < 2) {
				
					if (AudioSampler._ftMaxbuffer[j] == AudioSampler._ftMaxbuffer.Max())
					{
						GameObject go = Instantiate(_goPrefab, _goAudioScales[j].transform.parent.transform, false);

						_intCurrentMaterial = Random.Range(0, _materials.Length);
						if (_intCurrentMaterial == _intPreviousMaterial)
						{
							if (_intPreviousMaterial != 0)
								_intCurrentMaterial -= 1;
							else if (_intPreviousMaterial == 0)
								_intCurrentMaterial = _materials.Length - 1;
						}
						go.transform.GetComponent<Renderer>().material = _materials[_intCurrentMaterial];
						_intPreviousMaterial = _intCurrentMaterial;

						go.name = "Beat " + j;
						go.SetActive(true);
						_intBeats++;
					}
				}
			}

			_intBeats = 0;
			_ftTime = 0;
		}
	}
}
