using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public GameObject[] _goSpeaker;
    public GameObject _goAudio;
    public int _intPlayerScoring;
    public int _intPlayerDifficulty;
    public int _intSpawnPoint;
    int _intCounter;
    int _intCombo;
    float _ftProbablity;
    float _ftRNDTime;
    float _ftRNGTime;
    float _ftRNDWait;
    float _ftRNGWait;

    ControllerScript[] _controllers;
    [SerializeField]
    GameObject cam = null;
    WaveEffect Wave = null;
    void Start()
    {
        _intCombo = 0;
        _intPlayerScoring = 0;
        _intPlayerDifficulty = 1;
        _intSpawnPoint = 0;
        _intCounter = 0;
        _ftProbablity = 0.0f;
        _ftRNDTime = 0.0f;
        _ftRNGTime = 0.0f;
        _ftRNDWait = 3.0f;
        _ftRNGWait = 1.0f;


        _controllers = transform.parent.GetComponentsInChildren<ControllerScript>();
        Debug.Assert(cam);
        Wave = cam.GetComponent<WaveEffect>();
    }

    void Update()
    {
        if (_goAudio.GetComponent<AudioSource>().clip != null &&
			_goAudio.GetComponent<AudioSource>().isPlaying == true &&
		   (_goAudio.GetComponent<AudioSource>().time < _goAudio.GetComponent<AudioSource>().clip.length * 0.95f)) {
            ProbablityRandomDistribution();
            //ProbablityRandomGeneration();
        }
    }

    public void DifficultyChange(int _intDifficultyLevel)
    {
        _intPlayerDifficulty = _intDifficultyLevel;
    }
    //int numHitsToPulse = 10;
    public void ModifyScore(int score)
    {
        _intPlayerScoring += score;
    }

    public void ModifyCombo(bool hit)
    {
        if (hit)
        {
            _intCombo++;
            if(_intCombo % 10 == 0)
            {
                Wave.Pulse();
            }
        }
        else
            _intCombo = 0;

        foreach (ControllerScript controller in _controllers) //for each controllers
        {
            if (controller.currStick)
            {
                List<BatonCapsuleFollower> followers = controller.currStick.BatonFollowers;
                foreach (BatonCapsuleFollower follower in followers)//for each baton followers
                {
                    follower.gameObject.GetComponent<Rigidbody>().mass = (_intCombo * 0.5f) + 1;
                }
            }
        }
    }

    void ProbablityRandomDistribution() {
        if ((_ftRNDTime += 1 * Time.deltaTime) >= _ftRNDWait) {
            float _ftRND = (float)Random.Range(50f, 100f);
            _ftProbablity = 0.25f + Mathf.Pow(_intCounter, 2);

            if (_ftRND < _ftProbablity) {
                if (_intSpawnPoint == 1) _intSpawnPoint = 0;
                else                     _intSpawnPoint = 1;

                _ftProbablity = 0;
                _intCounter = 0;
            }
            else
                _intCounter++;

            _ftRNDTime = 0.0f;
        }
    }

    void ProbablityRandomGeneration() {            
        if ((_ftRNGTime += 1 * Time.deltaTime) >= _ftRNGWait) {
            float _ftRNG = (float)Random.Range(80f, 85f);
            float _ftRandom = (float)Random.Range(1f, 100f);

            if (_ftRNG < _ftRandom) {
                if(_goSpeaker.Length > 0)
                    _goSpeaker[Random.Range(0, _goSpeaker.Length)].GetComponent<SpeakerBeatSpawner>().SpawnSpeakerBeat();
            }

            _ftRNGTime = 0.0f;
        }
    }
}
