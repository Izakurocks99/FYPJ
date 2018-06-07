using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBeat : MonoBehaviour {

	Transform _tfPosition;

    public float _intShiftRate;

	void Start() {
		_tfPosition = this.transform;
        if (_intShiftRate == 0)
            _intShiftRate = 1.0f;

    }

	void Update () {
		_tfPosition.position += new Vector3(0, Mathf.Sin(Time.time * _intShiftRate) * Time.deltaTime, 0);
	}
}
