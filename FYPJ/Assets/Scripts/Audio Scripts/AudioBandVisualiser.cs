#define BEAT_POOL
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AudioBandVisualiser : MonoBehaviour
{
    public Vector3 beatScale;
    public GameObject[] _goAudioScales;
    public GameObject[] _goPrefab;
    public GameObject _goAudio;
    public GameObject _goPlayer;
    GameObject _goInstanceA;
    GameObject _goInstanceB;

#if (BEAT_POOL)
    public Shader DissolveShader = null;
    List<Material> DissolveMaterialPool = null;
    const uint MaterialPoolSize = 12;
    public Texture2D RoughnessBeatTex = null;
    public Texture2D MetallicBeatTex = null;
#endif

    public Material[] _matsPrimary;
    public Material[] _matsSecondary;
    public float _ftTime;
    public float _ftWait = 0.5f;
    public float _ftSpeed = 1f;
    float[] _ftAryPrevBuffer;
    float[] _ftAryDiffBuffer;
    public static int _intBeatCounts;
    //int _intPreviousMaterial,     
    int  _intCurrentMaterial;

    int _intMatColorL, _intMatColorR;

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
        _ftAryPrevBuffer = new float[AudioSampler._ftMaxbuffer.Length];
        _ftAryDiffBuffer = new float[AudioSampler._ftMaxbuffer.Length];
        _ftTime = 0f;
        _intBeatCounts = 0;
        //_intPreviousMaterial = 0;
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
    }

    void Update()
    {
        for (int i = 0; i < _goAudioScales.Length; i++)
            _goAudioScales[i].transform.localScale = new Vector3(1, AudioSampler._ftBandbuffer[i] * 0.5f + 1, 1);

        switch (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty) {
            case 0: {
                    _ftWait = 1.25f;
                    break; }
            case 1: {
                    _ftWait = 0.975f;
                    break; }
            case 2: {
                    _ftWait = 0.85f;
                    break; }
            case 3: {
                    _ftWait = 0.65f;
                    break; }
            default: {
                    _ftWait = 1.25f;
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
            if ((_ftTime += 1 * Time.deltaTime * _ftSpeed) >= _ftWait)
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
                        for (int _intCurrentBuffer = 0; _intCurrentBuffer < AudioSampler._ftMaxbuffer.Length; _intCurrentBuffer++) {
                            if ((AudioSampler._ftMaxbuffer[_intCurrentBuffer] - _ftAryPrevBuffer[_intCurrentBuffer]) == _ftAryDiffBuffer[_intCurrentCounter]) {
                                if (_intCurrentCounter == 0) _intFirst = _intCurrentBuffer;
                                else if (_intCurrentCounter != 0) {
                                    if (_intCurrentBuffer == _intFirst) continue;
                                    else if (_intCurrentBuffer != _intFirst) _intSecond = _intCurrentBuffer;
                                }
                            }
                        }
                    }

                    // Start of Spawning Section
                    for (int i = 0; i < _intMax; i++) {

                        // Parse in the Spawn Points
                        if (i == 0) _intParse = _intFirst;
                        else if (i != 0) _intParse = _intSecond;

                        Debug.Log(_intCurrentMaterial);

                        int _intGameMode = _goPlayer.GetComponent<PlayerStats>()._intPlayerMode;

                        switch (_intGameMode) {

                            // Set Material if the Gamemode is Type 1.
                            case 0: {
                                if ((_intMatColorL == 0) && (_intMatColorR == 0)) {
                                    _intCurrentMaterial = Random.Range(0, _matsPrimary.Length);
                                    _intMatColorL = _intCurrentMaterial;
                                    _intCurrentMaterial = Random.Range(0, _matsSecondary.Length) + 2;
                                    _intMatColorR = _intCurrentMaterial;
                                }
                                else {
                                    if (i == 0) _intCurrentMaterial = _intMatColorL;
                                    else if (i != 0) _intCurrentMaterial = _intMatColorR;
                                }
                                break;
                            }
    
                            // Set Material if the Gamemode is Type 2.
                            case 1: {
                                if (i == 0) _intCurrentMaterial = Random.Range(0, _matsPrimary.Length);
                                else if (i != 0) _intCurrentMaterial = Random.Range(0, _matsSecondary.Length) + 2;
                                break;
                            }
    
                            // Set Material if the Gamemode is Invalid.
                            default: {
                                break;
                            }
                        }
#if (BEAT_POOL)
                        GameObject go = GetObjectFromPool(_intCurrentMaterial, _goAudioScales[_intParse]);
#else
                        GameObject go = Instantiate(_goPrefab[_intCurrentMaterial], _goAudioScales[i].transform.parent.transform, false);
                        go.transform.GetComponent<Renderer>().material = _materials[_intCurrentMaterial];
#endif
                        // Attach Readable Object Here!
                        if (i == 0) _goInstanceA = go;
                        if (i != 0) _goInstanceB = go;

                        if (_goInstanceA != null) Debug.Log(_goInstanceA.GetComponent<AudioMotion>().endPoint.position);
                        if (_goInstanceB != null) Debug.Log(_goInstanceB.GetComponent<AudioMotion>().endPoint.position);
                        
                        go.transform.localScale = beatScale;
                        //_intPreviousMaterial = _intCurrentMaterial;

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
                    for (int j = _goAudioScales.Length - 1; j >= 0; j--)
                    {
                        if (AudioSampler._ftMaxbuffer[j] == AudioSampler._ftMaxbuffer.Max())
                        {
                            _intCurrentMaterial = Random.Range(0, _matsPrimary.Length);
                            _intCurrentMaterial = (_intCurrentMaterial == 1) ? 2 : 0;
#if (BEAT_POOL)
                            GameObject go = GetObjectFromPool(_intCurrentMaterial, _goAudioScales[j]);
#else
                            GameObject go = Instantiate(_goPrefab[_intCurrentMaterial], _goAudioScales[j].transform.parent.transform, false);
#endif
                            go.transform.localScale = beatScale;
                            go.name = "Beat " + j;
                            go.SetActive(true);
#if (BEAT_POOL)
                            InitPoolObject(go, _intCurrentMaterial);
#endif
                        }
                    }
                }
                _ftTime = 0;
            }
        }
    }

#if (BEAT_POOL)
    GameObject GetObjectFromPool(int index, GameObject parent)
    {
        GameObject go = _Pool.GetObjectFromPool(index);//listGOBeatPool[index].PopBack();
        go.transform.parent = parent.transform.parent.transform;
        go.transform.position = parent.transform.position;
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
            float _ftDifference = AudioSampler._ftMaxbuffer[i] - _ftAryPrevBuffer[i];
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
