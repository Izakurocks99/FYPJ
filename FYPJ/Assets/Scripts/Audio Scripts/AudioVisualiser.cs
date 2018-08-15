using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class AudioVisualiser : MonoBehaviour {

    public GameObject _goRParent;
    public GameObject _goLParent;
    public GameObject _goCenter;
    GameObject[] _goRArray;
    GameObject[] _goLArray;
    public static float[] _ftSSamplesBuffer;
    public static float _ftCSampleBuffer;
    float[] _ftSBufferDecrease;
    float _ftCBufferDecrease;
    float _ftScale;

    void Start() {
        _ftSSamplesBuffer = new float[_goLParent.transform.childCount];
        _ftSBufferDecrease = new float[_goRParent.transform.childCount];
        _ftCBufferDecrease = 0.0f;
        _ftScale = 1.0f;

        _goRArray = new GameObject[_goRParent.transform.childCount];
        _goLArray = new GameObject[_goLParent.transform.childCount];

        for (int _intR =  0; _intR < _goRArray.Length; _intR++)
            _goRArray[_intR] = _goRParent.transform.GetChild(_intR).gameObject;
        for (int _intL =  0; _intL < _goLArray.Length; _intL++)
            _goLArray[_intL] = _goLParent.transform.GetChild(_intL).gameObject;
    }

    void Update() {
        if (this.GetComponent<AudioSource>().isPlaying == true &&
            this.GetComponent<AudioSource>().clip != null) {

            ScaleBuffer();
            ScaleVisualise();
        }
    }

    void ScaleBuffer() {
        if (AudioPlayer._ftDelayedSamples[0] > _ftCSampleBuffer) {
            _ftCSampleBuffer = AudioPlayer._ftDelayedSamples[0];
            _ftCBufferDecrease = (0.002f / 100f);
        }
        else if (AudioPlayer._ftDelayedSamples[0] < _ftCSampleBuffer) {
            _ftCSampleBuffer -= _ftCBufferDecrease;
            _ftCBufferDecrease *= 1.2f;
            if (_ftCSampleBuffer < (0.004f / 100f))
                _ftCSampleBuffer = (0.004f / 100f);
        }

        for (int i = 0; i < _ftSSamplesBuffer.Length; i++) {
            if (AudioPlayer._ftDelayedSamples[i + 1] > _ftSSamplesBuffer[i]) {
                _ftSSamplesBuffer[i] = AudioPlayer._ftDelayedSamples[i + 1];
                _ftSBufferDecrease[i] = (0.002f / 100f);
            }
            else if (AudioPlayer._ftDelayedSamples[i + 1] < _ftSSamplesBuffer[i]) {
                _ftSSamplesBuffer[i] -= _ftSBufferDecrease[i];
                _ftSBufferDecrease[i] *= 1.2f;
                if (_ftSSamplesBuffer[i] < (0.004f / 100f))
                    _ftSSamplesBuffer[i] = (0.004f / 100f);
            }
        }
    }

    void ScaleVisualise() {
        _goCenter.transform.localScale = new Vector3(_goCenter.transform.localScale.x * _ftScale,
                                                    (_ftCSampleBuffer * 250.0f) + _ftScale * 7.0f,
                                                     _goCenter.transform.localScale.z * _ftScale);
        
        for (int i = 0; i < _goRArray.Length; i++) {
            _goRArray[i].transform.localScale = new Vector3(_goRArray[i].transform.localScale.x * _ftScale,
                                                           (_ftSSamplesBuffer[i] * 250.0f) + _ftScale * 7.0f,
                                                            _goRArray[i].transform.localScale.z * _ftScale);
            _goLArray[i].transform.localScale = new Vector3(_goLArray[i].transform.localScale.x * _ftScale,
                                                           (_ftSSamplesBuffer[i] * 250.0f) + _ftScale * 7.0f,
                                                            _goLArray[i].transform.localScale.z * _ftScale);
        }
    }
}
