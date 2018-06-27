using UnityEngine;
using System.Collections;

public class AudioLight : MonoBehaviour {

    public int _band;
    Light _light;

	void Start ()
    {
        _light = GetComponent<Light>();
	}
	

	void Update () {
        _light.intensity = Audio._bandBuffer[_band] * 8;
	}
}
