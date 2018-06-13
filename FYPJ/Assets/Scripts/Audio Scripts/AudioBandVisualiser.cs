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
	float[] _ftPrevBuffer = new float[AudioSampler._ftMaxbuffer.Length];
	float _ftTime = 0f;

	void Update () {
		for (int i = 0; i < _goAudioScales.Length; i++)
			_goAudioScales[i].transform.localScale = new Vector3(1, AudioSampler._ftBandbuffer[i] * 0.5f + 1, 1);
		
		if (_goAudio.GetComponent<AudioSource>().clip != null &&
			_goAudio.GetComponent<AudioSource>().isPlaying == true)
				InstantiateBeat();
	}

	void InstantiateBeat() {
		switch (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty)
		{
			case 0: {
				_ftWait = 1f;
				break;
			}
			case 1: {
				_ftWait = 0.75f;
				break;
			}
			case 2: {
				_ftWait = 0.345f;
				break;
			}
			case 3: {
				_ftWait = 0.2f;
				break;
			}
			default: {
				_ftWait = 0.345f;
				break;
			}
		}

		if ((_ftTime += 1 * Time.deltaTime * _ftSpeed) >= _ftWait)
		{
			int _intCnt = 0;
			int _intMaterialPrevious = 9;
			for (int i = 0; i < _goAudioScales.Length; i++)
			{
				if (_intCnt < 2)
				{
					if (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty != 1)
					{
						if ((AudioSampler._ftMaxbuffer[i] - _ftPrevBuffer[i]) > 0.4f)
						{
							GameObject go = Instantiate(_goPrefab, _goAudioScales[i].transform.parent.transform, false);
							go.transform.position += new Vector3(0, 0.5f, 0);
							go.transform.localScale = new Vector3(.5f, .5f, .5f);

							int _intMaterialCurrent = Random.Range(0, _materials.Length);
							if (_intMaterialCurrent == _intMaterialPrevious)
							{
								if (_intMaterialPrevious != 0)
									_intMaterialCurrent = _intMaterialPrevious - 1;
								else if (_intMaterialPrevious == 0)
									_intMaterialCurrent = _materials.Length - 1;
							}
							go.transform.GetComponent<Renderer>().material = _materials[_intMaterialCurrent];
							_intMaterialPrevious = _intMaterialCurrent;

							go.name = "Test " + i;
							go.SetActive(true);
							_intCnt++;
							_ftPrevBuffer[i] = AudioSampler._ftMaxbuffer[i];
						}
					}
					if (AudioSampler._ftMaxbuffer[i] == AudioSampler._ftMaxbuffer.Max())
					{
						GameObject go = Instantiate(_goPrefab, _goAudioScales[i].transform.parent.transform, false);
						go.transform.position += new Vector3(0, 0.5f, 0);
						go.transform.localScale = new Vector3(.5f, .5f, .5f);

						int _intMaterialCurrent = Random.Range(0, _materials.Length);
						if (_intMaterialCurrent == _intMaterialPrevious)
						{
							if (_intMaterialPrevious != 0)
								_intMaterialCurrent = _intMaterialPrevious - 1;
							else if (_intMaterialPrevious == 0)
								_intMaterialCurrent = _materials.Length - 1;
						}
						go.transform.GetComponent<Renderer>().material = _materials[_intMaterialCurrent];
						_intMaterialPrevious = _intMaterialCurrent;

						go.name = "Test " + i;
						go.SetActive(true);
						_intCnt++;
						_ftPrevBuffer[i] = AudioSampler._ftMaxbuffer[i];
					}
				}
			}
			
			_ftTime = 0;
		}

	}
}
