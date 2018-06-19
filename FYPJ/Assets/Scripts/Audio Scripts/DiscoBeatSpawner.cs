using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoBeatSpawner : MonoBehaviour {

	public GameObject _goPrefab;
	public GameObject _goPlayer;
	public GameObject _goAudio;
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
		GenerateDrag();
	}

	void GenerateDrag() {
		if (_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint == 1) {
			if (_goAudio.GetComponent<AudioSource>().clip != null &&
				_goAudio.GetComponent<AudioSource>().isPlaying == true)
			{
				for (int i = 0; i < _intCount; i++)
				{
					if ((_ftTime += 1 * Time.deltaTime) >= _ftWait)
					{
						Debug.Log(i + " / " + _intCount);
						GameObject go = Instantiate(_goPrefab, this.transform, false);
						go.name = "Test";
						go.SetActive(true);
						_ftTime = 0;
						if (i == _intCount - 1)
						_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint = 0;
					}
				}
			}
		}
	}
}
