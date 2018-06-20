using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public GameObject _goAudio;
    public Text _txt;
    public int _intPlayerScoring;
    public int _intPlayerDifficulty;
    public int _intSpawnPoint;
    int _intCounter;
    float _ftProbablity;
    float _ftTime;
    float _ftWait;

    void Start()
    {
        _intPlayerScoring = 0;
        _intPlayerDifficulty = 0;
        _intSpawnPoint = 0;
        _intCounter = 0;
        _ftProbablity = 0.0f;
        _ftTime = 0.0f;
        _ftWait = 3.0f;
    }

    void Update()
    {
        if (_txt.text != _intPlayerScoring.ToString())
            _txt.text = _intPlayerScoring.ToString();

        ProbablityRandomDistribution();
    }

    public void DifficultyChange(int _intDifficultyLevel)
    {
        _intPlayerDifficulty = _intDifficultyLevel;
    }

    public void ModifyScore(int score)
    {
        _intPlayerScoring += score / (_intPlayerDifficulty +1);
    }

    void ProbablityRandomDistribution() {
        if (_goAudio.GetComponent<AudioSource>().clip != null &&
        _goAudio.GetComponent<AudioSource>().isPlaying == true) {
            if ((_ftTime += 1 * Time.deltaTime) >= _ftWait) {
                float _ftRND = (float)Random.Range(10f, 100f);
                Debug.Log(_ftRND);
                _ftProbablity = 0.25f + Mathf.Pow(_intCounter, 2);

                if (_ftRND < _ftProbablity) {
                    
                    if (_intSpawnPoint == 1) _intSpawnPoint = 0;
                    else                     _intSpawnPoint = 1;

                    _ftProbablity = 0;
                    _intCounter = 0;
                }
                else
                    _intCounter++;

                Debug.Log(_ftProbablity);
                _ftTime = 0f;
            }
        }
    }
}
