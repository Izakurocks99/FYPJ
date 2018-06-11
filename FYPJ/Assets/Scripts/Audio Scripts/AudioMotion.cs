using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AudioMotion : MonoBehaviour {

	Transform _tfGrandParent;
	Transform _tfParent;
	Transform _tfThis;
    public float _intShiftRate;
	float _ftSmoothTIme;
	Vector3 _vec3Velocity;

	void Start() {
		_tfThis = this.transform;
		_tfParent = this.transform.parent.transform;
		_tfGrandParent = _tfParent.parent.transform;

        if (_intShiftRate == 0)
            _intShiftRate = 1.0f;
		_ftSmoothTIme = 0.7f;
    }

	void Update () {
		_tfThis.Rotate(_tfThis.up, 5.0f);
		
		BeatMotion();
		TransitBeat();
	}

	void BeatMotion() {
		_tfThis.position += new Vector3(0, Mathf.Sin(Time.time * _intShiftRate) * Time.deltaTime, 0);
	}

	void TransitBeat() {
		Vector3 _vec3Heading = _tfGrandParent.position - _tfThis.position;
		if (!(_vec3Heading.sqrMagnitude < 0.1f * 0.1f))
			_tfThis.position = Vector3.MoveTowards(_tfThis.position, _tfGrandParent.position, 0.1f);
		else
			Destroy(_tfThis.gameObject);
	}
}
