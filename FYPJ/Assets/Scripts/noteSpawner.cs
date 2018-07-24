using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noteSpawner : MonoBehaviour {

	public GameObject prefab;
	public GameObject strongerPrefab;
	public int number;
	float intensity, prevIntensity;
	GameObject _instance;
	public Audio1 audio1;
	bool spawnActive = true;

	// Use this for initialization
	void Start ()
	{
		prevIntensity = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		intensity = audio1.GetComponent<Audio1>()._bandBuffer[number];

		if ((intensity - prevIntensity > audio1.GetComponent<Audio1>().sensitivity /*|| intensity > 0.7f*/) && audio1.GetComponent<Audio1>().spawnLimit < 2  && Time.time>1 && spawnActive && audio1.GetComponent<Audio1>().spawnActive )
		{
			if(intensity - prevIntensity >0.6f)
				_instance = Instantiate(strongerPrefab, this.transform);
			else
				_instance = Instantiate(prefab, this.transform);


			_instance.GetComponent<StanAudioMotion>().id = number;
			audio1.GetComponent<Audio1>().spawnLimit += 1;
			//spawnActive = false;
			//StartCoroutine("SpawnDelay");
		}

		
		prevIntensity = intensity;
	}


}
