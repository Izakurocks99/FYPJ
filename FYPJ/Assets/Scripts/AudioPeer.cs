using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour {

    AudioSource _audiosource;
    public static float[] _samples = new float[64];

    void Start () {
        _audiosource = GetComponent<AudioSource>();
    }
	
	void Update () {
        _audiosource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }
}
