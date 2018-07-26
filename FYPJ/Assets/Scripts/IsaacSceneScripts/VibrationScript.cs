﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class VibrationScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        PS4Input.MoveSetVibration(0, 1, 0);
        PS4Input.MoveSetVibration(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {

    }

    public IEnumerator VibrateLeft()
    {
#if UNITY_PS4
        PS4Input.MoveSetVibration(0, 1, 128);
        yield return new WaitForSeconds(0.1f);
        PS4Input.MoveSetVibration(0, 1, 0);
#else
		yield return new WaitForSeconds(0.1f);

#endif
	}
	public IEnumerator VibrateRight()
    {
#if UNITY_PS4

		PS4Input.MoveSetVibration(0, 0, 128);
        yield return new WaitForSeconds(0.1f);
        PS4Input.MoveSetVibration(0, 0, 0);
#else
		        yield return new WaitForSeconds(0.1f);

#endif
	}
}
