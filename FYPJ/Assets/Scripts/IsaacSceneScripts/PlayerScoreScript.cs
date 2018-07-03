using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreScript : MonoBehaviour {

    private PlayerStats player;
    private Text text;
	// Use this for initialization

	void Start () {
        player = FindObjectOfType<PlayerStats>();
        text = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (text.text != player._intPlayerScoring.ToString())
        {
            text.text = player._intPlayerScoring.ToString();
        }
	}
}
