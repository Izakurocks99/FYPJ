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
	float _ftXIncrement;
	float _ftY;
	int _intCount;
	int _intCurrent;

	void Start() {
		_ftTime = 0.0f;
		_ftWait = 0.125f;
		_ftXIncrement = 0.0f;
		_intCount = 0;
		_intCurrent = 0;
	}

	void Update () {
		if (_intCount == 0)
			// _intCount = Random.Range(15, 35);
			_intCount = 11;

		_ftY = Mathf.Lerp(-0.25f, 0.25f, Mathf.PingPong(Time.time, 1));

		if (_goAudio.GetComponent<AudioSource>().clip != null &&
			_goAudio.GetComponent<AudioSource>().isPlaying == true)
			if (_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint == 1)
				GenerateDrag();
	}

	void GenerateDrag() {
		if (_intCount % 2 == 1)
			_ftXIncrement = 0.5f / (_intCount / 2);

		if ((_ftTime += 1 * Time.deltaTime) >= _ftWait) {
			if (_intCurrent < _intCount) {

				GameObject go = Instantiate(_goPrefab, this.transform, false);
				go.name = "Test " + _intCurrent;
				go.SetActive(true);

				go.transform.position = this.GetComponent<DiscoMotion>()._vec3Spawn;
				float _ftX = _ftXIncrement * _intCurrent;
				go.GetComponent<DiscoBeatMotion>()._vec3Shift = new Vector3(_ftX, _ftY, 0);
				
				_intCurrent += 1;
			}
			else
			{
				_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint = 0;
				_intCurrent = 0;
			}

			_ftTime = 0.0f;
		}
	}
}
