using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    public Text _txt;

    public int _intPlayerScoring;
    public int _intPlayerDifficulty;

	void Start () {
        _intPlayerScoring = 0;
        _intPlayerDifficulty = 0;

    }
	
	void Update () {
        if (_txt.text != _intPlayerScoring.ToString())
            _txt.text = _intPlayerScoring.ToString();
	}

    public void DifficultyChange(int _intDifficultyLevel)
    {
        _intPlayerDifficulty = _intDifficultyLevel;
    }
}
