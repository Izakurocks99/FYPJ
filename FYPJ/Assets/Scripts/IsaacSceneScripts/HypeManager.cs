using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypeManager : MonoBehaviour {

    public GameObject player;
    public GameObject audienceManager;

    private float playerScore = 0;
    private float scoreHypeRatio = 0.1f;
    private int hype;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(audienceManager == null)
        {
            audienceManager = FindObjectOfType<AudianceManager>().gameObject;
        }
		if (playerScore != player.GetComponent<PlayerStats>()._intPlayerScoring)
        {
            playerScore = player.GetComponent<PlayerStats>()._intPlayerScoring;
            hype = Mathf.RoundToInt(playerScore * scoreHypeRatio);
            if (hype > 0)
            {
                audienceManager.GetComponent<AudianceManager>().HypeMeter = hype;
            }
        }
	}
}
