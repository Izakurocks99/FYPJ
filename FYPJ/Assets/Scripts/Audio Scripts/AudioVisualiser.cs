using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class AudioVisualiser : MonoBehaviour {

    public GameObject _goRParent;
    public GameObject _goLParent;
    public static float[] _ftRSamplesbands;
    public static float[] _ftLSamplesbands;
    public static float[] _ftRSamplesbuffer;
    public static float[] _ftLSamplesbuffer;
    float[] _ftRBufferDecrease;
    float[] _ftLBufferDecrease;

    void Start() {
        _ftRSamplesbands = new float[_goRParent.transform.childCount];
        _ftLSamplesbands = new float[_goLParent.transform.childCount];

        _ftRSamplesbuffer = new float[_goRParent.transform.childCount];
        _ftLSamplesbuffer = new float[_goLParent.transform.childCount];
        
        _ftRBufferDecrease = new float[_goRParent.transform.childCount];
        _ftLBufferDecrease = new float[_goLParent.transform.childCount];
    }

    void Update() {
        if (this.GetComponent<AudioSource>().isPlaying == true &&
            this.GetComponent<AudioSource>().clip != null) {

            UpdateSamples();
            ScaleBuffer();
            ScaleVisualise();
        }
    }

    void UpdateSamples() {
        for (int i = 0; i < _ftRSamplesbands.Length; i++)
            _ftRSamplesbands[i] = AudioSampler._ftSamples[i];
        for (int j = 1; j < _ftRSamplesbands.Length; j++)
            _ftLSamplesbands[j - 1] = AudioSampler._ftSamples[j];
    }

    void ScaleBuffer() {
        for (int k = 0; k < _goRParent.transform.childCount; k++) {
            if (_ftRSamplesbands[k] > _ftRSamplesbuffer[k]) {
                _ftRSamplesbuffer[k] = _ftRSamplesbands[k];
                _ftRBufferDecrease[k] = 0.005f;
            }
            else if (_ftRSamplesbands[k] < _ftRSamplesbuffer[k]) {
                _ftRSamplesbuffer[k] -= _ftRBufferDecrease[k];
                _ftRBufferDecrease[k] *= 1.2f;
                if (_ftRSamplesbuffer[k] < 0.04f)
                    _ftRSamplesbuffer[k] = 0.04f;
            }
        }
        for (int q = 0; q < _goRParent.transform.childCount; q++) {
            if (_ftRSamplesbands[q] > _ftRSamplesbuffer[q]) {
                _ftRSamplesbuffer[q] = _ftRSamplesbands[q];
                _ftRBufferDecrease[q] = 0.005f;
            }
            else if (_ftRSamplesbands[q] < _ftRSamplesbuffer[q]) {
                _ftRSamplesbuffer[q] -= _ftRBufferDecrease[q];
                _ftRBufferDecrease[q] *= 1.2f;
                if (_ftRSamplesbuffer[q] < 0.04f)
                    _ftRSamplesbuffer[q] = 0.04f;
            }
        }
    }

    void ScaleVisualise() {
        for (int i = 0; i < _goRParent.transform.childCount; i++) {
            if (_goRParent.transform.GetChild(i) != null) {
                _goRParent.transform.GetChild(i).transform.localScale = new Vector3(2.0f, _ftRSamplesbands[i] * 500.0f + 10.0f, 2.0f);
            }
        }
        for (int j = 1; j < _goLParent.transform.childCount; j++) {
            if (_goLParent.transform.GetChild(j - 1) != null) {
                _goLParent.transform.GetChild(j - 1).transform.localScale = new Vector3(2.0f, _ftLSamplesbands[j] * 500.0f + 10.0f, 2.0f);
            }
        }
    }
}
