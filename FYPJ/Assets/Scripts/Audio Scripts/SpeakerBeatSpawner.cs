using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerBeatSpawner : MonoBehaviour {

	public GameObject[] _goContainer;
	public GameObject[] _goPrefab;

	void Update () {
		this.transform.parent.transform.LookAt(Camera.main.transform);
	}

	public void SpawnSpeakerBeat() {
		int _intRandomContainer = Random.Range(0, _goContainer.Length);
		float _ftRandomPrefab = Random.value;

		if (_ftRandomPrefab < 0.6f) {
			GameObject go = Instantiate(_goPrefab[0], _goContainer[_intRandomContainer].transform.parent.transform.parent.transform, false);
			go.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
			
			go.name = "Test";
			go.SetActive(true);
		}
		
		else {
			GameObject go = Instantiate(_goPrefab[1], _goContainer[_intRandomContainer].transform.parent.transform.parent.transform, false);
			go.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
			
			go.name = "Test";
			go.SetActive(true);
		}
	}
}
