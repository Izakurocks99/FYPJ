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

	public GameObject musicSource;

	
	void Start ()
    {
        _audioSource = GetComponent<AudioSource>();
		_audioSource.PlayDelayed(3);
	}



	void LateUpdate()
	{
		
		if (spawnLimit > 0)																//if a beat has spawn this frame
		{
			spawnActive = false;														//desactivate every spawners
			StartCoroutine("SpawnReactivate");											//and Delay its Reactivation
		}

	}

	//reactivation delay for beat spawner after each spawn
	IEnumerator SpawnReactivate()
	{
		spawnLimit = 0;
		yield return new WaitForSeconds(difficulty);									//reactivation delay is set by difficulty  = Delay between 2 beats
		spawnActive = true;

		yield break;
	}


	void Update ()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);

        MakeFrequencyBands();
        CreateAudioBands();
        BandBuffer();
	}



	//create 8 bands from the 512 samples
    void MakeFrequencyBands()
    {
        int count = 0;

        for(int i =0; i<_freqBand.Length; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i + 1);

            for (int j =0; j<sampleCount; j++)
            {
                average += _samples[count] * (count + 1);  
                count++;
            }
            average /= count;

            _freqBand[i] = average * 10;
        }
    }


	//some tweaking to smooth each band
    void CreateAudioBands()
    {
        for (int i = 0; i < _barLenght; i++)
        {
            if (_freqBand[i] > _frequenceHighest[i]) {
                _frequenceHighest[i] = _freqBand[i];									//replace the max intensity if it has been overpowered
            }
            _audioBand[i] = (_freqBand[i] / _frequenceHighest[i]);						//bands will have the same size when they are at their max even if they have different intensity

			_frequenceHighest[i] -= _frequenceHighest[i]/10 * Time.deltaTime;			//band's max will slowly decrease to fit songs that goes down in intensity
        }
    }


	//add buffering to the decrease of band intensity
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

	//difficulty settings
	void SetDifficultyToEasy()
	{
		sensitivity = 0.7f;
		difficulty = .5f;
	}

	void SetDifficultyToNormal()
	{
		 sensitivity = 0.65f;
		 difficulty = 0.3f;
	}

	void SetDifficultyToHard()
	{
		sensitivity = .6f;
		difficulty = .2f;
	}
}
