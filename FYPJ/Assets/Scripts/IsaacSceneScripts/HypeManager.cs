using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypeManager : MonoBehaviour {

    PlayerStats player;
    AudianceManager audienceManager;
    [HideInInspector]
    public int MaxCombo = 10;
    [SerializeField]
    float defaultIncreaceAmount = 0.01f;
    [SerializeField]
    GameObject HypeMeter = null;

    [SerializeField]
    //List<Texture2D> numberTextures = new List<Texture2D>();
    Sprite[] numberTextures = new Sprite[10];

    private float playerScore = 0;
    public float scoreHypeRatio = 0.1f;
    //private int hype;
    Material hypeMaterial = null;
    SpriteRenderer hypeNumberMaterial = null;
    float _hype = 0; // 0 - 1 value 

    [HideInInspector]
    public int hypeMult = 0; // 1 - max hype value, used by player stats
    // Use this for initialization
    //
    void OnValidate()
    {
        if(numberTextures.Length != 10)
        {
            Debug.LogWarning("DO NOT RESIZE THE NUMBER TEXTURE ARRAY");
            Array.Resize(ref numberTextures,10);
        }
    }
    void Start () {
        player = FindObjectOfType<PlayerStats>();
        Debug.Assert(HypeMeter); // SET METER TO GAMOBJETC USE IT
        hypeMaterial = HypeMeter.GetComponent<Renderer>().material;
        GameObject child = HypeMeter.transform.GetChild(0).gameObject;
        hypeNumberMaterial = child.GetComponent<SpriteRenderer>();
        setnumbertexture();
    }

    void setnumbertexture()
    {
        hypeNumberMaterial.sprite = numberTextures[hypeMult];
    }
    public void IncreaseHype()
    {
        _hype += defaultIncreaceAmount + (defaultIncreaceAmount * scoreHypeRatio);
        _hype = Mathf.Clamp01(_hype);
        hypeMult = hypeMult < 9 ? hypeMult + 1 : 9;
        setnumbertexture();
    }
    public void DecreaseHype()
    {
        _hype -= defaultIncreaceAmount ;
        _hype = Mathf.Clamp01(_hype);
        hypeMult = hypeMult > 0 ? hypeMult - 1 : 0;
        setnumbertexture();
    }
    
	// Update is called once per frame
	void Update () 
    {
        if(audienceManager == null)
        {
            audienceManager = FindObjectOfType<AudianceManager>();
        }
		if (playerScore != player._intPlayerScoring)
        {
            playerScore = player.GetComponent<PlayerStats>()._intPlayerScoring;
            if (_hype >= 0) {
                audienceManager.HypeMeter = (int)(_hype * 100);
            }
        }
            hypeMaterial.SetFloat("_ShowPercent",_hype);
	}
}
//TODO test in game 
