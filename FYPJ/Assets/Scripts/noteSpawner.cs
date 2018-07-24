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

		//spawn beat when the audio variation is higher than the sensitivity
		if ((intensity - prevIntensity > audio1.GetComponent<Audio1>().sensitivity /*|| intensity > 0.7f*/) && audio1.GetComponent<Audio1>().spawnLimit < 2  && Time.time>1 && spawnActive && audio1.GetComponent<Audio1>().spawnActive )
		{
			
			if(intensity - prevIntensity >0.6f)
				_instance =  Instantiate(strongerPrefab, this.transform);				 //spawning a red beat if the audio variation is a lot stronger


			else
				_instance = Instantiate(prefab, this.transform);						//spawning a normal beat

			
			_instance.GetComponent<NoteComponent>().number = number;						 //giving it his id so it goes on the right target
			audio1.GetComponent<Audio1>().spawnLimit += 1;                              //adding one to spawn limit so it won't spawwn 3 beats at the same time.
			_instance.transform.parent = this.transform;
		}

		
		prevIntensity = intensity;														//storing the previou intensity for next frame
	}


}
