using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSampler : MonoBehaviour {
// Generals
    public GameObject _goPlayer;
    AudioSource _audiosource;
    public AudioClip[] _audioclips;
    public static float[] _ftSamples = new float[512];

// Sampling Seperation
    // public static float[] _ftFreqbands = new float[8];
    // public static float[] _ftBandbuffer = new float[8];
    // public static float[] _ftMaxbuffer = new float[8];
    // float[] _ftBufferDecrease = new float[8];
    private int _intLimit;
    public static float[] _ftMaxbufferParse;

// 8x Sampling
    private float[] _ftFreqbands8x = new float[8];
    private float[] _ftBandbuffer8x = new float[8];
    private float[] _ftBufferDecrease8x = new float[8];

// 4x Sampling
    private float[] _ftFreqbands4x = new float[4];
    private float[] _ftBandbuffer4x = new float[4];
    private float[] _ftBufferDecrease4x = new float[4];

    void Start () {
        _audiosource = GetComponent<AudioSource>();

        if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true)
            _intLimit = 4;
        else
            _intLimit = 8;

        _ftMaxbufferParse = new float[_intLimit];
    }
	
	void Update () {
// Get Samples
        GetSpectrumAudioData();

// Use Samples
        MakeFrequencyBand();
        BandBuffer();
        BandMax();
    }

    void GetSpectrumAudioData() {
        _audiosource.GetSpectrumData(_ftSamples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBand() {
        int _intCnt = 0;

// Get the Sample Count for each Range
        for (int i = 0; i < _intLimit; i++)
        {
            float _ftAvg = 0;
            int _intSampleCnt = (int)Mathf.Pow(2, i) * 2;

// Get Full Range for 8 Band Range
            if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) {
                if (i == 7)
                    _intSampleCnt +=2;
            }

// Get Total Avg for each Range
            for (int j = 0; j < _intSampleCnt; j++) {
                _ftAvg += _ftSamples[_intCnt] * (_intCnt + 1);
                _intCnt++;
            }

// Get Avg for each Range
            _ftAvg /= _intCnt;

// Seperate Avg into each Bands
            if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) {
                _ftFreqbands4x[i] = _ftAvg * 10;
            }
            else
                _ftFreqbands8x[i] = _ftAvg * 10;
        }
    }

    void BandBuffer() {
        float[] _ftFreqbandsParse = new float[_intLimit];
        float[] _ftBandbufferParse = new float[_intLimit];
        float[] _ftBufferDecreaseParse = new float[_intLimit];

        if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) {
            _ftFreqbandsParse = _ftFreqbands4x;
            _ftBandbufferParse = _ftBandbuffer4x;
            _ftBufferDecreaseParse = _ftBufferDecrease4x;
        }
        else {
            _ftFreqbandsParse = _ftFreqbands8x;
            _ftBandbufferParse = _ftBandbuffer8x;
            _ftBufferDecreaseParse = _ftBufferDecrease8x;
        }

        for (int k = 0; k < _intLimit; k++) {
            if (_ftFreqbandsParse[k] > _ftBandbufferParse[k]) {
                _ftBandbufferParse[k] = _ftFreqbandsParse[k];
                _ftBufferDecreaseParse[k] = 0.005f;
            }
            else if (_ftFreqbandsParse[k] < _ftBandbufferParse[k]) {
                _ftBandbufferParse[k] -= _ftBufferDecreaseParse[k];
                _ftBufferDecreaseParse[k] *= 1.2f;
                if (_ftBandbufferParse[k] < 0.04f)
                    _ftBandbufferParse[k] = 0.04f;
            }
        }

        if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) {
            _ftFreqbands4x = _ftFreqbandsParse;
            _ftBandbuffer4x = _ftBandbufferParse;
            _ftBufferDecrease4x = _ftBufferDecreaseParse;
        }
        else {
            _ftFreqbands8x = _ftFreqbandsParse;
            _ftBandbuffer8x = _ftBandbufferParse;
            _ftBufferDecrease8x = _ftBufferDecreaseParse;
        }

        /* for (int k = 0; k < 8; k++)
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
        } */
    }

    void BandMax() {
        float[] _ftBandbufferParse = new float[_intLimit];

        if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) _ftBandbufferParse = _ftBandbuffer4x;
        else                                                     _ftBandbufferParse = _ftBandbuffer8x;

        _ftMaxbufferParse = _ftBandbufferParse;

/*         _ftMaxbuffer[0] = _ftBandbuffer[0];
        _ftMaxbuffer[1] = _ftBandbuffer[1];
        _ftMaxbuffer[2] = _ftBandbuffer[2];
        _ftMaxbuffer[3] = _ftBandbuffer[3];
        _ftMaxbuffer[4] = _ftBandbuffer[4];
        _ftMaxbuffer[5] = _ftBandbuffer[5];
        _ftMaxbuffer[6] = _ftBandbuffer[6];
        _ftMaxbuffer[7] = _ftBandbuffer[7]; */
    }
}
