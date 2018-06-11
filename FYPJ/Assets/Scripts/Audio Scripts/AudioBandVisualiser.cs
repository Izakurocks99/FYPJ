using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBandVisualiser : MonoBehaviour {

	public GameObject[] _goAudioScales;
	public GameObject _goPrefab;
	public GameObject _goAudio;
	public Material[] _materials;
	public float floatwait = 0.5f;
	public float floatspeed = 1f;
	float floattime = 0f;

	void Update () {
		for (int i = 0; i < _goAudioScales.Length; i++)
			_goAudioScales[i].transform.localScale = new Vector3(20, AudioSampler._ftBandbuffer[i] * 5f + 20, 20);
		
		if (_goAudio.GetComponent<AudioSource>().clip != null &&
			_goAudio.GetComponent<AudioSource>().isPlaying == true)
				InstantiateBeat();
	}

	void InstantiateBeat() {
		if ((floattime += 1 * Time.deltaTime * floatspeed) >= floatwait)
		{
			for (int i = 0; i < _goAudioScales.Length; i++)
			{
				if (AudioSampler._ftMaxbuffer[i] == AudioSampler._ftMaxbuffer.Max())
				{
					GameObject go = Instantiate(_goPrefab, _goAudioScales[i].transform, false);
					go.transform.position += new Vector3(0, 0.5f, 0);
					go.transform.localScale = new Vector3(0.125f, 0.125f, 0.125f);
					go.transform.GetComponent<Renderer>().material = _materials[Random.Range(0, _materials.Length)];
					go.name = "Test " + i;
					go.SetActive(true);
				}
			}
			floattime = 0;
		}
	}
}
