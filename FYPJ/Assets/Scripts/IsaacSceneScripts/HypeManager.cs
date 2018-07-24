using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypeManager : MonoBehaviour {

    PlayerStats player;
    AudianceManager audienceManager;
    [SerializeField]
    public int MaxCombo = 8;
    [SerializeField]
    float defaultIncreaceAmount = 0.01f;
    [SerializeField]
    GameObject HypeMeter = null;


    private float playerScore = 0;
    public float scoreHypeRatio = 0.1f;
    //private int hype;
    Material hypeMaterial = null;
    float _hype = 0; // 0 - 1 value 

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerStats>();
        Debug.Assert(HypeMeter);
    }

    void IncreaseHype()
    {
        _hype += defaultIncreaceAmount + (defaultIncreaceAmount * scoreHypeRatio);
        _hype = Mathf.Clamp01(_hype);
    }
    void DecreaseHype()
    {
        _hype -= defaultIncreaceAmount ;
        _hype = Mathf.Clamp01(_hype);
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
            //hype = Mathf.RoundToInt(playerScore * scoreHypeRatio);
            if (_hype >= 0)
            {
                audienceManager.HypeMeter = (int)(_hype * 100);
            }
        }
        if (hypeMaterial == null)
        {
            hypeMaterial = HypeMeter.GetComponent<Renderer>().material;
        }
        hypeMaterial.SetFloat("_ShowPercent",_hype);
	}
}
