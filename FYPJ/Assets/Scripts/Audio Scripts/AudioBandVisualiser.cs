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
	float floattime = 0f;

	void Update () {
		for (int i = 0; i < _goAudioScales.Length; i++)
			_goAudioScales[i].transform.localScale = new Vector3(20, AudioSampler._ftBandbuffer[i] * 5f + 20, 20);
		
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

		if ((floattime += 1 * Time.deltaTime * _ftSpeed) >= _ftWait)
		{
			for (int i = 0; i < _goAudioScales.Length; i++)
			{
				if (AudioSampler._ftMaxbuffer[i] == AudioSampler._ftMaxbuffer.Max())
				{
					GameObject go = Instantiate(_goPrefab, _goAudioScales[i].transform, false);
					go.transform.position += new Vector3(0, 0.5f, 0);
					go.transform.localScale = new Vector3(1f, 1f, 1f);
					go.transform.GetComponent<Renderer>().material = _materials[Random.Range(0, _materials.Length)];
					go.name = "Test " + i;
					go.SetActive(true);
					break;
				}
			}
			floattime = 0;
		}
	}
}
