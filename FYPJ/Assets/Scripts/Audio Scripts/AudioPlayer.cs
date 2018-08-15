using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour {

	public AudioSampler _audioSampler;
	public float _ftDelay;

// General
	public GameObject _goPlayer;
	public static float[] _ftDelayedSamples = new float[512];

// Sampling Seperation
    private int _intLimit;
    public static float[] _ftDelayedMaxBufferParse;

// 8x Sampling
    private float[] _ftDelayedFreqbands8x = new float[8];
    private float[] _ftDelayedBandbuffer8x = new float[8];
    private float[] _ftDelayedBufferDecrease8x = new float[8];

// 4x Sampling
    private float[] _ftDelayedFreqbands4x = new float[4];
    private float[] _ftDelayedBandbuffer4x = new float[4];
    private float[] _ftDelayedBufferDecrease4x = new float[4];

    public float[] _ftDisplay = new float[8];

	void Start () {
		if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) _intLimit = 4;
		else _intLimit = 8;

		_ftDelayedMaxBufferParse = new float[_intLimit];
	}

	void Update () {
        if (_audioSampler.transform.GetComponent<AudioSource>().clip != null &&
            _audioSampler.transform.GetComponent<AudioSource>().isPlaying == true) {
            if (GetComponent<AudioSource>().clip == null && GetComponent<AudioSource>().isPlaying != true) {
                this.GetComponent<AudioSource>().clip = _audioSampler.transform.GetComponent<AudioSource>().clip;
                this.GetComponent<AudioSource>().PlayDelayed(_ftDelay);
            }
		}
		if (GetComponent<AudioSource>().isPlaying) {
// Get Samples
			GetSpectrumAudioData();

// Use Samples
			MakeFrequencyBands();
			BandBuffers();
			BandMax();
		}
	}

	void GetSpectrumAudioData () {
		GetComponent<AudioSource>().GetSpectrumData(_ftDelayedSamples, 0, FFTWindow.Blackman);
    }

	void MakeFrequencyBands () {
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
                _ftAvg += _ftDelayedSamples[_intCnt] * (_intCnt + 1);
                _intCnt++;
            }

// Get Avg for each Range
            _ftAvg /= _intCnt;

// Seperate Avg into each Bands
            if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) {
                _ftDelayedFreqbands4x[i] = _ftAvg * 10;
            }
            else
                _ftDelayedFreqbands8x[i] = _ftAvg * 10;
        }
	}

	void BandBuffers () {
        float[] _ftDelayedFreqbandsParse = new float[_intLimit];
        float[] _ftDelayedBandbufferParse = new float[_intLimit];
        float[] _ftDelayedBufferDecreaseParse = new float[_intLimit];

        if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) {
            _ftDelayedFreqbandsParse = _ftDelayedFreqbands4x;
            _ftDelayedBandbufferParse = _ftDelayedBandbuffer4x;
            _ftDelayedBufferDecreaseParse = _ftDelayedBufferDecrease4x;
        }
        else {
            _ftDelayedFreqbandsParse = _ftDelayedFreqbands8x;
            _ftDelayedBandbufferParse = _ftDelayedBandbuffer8x;
            _ftDelayedBufferDecreaseParse = _ftDelayedBufferDecrease8x;
        }

        for (int k = 0; k < _intLimit; k++) {
            if (_ftDelayedFreqbandsParse[k] > _ftDelayedBandbufferParse[k]) {
                _ftDelayedBandbufferParse[k] = _ftDelayedFreqbandsParse[k];
                _ftDelayedBufferDecreaseParse[k] = (0.005f / 100f);
            }
            else if (_ftDelayedFreqbandsParse[k] < _ftDelayedBandbufferParse[k]) {
                _ftDelayedBandbufferParse[k] -= _ftDelayedBufferDecreaseParse[k];
                _ftDelayedBufferDecreaseParse[k] *= 1.2f;
                if (_ftDelayedBandbufferParse[k] < (0.01f / 100f))
                    _ftDelayedBandbufferParse[k] = (0.01f / 100f);
            }
        }

        if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) {
            _ftDelayedFreqbands4x = _ftDelayedFreqbandsParse;
            _ftDelayedBandbuffer4x = _ftDelayedBandbufferParse;
            _ftDelayedBufferDecrease4x = _ftDelayedBufferDecreaseParse;
        }
        else {
            _ftDelayedFreqbands8x = _ftDelayedFreqbandsParse;
            _ftDelayedBandbuffer8x = _ftDelayedBandbufferParse;
            _ftDelayedBufferDecrease8x = _ftDelayedBufferDecreaseParse;
        }
        _ftDisplay = _ftDelayedBandbuffer4x;
	}

	void BandMax () {
		float[] _ftDelayedBandbufferParse = new float[_intLimit];

        if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) _ftDelayedBandbufferParse = _ftDelayedBandbuffer4x;
        else                                                     _ftDelayedBandbufferParse = _ftDelayedBandbuffer8x;

        _ftDelayedMaxBufferParse = _ftDelayedBandbufferParse;
	}
}
