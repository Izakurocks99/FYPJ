using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowTarget : MonoBehaviour {

	public Audio1 audio1;
	private Transform destination;
	private float interpolation;
	private Vector3 startPos;



	private void Start()
	{
		startPos = this.transform.position;
		destination = this.transform;
		interpolation = 0;
	}

	private void Update()
	{
		this.transform.position = Vector3.Lerp(startPos, destination.position, interpolation);
		interpolation += Time.deltaTime;

		if (Vector3.Distance(this.transform.position, destination.position)==0)
		{
			destination = GameObject.Find("target " + GetHighestAudioBarID()).transform;
			interpolation = 0;
			startPos = this.transform.position;
		}

	}



	int GetHighestAudioBarID()
	{
		float _highestIntensity = 0;
		int _id = 0;
		for(int i =0; i<audio1._bandBuffer.Length; i++)
		{
			if (audio1._bandBuffer[i] > _highestIntensity)
			{
				_id = i;
				_highestIntensity = audio1._bandBuffer[i];
			}
		}

		return _id;
	}

}
