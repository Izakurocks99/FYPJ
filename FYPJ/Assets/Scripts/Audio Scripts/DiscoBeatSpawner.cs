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
	bool _blFlip;

	void Start() {
		_ftTime = 0.0f;
		_ftWait = 0.125f;
		_ftXIncrement = 0.0f;
		_intCount = 0;
		_intCurrent = 0;
		_blFlip = false;
	}

	void Update () {
		if (_intCount == 0)
			_intCount = Random.Range(11, 35);

			_ftY = Mathf.Lerp(-0.25f, 0.25f, Mathf.PingPong(Time.time, 1));

		if (_goAudio.GetComponent<AudioSource>().clip != null &&
			_goAudio.GetComponent<AudioSource>().isPlaying == true)
			if (_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint == 1)
				GenerateDrag();
	}

	void GenerateDrag() {
		Vector3 _vec3View = Camera.main.transform.position + Camera.main.transform.forward * 0.5f;

		if (_intCount % 2 == 1)
			_ftXIncrement = 0.5f / (_intCount / 2);
		else if (_intCount % 2 == 0)
			_ftXIncrement = 0.5f / (((float)_intCount / 2.0f) + 0.5f);

		if (_blFlip == true)
			if (_ftXIncrement < 0)
				_ftXIncrement = -_ftXIncrement;
		else
			if (_ftXIncrement > 0)
				_ftXIncrement = -_ftXIncrement;

		if ((_ftTime += 1 * Time.deltaTime) >= _ftWait) {
			if (_intCurrent < _intCount) {

				GameObject go = Instantiate(_goPrefab, this.transform, false);
				go.name = "Test " + _intCurrent;
				go.SetActive(true);

				go.transform.position = this.GetComponent<DiscoMotion>()._vec3Spawn;
				float _ftX = _ftXIncrement * _intCurrent;

				if (_blFlip == true)
					go.GetComponent<DiscoBeatMotion>()._vec3Area = new Vector3(0.5f, 0.0f, 0.0f);
				else
					go.GetComponent<DiscoBeatMotion>()._vec3Area = new Vector3(-0.5f, 0.0f, 0.0f);

				go.GetComponent<DiscoBeatMotion>()._vec3Shift = new Vector3(_ftX, _ftY, 0);
				go.GetComponent<DiscoBeatMotion>()._vec3Target = _vec3View;
				
				_intCurrent += 1;
				_goBandVisualiser.GetComponent<AudioBandVisualiser>()._ftTime -= 0.1f;
			}
			else
			{
				_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint = 0;
				_intCurrent = 0;
				float _ftFlip = Random.value;
				if (_ftFlip < 0.5f)
					_blFlip = !_blFlip;
				else
					return;
			}

			_ftTime = 0.0f;
		}
	}
}
