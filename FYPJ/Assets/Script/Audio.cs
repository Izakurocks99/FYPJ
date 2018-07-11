using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class Audio : MonoBehaviour {


    AudioSource _audioSource;
    public static int _barLenght = 512;
    float[] _samples = new float[512];
    float[] _freqBand = new float[_barLenght];
    public static float[] _bandBuffer = new float[_barLenght];
    float[] _bufferDecrease = new float[512];
    public float _initialBufferDecrease, _decreaseMultiplier;

    float[] _frequenceHighest = new float[_barLenght];
    public static float[] _audioBand = new float[_barLenght];
    public static float[] _audioBandBuffered = new float[_barLenght];

	
	void Start ()
    {
        _audioSource = GetComponent<AudioSource>();
	}
	
	
	void Update ()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
		//MakeFrequencyBands();
		FrequencyBand();


		CreateAudioBands();
        BandBuffer();
    }

	void FrequencyBand()
	{
		for (int i = 0; i <_freqBand.Length; i++)
			_freqBand[i] = _samples[i];
	}

    void MakeFrequencyBands()
    {
        int count = 0;

        for(int i =0; i<_freqBand.Length; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i +1);

            for (int j =0; j<sampleCount; j++)
            {
                average += _samples[count] * (count + 1);  // le coef est-il vraiment utile ?
				Debug.Log(count);
                count++;
            }
            average /= count;

            _freqBand[i] = average * 10;
        }
    }

    void BandBuffer()
    {
        for (int i = 0; i < _barLenght; i++)
        {
            if (_bandBuffer[i] < _audioBand[i])
            {
                _bandBuffer[i] = _audioBand[i];
                _bufferDecrease[i] = _initialBufferDecrease;
            }
            if (_bandBuffer[i] >_audioBand[i])
            {
                _bandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *= _decreaseMultiplier;
            }
        }
    }

    void CreateAudioBands()
    {
        for (int i = 0; i < _barLenght; i++)
        {
            if (_freqBand[i] > _frequenceHighest[i]) {
                _frequenceHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _frequenceHighest[i]);

        }

    }
}
