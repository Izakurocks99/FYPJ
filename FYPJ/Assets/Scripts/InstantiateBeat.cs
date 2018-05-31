using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBeat : MonoBehaviour {

    public GameObject _goPrefab;
    GameObject[] _goAryPrefabLeft = new GameObject[10];
    GameObject[] _goAryPrefabRight = new GameObject[10];
    public float _ftBeatScale;

    void Start () {
        for (int i = 0; i < 10; i++)
        {
            GameObject _go1 = Instantiate(_goPrefab, this.transform, false);
            _go1.name = "Sample Cube Left " + i;
            _go1.transform.eulerAngles = new Vector3(0, -5.625f * (i + 0.5f), 0);
            _go1.transform.position = _go1.transform.forward * 15;
            _go1.SetActive(true);
            _goAryPrefabLeft[i] = _go1;

            GameObject _go2 = Instantiate(_goPrefab, this.transform, false);
            _go2.name = "Sample Cube Right " + i;
            _go2.transform.eulerAngles = new Vector3(0, 5.625f * (i + 0.5f), 0);
            _go2.transform.position = _go2.transform.forward * 15;
            _go2.SetActive(true);
            _goAryPrefabRight[i] = _go2;
        }
    }
	
	void Update () {
        for (int i = 0; i < 10; i++)
        {
            if (_goAryPrefabLeft[i] != null)
            {
                _goAryPrefabLeft[i].transform.localScale = new Vector3(0.5f, AudioPeer._samples[i] * _ftBeatScale + 1, 0.5f);
                _goAryPrefabLeft[i].transform.position = new Vector3(_goAryPrefabLeft[i].transform.position.x, (AudioPeer._samples[i] * _ftBeatScale + 1)/2, _goAryPrefabLeft[i].transform.position.z);
            }
            if (_goAryPrefabRight[i] != null)
            {
                _goAryPrefabRight[i].transform.localScale = new Vector3(0.5f, AudioPeer._samples[i] * _ftBeatScale + 1, 0.5f);
                _goAryPrefabRight[i].transform.position = new Vector3(_goAryPrefabRight[i].transform.position.x, (AudioPeer._samples[i] * _ftBeatScale + 1) / 2, _goAryPrefabRight[i].transform.position.z);
            }
        }
    }
}
