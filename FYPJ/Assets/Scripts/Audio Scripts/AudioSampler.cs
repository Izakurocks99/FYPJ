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
    private int _intLimit;
    public static float[] _ftMaxBufferParse = new float[8];

// 8x Sampling
    private float[] _ftFreqbands8x = new float[8];
    private float[] _ftBandbuffer8x = new float[8];
    private float[] _ftBufferDecrease8x = new float[8];

// 4x Sampling
    private float[] _ftFreqbands4x = new float[4];
    private float[] _ftBandbuffer4x = new float[4];
    private float[] _ftBufferDecrease4x = new float[4];

    public float[] _ftDisplay = new float[8];

    void Start () {
        _audiosource = GetComponent<AudioSource>();
    }
	
	void Update () {
        _intLimit = (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) ? 4 : 8;
        if (_intLimit != 8)
            _ftMaxBufferParse = new float[_intLimit];

// Get Samples
        GetSpectrumAudioData();

// Use Samples
        MakeFrequencyBands();
        BandBuffers();
        BandMax();
    }

    void GetSpectrumAudioData() {
        _audiosource.GetSpectrumData(_ftSamples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands() {
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

    void BandBuffers() {
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
                _ftBufferDecreaseParse[k] = (0.005f / 100f);
            }
            else if (_ftFreqbandsParse[k] < _ftBandbufferParse[k]) {
                _ftBandbufferParse[k] -= _ftBufferDecreaseParse[k];
                _ftBufferDecreaseParse[k] *= 1.2f;
                if (_ftBandbufferParse[k] < (0.01f / 100f))
                    _ftBandbufferParse[k] = (0.01f / 100f);
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
        _ftDisplay = _ftBandbufferParse;
    }

    void BandMax() {
        float[] _ftBandbufferParse = new float[_intLimit];

        if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) _ftBandbufferParse = _ftBandbuffer4x;
        else                                                     _ftBandbufferParse = _ftBandbuffer8x;

        _ftMaxBufferParse = _ftBandbufferParse;
        // _ftDisplay = _ftMaxBufferParse;
    }
}
