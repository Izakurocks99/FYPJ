using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSampler : MonoBehaviour {

    AudioSource _audiosource;
    public AudioClip[] _audioclips;
    public static float[] _ftSamples = new float[512];
    public static float[] _ftFreqbands = new float[8];
    public static float[] _ftBandbuffer = new float[8];
    public static float[] _ftMaxbuffer = new float[8];
    float[] _ftBufferDecrease = new float[8];

    void Start () {
        _audiosource = GetComponent<AudioSource>();
    }
	
	void Update () {
        GetSpectrumAudioData();
        MakeFrequencyBand();
        BandBuffer();
        BandMax();
    }

    void GetSpectrumAudioData() {
        _audiosource.GetSpectrumData(_ftSamples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBand() {
        int _intCnt = 0;
        for (int i = 0; i < 8; i++)
        {
            float _ftAvg = 0;
            int _intSampleCnt = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
                _intSampleCnt +=2;
            for (int j = 0; j < _intSampleCnt; j++)
            {
                _ftAvg += _ftSamples[_intCnt] * (_intCnt + 1);
                _intCnt++;
            }

            _ftAvg /= _intCnt;
            _ftFreqbands[i] = _ftAvg * 10;
        }
    }

    void BandBuffer() {
        for (int k = 0; k < 8; k++)
        {
            if (_ftFreqbands[k] > _ftBandbuffer[k])
            {
                _ftBandbuffer[k] = _ftFreqbands[k];
                _ftBufferDecrease[k] = 0.005f;
            }
            else if (_ftFreqbands[k] < _ftBandbuffer[k])
            {
                _ftBandbuffer[k] -= _ftBufferDecrease[k];
                _ftBufferDecrease[k] *= 1.2f;
                if (_ftBandbuffer[k] < 0.04f)
                    _ftBandbuffer[k] = 0.04f;
            }
        }
    }

    void BandMax() {
        _ftMaxbuffer[0] = _ftBandbuffer[0];
        _ftMaxbuffer[1] = _ftBandbuffer[1];
        _ftMaxbuffer[2] = _ftBandbuffer[2];
        _ftMaxbuffer[3] = _ftBandbuffer[3];
        _ftMaxbuffer[4] = _ftBandbuffer[4];
        _ftMaxbuffer[5] = _ftBandbuffer[5];
        _ftMaxbuffer[6] = _ftBandbuffer[6];
        _ftMaxbuffer[7] = _ftBandbuffer[7];
    }
}
