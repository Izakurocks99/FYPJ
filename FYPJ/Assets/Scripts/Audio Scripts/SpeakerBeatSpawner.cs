using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerBeatSpawner : MonoBehaviour {

	public GameObject[] _goContainer;
	public GameObject _goPrefab;
	public Material[] _material;

	public void SpawnSpeakerBeat() {
		int _intRandomContainer = Random.Range(1, 4);
		float _ftRandomMaterial = (float)Random.Range(1f, 4f);

		GameObject go = Instantiate(_goPrefab, _goContainer[_intRandomContainer - 1].transform, false);
		
		if (_ftRandomMaterial <= 2.75f)
			go.GetComponent<Renderer>().material = _material[0];
		else
			go.GetComponent<Renderer>().material = _material[1];
		
		go.name = "Test";
		go.SetActive(true);
	}
}
