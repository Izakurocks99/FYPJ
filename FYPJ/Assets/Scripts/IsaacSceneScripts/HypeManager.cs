using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HypeManager : MonoBehaviour
{

    PlayerStats player;
    AudianceManager audienceManager;
    [HideInInspector]
    public int MaxCombo = 10;
    [SerializeField]
    float defaultIncreaceAmount = 0.01f;
    [SerializeField]
    GameObject HypeMeter = null;

    [SerializeField]
    Sprite[] numberTextures = new Sprite[10];

    [SerializeField]
    float hypeDecaySpeed = 1;

    Material hypeMaterial = null;
    SpriteRenderer hypeNumberMaterial = null;
    float _hype = 0; // 0 - 1 value 

    public int hypeMult = 0; // 1 - max hype value, used by player stats
    // Use this for initialization
    //
    void OnValidate()
    {
        if (numberTextures.Length != 10)
        {
            Debug.LogWarning("DO NOT RESIZE THE NUMBER TEXTURE ARRAY");
            Array.Resize(ref numberTextures, 10);
        }
    }
    void Start()
    {
        switch (PlayerPrefs.GetInt("difficulty"))
        {
            case 0:
                hypeDecaySpeed = 5;
                break;
            case 1:
                hypeDecaySpeed = 10;
                break;
            case 2:
                hypeDecaySpeed = 15;
                break;
        }

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
    public void IncreaseHype(int score)
    {
        _hype += score * defaultIncreaceAmount * hypeMult;
        _hype = Mathf.Clamp01(_hype);
        hypeMult = hypeMult < 9 ? hypeMult + 1 : 9;
        if (_hype >= 0)
            audienceManager.HypeMeter = (int)(_hype * 100);

        setnumbertexture();
    }
    public void DecreaseHype()
    {
        _hype -= defaultIncreaceAmount * 10;
        _hype = Mathf.Clamp01(_hype);
        //hypeMult = hypeMult > 0 ? hypeMult - 1 : 0;
        hypeMult = hypeMult > 0 ? 0 : 0;

        if (_hype >= 0)
            audienceManager.HypeMeter = (int)(_hype * 100);

        setnumbertexture();
    }

    // Update is called once per frame
    void Update()
    {
        if (audienceManager == null)
        {
            audienceManager = FindObjectOfType<AudianceManager>();
        }
        else
        {
            if (_hype > 0)
            {
                _hype -= defaultIncreaceAmount * Time.deltaTime * hypeDecaySpeed;
                audienceManager.HypeMeter = (int)(_hype * 100);
            }
        }
        hypeMaterial.SetFloat("_ShowPercent", _hype);
    }
}