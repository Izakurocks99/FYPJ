using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypeManager : MonoBehaviour {

    PlayerStats player;
    AudianceManager audienceManager;

    private float playerScore = 0;
    private float scoreHypeRatio = 0.1f;
    private int hype;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerStats>();

    }
	
	// Update is called once per frame
	void Update () {
        if(audienceManager == null)
        {
            audienceManager = FindObjectOfType<AudianceManager>();
        }
		if (playerScore != player._intPlayerScoring)
        {
            playerScore = player.GetComponent<PlayerStats>()._intPlayerScoring;
            hype = Mathf.RoundToInt(playerScore * scoreHypeRatio);
            if (hype >= 0)
            {
                audienceManager.HypeMeter = hype;
            }
        }
	}
}
