using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerBeatSpawner : MonoBehaviour {

	public GameObject[] _goContainer;
	public GameObject _goPrefab;
	public Material[] _material;

	void Update () {
		this.transform.parent.transform.LookAt(Camera.main.transform);
	}

	public void SpawnSpeakerBeat() {
		int _intRandomContainer = Random.Range(0, _goContainer.Length);
		float _ftRandomMaterial = Random.value;

		GameObject go = Instantiate(_goPrefab, _goContainer[_intRandomContainer].transform.parent.transform.parent.transform, false);
		go.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		
		if (_ftRandomMaterial <= 0.6f)
			go.GetComponent<Renderer>().material = _material[1];
		else
			go.GetComponent<Renderer>().material = _material[0];
		
		go.name = "Test";
		go.SetActive(true);
	}
}
