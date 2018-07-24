using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class Audio1 : MonoBehaviour {

    AudioSource _audioSource;
    public static int _barLenght = 8;
    float[] _samples = new float[512];
    float[] _freqBand = new float[_barLenght];
	public float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[_barLenght];
    public float _initialBufferDecrease, _decreaseMultiplier, _higherDecrease;

    float[] _frequenceHighest = new float[_barLenght];
    public static float[] _audioBand = new float[_barLenght];
    public static float[] _audioBandBuffered = new float[_barLenght];

	public float spawnLimit = 0;
	public float sensitivity = 0.65f;
	public float difficulty = 0.3f;
	public bool spawnActive = true;

	
	void Start ()
    {
        _audioSource = GetComponent<AudioSource>();
		Debug.Log(_bandBuffer.Length);
	}

	void LateUpdate()
	{
		
		if (spawnLimit > 0)
		{
			spawnActive = false;
			StartCoroutine("SpawnReactivate");
		}

	}

	IEnumerator SpawnReactivate()
	{
		//Debug.Log("Reactivation");
		spawnLimit = 0;
		yield return new WaitForSeconds(difficulty);
		spawnActive = true;

		yield break;
	}

	void Update ()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
        MakeFrequencyBands();
        
        CreateAudioBands();
        BandBuffer();

		//Debug.Log(spawnLimit);


	}

    void MakeFrequencyBands()
    {
        int count = 0;

        for(int i =0; i<_freqBand.Length; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i + 1);

            for (int j =0; j<sampleCount; j++)
            {
                average += _samples[count] * (count + 1);  // le coef est-il vraiment utile ?
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

			_frequenceHighest[i] -= _frequenceHighest[i]/10 * Time.deltaTime;
        }


    }
}
