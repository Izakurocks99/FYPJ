using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class InstantiateBeat : MonoBehaviour {

    public GameObject[] _goPrefab;
    public GameObject[] _goContainer;
    public GameObject[] _goBridges;
    GameObject[] _goAryPrefabLeft = new GameObject[6];
    GameObject[] _goAryPrefabRight = new GameObject[6];
    public GameObject _goPlayer;
    public GameObject _goAudio;

    Color[] _colAry = new Color[9];

    public Material[] _materials;

    public float _ftSpacing;
    public float _ftBeatScale;
    public float _ftBeatSpeed;
    public float _ftDistanceCheck;
    float _ftWaitTimer;

    Vector3 _vec3Center;

    void InitializeColor()
    {
        _colAry[0] = new Color32(246, 221, 204, 255);
        _colAry[1] = new Color32(237, 187, 153, 255);
        _colAry[2] = new Color32(229, 152, 102, 255);
        _colAry[3] = new Color32(220, 118, 51,  255);
        _colAry[4] = new Color32(211, 84,  0,   255);
        _colAry[5] = new Color32(186, 74,  0,   255);
        _colAry[6] = new Color32(160, 64,  0,   255);
        _colAry[7] = new Color32(135, 54,  0,   255);
        _colAry[8] = new Color32(110, 44,  0,   255);
    }

    void Initialize()
    {
        _ftWaitTimer = 0.0f;
        if (_ftSpacing == 0)
            _ftSpacing = 3.75f;
        if (_ftDistanceCheck == 0)
            _ftDistanceCheck = 2.0f;

        InitializeColor();

        for (int i = 0; i < 6; i++)
        {
            GameObject go = Instantiate(_goPrefab[0], _goContainer[0].transform, false);

            GameObject _go1 = Instantiate(_goPrefab[0], _goContainer[0].transform, false);
            _go1.name = "Sample Cube Left " + i;
            _go1.transform.eulerAngles = new Vector3(0, -_ftSpacing * (i + 0.5f), 0);
            _go1.transform.position = _go1.transform.forward * 3.0f + new Vector3(0.0f, 0.3f, -7.5f);
            
            // if (i < 3)
            //     _go1.transform.LookAt(_goBridges[0].transform);
            // else
            //     _go1.transform.LookAt(_goBridges[1].transform);
            
            _go1.SetActive(true);
            _goAryPrefabLeft[i] = _go1;

            GameObject _go2 = Instantiate(_goPrefab[0], _goContainer[0].transform, false);
            _go2.name = "Sample Cube Right " + i;
            _go2.transform.eulerAngles = new Vector3(0, _ftSpacing * (i + 0.5f), 0);
            _go2.transform.position = _go2.transform.forward * 3.0f + new Vector3(0.0f, 0.3f, -7.5f);

            // if (i < 3)
            //     _go2.transform.LookAt(_goBridges[2].transform);
            // else
            //     _go2.transform.LookAt(_goBridges[3].transform);

            _go2.SetActive(true);
            _goAryPrefabRight[i] = _go2;
        }
    }

    void Start () {
        Initialize();
    }
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            #if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        if (_goAudio.transform.GetComponent<AudioSource>().clip == null && _goAudio.transform.GetComponent<AudioSource>().isPlaying == false)
              return;
        else
            DelayedSpawnBeats(true);

        for (int i = 0; i < 6; i++)
        {
            if (_goAryPrefabLeft[i] != null)
            {
                _goAryPrefabLeft[i].transform.localScale = new Vector3(_goAryPrefabLeft[i].transform.localScale.x,
                                                                      (AudioPeer._samples[i] * _ftBeatScale + 1),
                                                                       _goAryPrefabLeft[i].transform.localScale.z);
            }
            if (_goAryPrefabRight[i] != null)
            {
                _goAryPrefabRight[i].transform.localScale = new Vector3(_goAryPrefabRight[i].transform.localScale.x,
                                                                       (AudioPeer._samples[i] * _ftBeatScale + 1),
                                                                        _goAryPrefabRight[i].transform.localScale.z);
            }
        }

        TransitBeats();
    }

    void TransitBeats() {
        int _intSpawnpoint = 0;
        List<GameObject> listGO = new List<GameObject>();
        foreach (Transform child in _goContainer[1].transform)
            listGO.Add(child.gameObject);

        for (int i = 0; i < listGO.Count; i++)
        {
            _intSpawnpoint = int.Parse(Regex.Replace(listGO[i].name, "[^0-9]", ""));

            if (listGO[i].name.StartsWith("L"))
            {
                if (_intSpawnpoint < 3)
                {
                    Vector3 _vec3Heading = _goBridges[0].transform.position - listGO[i].transform.position;
                    if (!(_vec3Heading.sqrMagnitude < _ftDistanceCheck * _ftDistanceCheck))
                        listGO[i].transform.position = Vector3.MoveTowards(listGO[i].transform.position, _goBridges[0].transform.position, _ftBeatSpeed * Time.deltaTime);
                    else {
                        if (listGO[i].GetComponent<Renderer>().material.name.Contains(_materials[0].name))
                            _goPlayer.transform.GetComponent<PlayerStats>()._intPlayerScoring += 10;
                        Destroy(listGO[i]); }
                }
                else {
                    Vector3 _vec3Heading = _goBridges[1].transform.position - listGO[i].transform.position;
                    if (!(_vec3Heading.sqrMagnitude < _ftDistanceCheck * _ftDistanceCheck))
                        listGO[i].transform.position = Vector3.MoveTowards(listGO[i].transform.position, _goBridges[1].transform.position, _ftBeatSpeed * Time.deltaTime);
                    else {
                        if (listGO[i].GetComponent<Renderer>().material.name.Contains(_materials[0].name))
                            _goPlayer.transform.GetComponent<PlayerStats>()._intPlayerScoring += 10;
                        Destroy(listGO[i]); }
                }   
            }
            else
            {
                if (_intSpawnpoint < 3)
                {
                    Vector3 _vec3Heading = _goBridges[2].transform.position - listGO[i].transform.position;
                    if (!(_vec3Heading.sqrMagnitude < _ftDistanceCheck * _ftDistanceCheck))
                        listGO[i].transform.position = Vector3.MoveTowards(listGO[i].transform.position, _goBridges[2].transform.position, _ftBeatSpeed * Time.deltaTime);
                    else {
                        if (listGO[i].GetComponent<Renderer>().material.name.Contains(_materials[0].name))
                            _goPlayer.transform.GetComponent<PlayerStats>()._intPlayerScoring += 10;
                        Destroy(listGO[i]); }
                }
                else {
                    Vector3 _vec3Heading = _goBridges[3].transform.position - listGO[i].transform.position;
                    if (!(_vec3Heading.sqrMagnitude < _ftDistanceCheck * _ftDistanceCheck))
                        listGO[i].transform.position = Vector3.MoveTowards(listGO[i].transform.position, _goBridges[3].transform.position, _ftBeatSpeed * Time.deltaTime);    
                    else {
                        if (listGO[i].GetComponent<Renderer>().material.name.Contains(_materials[0].name))
                            _goPlayer.transform.GetComponent<PlayerStats>()._intPlayerScoring += 10;
                        Destroy(listGO[i]); }
                }
            }
        }
    }

    void DelayedSpawnBeats(bool _blTrigger)
    {
        float _ftRandomRange = 0;
        switch (_goPlayer.GetComponent<PlayerStats>()._intPlayerDifficulty)
        {
            case 1:
                {
                    _ftRandomRange = Random.Range(1.0f, 4.5f);
                    break;
                }
            case 2:
                {
                    _ftRandomRange = Random.Range(2.5f, 5.5f);
                    break;
                }
            default:
                {
                    _ftRandomRange = Random.Range(2.5f, 10.5f);
                    break;
                }
        }

        if ((_ftWaitTimer += 1 * Time.deltaTime) >= _ftRandomRange)
        {
            for (int i = 0; i < 6; i++)
            {
                Color _colRandom = _colAry[i];
                if (_goAryPrefabLeft[i] != null)
                    _goAryPrefabRight[i].transform.GetComponent<Renderer>().material.color = _colRandom;

                if (_goAryPrefabRight[i] != null)
                    _goAryPrefabLeft[i].transform.GetComponent<Renderer>().material.color = _colRandom;
            }

            StartCoroutine(SpawnBeats());

            _ftWaitTimer = 0f;
        }
    }

    IEnumerator SpawnBeats()
    {
        int _intSpawnpoint = Random.Range(0, 11);

        GameObject _go = Instantiate(_goPrefab[1], _goContainer[1].transform, false);
        _go.transform.localScale = new Vector3(1f, 1f, 1f);

        if (_intSpawnpoint > 5)
        {
            _intSpawnpoint -= 5;
            _go.name = "R";
            _go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _go.transform.position = _goAryPrefabRight[_intSpawnpoint].transform.position;
            _go.transform.GetComponent<Renderer>().material = _materials[Random.Range(1, _materials.Length)];
        }
        else
        {
            _go.name = "L";
            _go.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _go.transform.position = _goAryPrefabLeft[_intSpawnpoint].transform.position;
            _go.transform.GetComponent<Renderer>().material = _materials[Random.Range(1, _materials.Length)];
        }
    
            _go.name += _intSpawnpoint.ToString();
            _go.transform.LookAt(Camera.main.transform);

        _go.SetActive(true);
        yield break;
    }
}
