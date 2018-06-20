using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBeatSpawner : MonoBehaviour {

	public GameObject _goPrefab;
	public GameObject _goPlayer;
	public GameObject _goAudio;
	public GameObject _goBandVisualiser;
	float _ftTime;
	float _ftWait;
	int _intCount;

	void Start() {
		_ftTime = 0;
		_ftWait = 1.0f;
		_intCount = 0;
	}

	void Update () {
		if (_intCount == 0)
			_intCount = Random.Range(15, 40);

		if (_goAudio.GetComponent<AudioSource>().clip != null &&
			_goAudio.GetComponent<AudioSource>().isPlaying == true)
			GenerateDrag();
	}

	void GenerateDrag() {
		if (_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint == 1) {
			
			for (int i = 0; i < _intCount; i++)
			{
				if ((_ftTime += 1 * Time.deltaTime) >= _ftWait)
				{
					GameObject go = Instantiate(_goPrefab, this.transform, false);
					go.name = "Test " + i;
					go.SetActive(true);
					_goPlayer.GetComponent<PlayerStats>().listGO.Add(go);
					_ftTime = 0;
					_goBandVisualiser.GetComponent<AudioBandVisualiser>()._ftTime -= 0.1f;
					if (i == _intCount - 1)
					{
						_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint = 0;
						_ftTime -= 2.0f;
					}
				}
			}
		}
	}
}
