using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class AudioVisualiser : MonoBehaviour {

    public GameObject _goPrefab;
    public GameObject _goContainer;
    GameObject[] _goArySampleCubes = new GameObject[512];

    void Start() {
        for (int i = 0; i < 512; i++) {
            GameObject _go = Instantiate(_goPrefab, _goContainer.transform, false);
            _go.name = "Sample " + i;
            _go.transform.eulerAngles = new Vector3(0, -15f * i, 0f);
            _go.transform.position = _go.transform.forward * 10f;
            _go.SetActive(true);
        }
    }

    void Update() {
        for (int i = 0; i < 512; i++)
        {
            if (_goArySampleCubes != null)
            _goContainer.transform.GetChild(i).transform.localScale = new Vector3(1f, AudioSampler._ftSamples[i] * 10f + 1, 1f);
        }
    }
}
