﻿#define BEAT_POOL
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AudioBandVisualiser : MonoBehaviour
{
    public Vector3 beatScale;
    public GameObject[] _goPrefab;
    public GameObject _goAudioScalesPrimary;
    public GameObject _goAudioScalesSecondary;
    public GameObject _goAudioScalesTertiary;
    public GameObject _goAudio;
    public GameObject _goPlayer;
    public GameObject _goBounds;
    GameObject[] _goPrimaryArray;
    GameObject[] _goSecondaryArray;
    GameObject[] _goTertiaryArray;
    GameObject[] _goParseArray;
    GameObject _goInstanceA;
    GameObject _goInstanceB;

#if (BEAT_POOL)
    public Shader DissolveShader = null;
    List<Material> DissolveMaterialPool = null;
    const uint MaterialPoolSize = 12;
    public Texture2D RoughnessBeatTex = null;
    public Texture2D MetallicBeatTex = null;
#endif

    public float _ftTime;
    float _ftBPM;
    float[] _ftAryPrevBuffer;
    float[] _ftAryDiffBuffer;
    public static int _intBeatCounts;     
    public static int _intPathing;
    int _intCurrentMaterial;
    int _intMatColorL;
    int _intMatColorR;
    int _intMaxLimit;

#if (BEAT_POOL)  // in external class for now
    //[SerializeField]
    //List<GameObject> listGOPrefab = new List<GameObject>();
    //List<List<GameObject>> listGOBeatPool = null;
    //[Range(50, 500)]
    //uint _maxBeats = 10;
    [SerializeField]
    GameObject _ObjectPool = null;
    ObjectPool _Pool = null;
#endif

    void Start()
    {
        _intMaxLimit = (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) ? 4 : 8;
        // _intMaxLimit = 4;
        _ftAryPrevBuffer = new float[_intMaxLimit];
        _ftAryDiffBuffer = new float[_intMaxLimit];

        _ftTime = 0f;
        _intBeatCounts = 0;
        _intCurrentMaterial = 0;
        _intMatColorL = 0;
        _intMatColorR = 0;

#if (BEAT_POOL)
        Debug.Assert(DissolveShader);
        Debug.Assert(RoughnessBeatTex);
        Debug.Assert(MetallicBeatTex);
        DissolveMaterialPool = new List<Material>();

        for (int i = 0; i < MaterialPoolSize; i++) {
            Material temp = new Material(DissolveShader);
            temp.SetTexture("_RoughnessTex", RoughnessBeatTex);
            temp.SetTexture("_MetallicTex", MetallicBeatTex);
            temp.SetTexture("NoiseTex",
                NoiseTexGenerator.GetTexture((float)i, (float)i));
            DissolveMaterialPool.Add(temp); }

	Debug.Assert(_ObjectPool);
	_Pool = _ObjectPool.GetComponent<ObjectPool>();
	/*
	listGOBeatPool = new List<List<GameObject>>();
        for (int i1 = 0; i1 < listGOPrefab.Count; i1++) {
            listGOBeatPool.Add(new List<GameObject>());

            for (int i2 = 0; i2 < _maxBeats; i2++) {
                listGOBeatPool[i1].Add(Instantiate(listGOPrefab[i1]));
                listGOBeatPool[i1][i2].SetActive(false); }
        }

	 * */
        #endif

        _goPrimaryArray = new GameObject[_goAudioScalesPrimary.transform.childCount];
        for (int _intPrimary = 0; _intPrimary < _goAudioScalesPrimary.transform.childCount; _intPrimary++) {
            _goPrimaryArray[_intPrimary] = _goAudioScalesPrimary.transform.GetChild(_intPrimary).gameObject;
        }

        _goSecondaryArray = new GameObject[_goAudioScalesSecondary.transform.childCount];
        for (int _intSecondary = 0; _intSecondary < _goAudioScalesSecondary.transform.childCount; _intSecondary++) {
            _goSecondaryArray[_intSecondary] = _goAudioScalesSecondary.transform.GetChild(_intSecondary).gameObject;
        }

        _goTertiaryArray = new GameObject[_goAudioScalesTertiary.transform.childCount];
        for (int _intTertiary = 0; _intTertiary < _goAudioScalesTertiary.transform.childCount; _intTertiary++) {
            _goTertiaryArray[_intTertiary] = _goAudioScalesTertiary.transform.GetChild(_intTertiary).gameObject;
        }
    }

#if true
    void Update()
#else 
    void reeee()
#endif
    {
        if (_intPathing == 0)
            _goParseArray = _goPrimaryArray;
        else if (_intPathing == 1)
            _goParseArray = _goSecondaryArray;
        else if (_intPathing == 2)
            _goParseArray = _goTertiaryArray;

        switch (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty) {
            case 0: {
                    _ftBPM = .25f;
                    break; }
            case 1: {
                    _ftBPM = .5f;
                    break; }
            case 2: {
                    _ftBPM = .75f;
                    break; }
            default: {
                    _ftBPM = .5f;
                    break; }
        }

        if (_goAudio.GetComponent<AudioSource>().clip != null &&
            _goAudio.GetComponent<AudioSource>().isPlaying == true &&
           (_goAudio.GetComponent<AudioSource>().time < _goAudio.GetComponent<AudioSource>().clip.length * 0.95f)) {
            GetDifference();
            InstantiateBeat();
        }
    }

    void InstantiateBeat()
    {
        if (_goPlayer.GetComponent<PlayerStats>()._intSpawnPoint == 0)
        {
            if ((_ftTime += 1 * Time.deltaTime) >= _ftBPM)
            {
                float _ftSpawn = Random.value;
                int _intMax = (_ftSpawn < 0.6f) ? 2 : 1;

                // Normal, Hard && Challenge Mode
                if (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty != 0)
                {
                    int _intParse = 9;
                    int _intFirst = 9;
                    int _intSecond = 9;

                    // Getting the Spawn Locations
                    for (int _intCurrentCounter = 0; _intCurrentCounter < _intMax; _intCurrentCounter++) {
                        for (int _intCurrentBuffer = 0; _intCurrentBuffer < AudioSampler._ftMaxBufferParse.Length; _intCurrentBuffer++) {
                            if ((AudioSampler._ftMaxBufferParse[_intCurrentBuffer] - _ftAryPrevBuffer[_intCurrentBuffer]) == _ftAryDiffBuffer[_intCurrentCounter]) {
                                if (_ftAryDiffBuffer[_intCurrentCounter] >= 0.1f) {
                                    if (_intCurrentCounter == 0) _intFirst = _intCurrentBuffer;
                                    else if (_intCurrentCounter != 0) {
                                        if (_intCurrentBuffer != _intFirst) _intSecond = _intCurrentBuffer;
                                        else continue;
                                    }
                                }
                                else continue;
                            }
                        }
                    }

                    // Start of Spawning Section
                    for (int i = 0; i < _intMax; i++) {

                        // Parse in the Spawn Points
                        if (i == 0) _intParse = _intFirst;
                        else if (i != 0) _intParse = _intSecond;

                        int _intGameMode = _goPlayer.GetComponent<PlayerStats>()._intPlayerMode;

                        switch (_intGameMode) {
                            // Set Material if the Gamemode is Type 0.
                            case 0: {
                                if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true) {
                                    switch (_intParse) {
                                        case 0: _intCurrentMaterial = 0; break;
                                        case 1: _intCurrentMaterial = 0; break;
                                        case 2: _intCurrentMaterial = 2; break;
                                        case 3: _intCurrentMaterial = 2; break;
                                        default: break;
                                    }
                                }
                                else {
                                    switch (_intParse) {
                                        case 0: _intCurrentMaterial = 0; break;
                                        case 1: _intCurrentMaterial = 0; break;
                                        case 2: _intCurrentMaterial = 2; break;
                                        case 3: _intCurrentMaterial = 2; break;
                                        case 4: _intCurrentMaterial = 0; break;
                                        case 5: _intCurrentMaterial = 0; break;
                                        case 6: _intCurrentMaterial = 2; break;
                                        case 7: _intCurrentMaterial = 2; break;
                                        default: break;
                                    }
                                }
                                break;
                            }

                            // Set Material if the Gamemode is Type 1
                            case 1: {
                                if (_goPlayer.GetComponent<PlayerStats>()._bl4x == true ) {
                                    switch (_intParse) {
                                        case 0: _intCurrentMaterial = 3; break;
                                        case 1: _intCurrentMaterial = 1; break;
                                        case 2: _intCurrentMaterial = 2; break;
                                        case 3: _intCurrentMaterial = 0; break;
                                        default: break;
                                    }
                                }
                                else {
                                    switch (_intParse) {
                                        case 0: _intCurrentMaterial = 3; break;
                                        case 1: _intCurrentMaterial = 1; break;
                                        case 2: _intCurrentMaterial = 2; break;
                                        case 3: _intCurrentMaterial = 0; break;
                                        case 4: _intCurrentMaterial = 3; break;
                                        case 5: _intCurrentMaterial = 1; break;
                                        case 6: _intCurrentMaterial = 2; break;
                                        case 7: _intCurrentMaterial = 0; break;
                                        default: break;
                                    }
                                }
                                break;
                            }

                            /* // Set Material if the Gamemode is Type 2.
                            case 2: {
                                if ((_intMatColorL == 0) && (_intMatColorR == 0)) {
                                    _intMatColorL = 0;
                                    _intMatColorR = 2;
                                }

                                if (i == 0) _intCurrentMaterial = _intMatColorL;
                                else if (i != 0) _intCurrentMaterial = _intMatColorR;
                                break;
                            }
    
                            // Set Material if the Gamemode is Type 3.
                            case 3: {
                                if (i == 0)      _intCurrentMaterial = Random.Range(0, 2);
                                else if (i != 0) _intCurrentMaterial = Random.Range(2, 4);
                                break;
                            } */

                            // Set Material if the Gamemode is Invalid.
                            default: break;
                        }
#if (BEAT_POOL)
                        if (_intParse >= 9) continue;
                        GameObject go = GetObjectFromPool(_intCurrentMaterial, _goParseArray[_intParse]);
#else
                        GameObject go = Instantiate(_goPrefab[_intCurrentMaterial], _goAudioScales[i].transform.parent.transform, false);
                        go.transform.GetComponent<Renderer>().material = _materials[_intCurrentMaterial];
#endif
                        // Attach Readable Object Here!
                        // if (i == 0) _goInstanceA = go;
                        // if (i != 0) _goInstanceB = go;

                        // if (_goInstanceA != null) Debug.Log(_goInstanceA.GetComponent<AudioMotion>().endPoint.position);
                        // if (_goInstanceB != null) Debug.Log(_goInstanceB.GetComponent<AudioMotion>().endPoint.position);
                        
                        go.transform.localScale = beatScale;

                        go.name = "Beat " + _intParse;
                        go.SetActive(true);
#if (BEAT_POOL)
                        InitPoolObject(go, _intCurrentMaterial);
#endif
                    }
                }

                // Easy Mode && Default Mode
                else if (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty == 0)
                {
                    for (int j = _goParseArray.Length - 1; j >= 0; j--)
                    {
                        if (AudioSampler._ftMaxBufferParse[j] == AudioSampler._ftMaxBufferParse.Max())
                        {
                            _intCurrentMaterial = Random.Range(0, 2);
                            _intCurrentMaterial = (_intCurrentMaterial == 1) ? 2 : 0;
#if (BEAT_POOL)
                            GameObject go = GetObjectFromPool(_intCurrentMaterial, _goParseArray[j]);
#else
                            GameObject go = Instantiate(_goPrefab[_intCurrentMaterial], _goAudioScales[j].transform.parent.transform, false);
#endif
                            go.transform.localScale = beatScale;
                            go.name = "Beat " + j;
                            go.SetActive(true);
#if (BEAT_POOL)
                            InitPoolObject(go, _intCurrentMaterial);
#endif
                            break;    
                        }
                    }
                }
                _ftTime -= 1.0f;
            }
        }
        // Debug.Log(_ftTime);
    }

#if (BEAT_POOL)
    GameObject GetObjectFromPool(int index, GameObject parent)
    {
        //listGOBeatPool[index].PopBack();
        GameObject go = _Pool.GetObjectFromPool(index);
        go.transform.parent = parent.transform;
        go.GetComponent<AudioMotion>()._tfPar = parent.transform;
        go.GetComponent<AudioMotion>().bounds = _goBounds.GetComponent<BoundCalculator>();
        go.transform.position = parent.transform.position;
        go.GetComponent<AudioMotion>().SetPlayer(_goPlayer);
        return go;
    }

    void InitPoolObject(GameObject go, int poolsindex)
    {
        go.SetActive(true);
        //go.transform.position = new Vector3(0, 0, 0);
        go.GetComponent<AudioMotion>().PoolInit(poolsindex, DissolveMaterialPool,_Pool); // responsible for returing the object
        go.GetComponent<AudioBeatCollisionScript>().PoolInit();
    }
#endif

    void GetDifference()
    {
        for (int i = 0; i < _ftAryDiffBuffer.Length; i++)
        {
            // float _ftDifference = AudioSampler._ftMaxbuffer[i] - _ftAryPrevBuffer[i];
            float _ftDifference = AudioSampler._ftMaxBufferParse[i] - _ftAryPrevBuffer[i];
            if (_ftDifference < 0)
                _ftDifference *= -1;
            _ftAryDiffBuffer[i] = _ftDifference;
        }

        _ftAryDiffBuffer = _ftAryDiffBuffer.OrderByDescending(ft => ft).ToArray();
    }

    public void InjectTime(float _ftInjection) {
		 _ftTime -= _ftInjection;
    }
}
