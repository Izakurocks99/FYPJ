using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AudioMotion : MonoBehaviour {

	Transform _tfCamera;
	Transform _tfParent;
	Transform _tfThis;
    public float _intShiftRate;
	int _intNumber;

	void Start() {
		_tfThis = this.transform;
		_tfParent = this.transform.parent.transform;
		_tfCamera = Camera.main.transform;

		_intNumber = int.Parse(Regex.Replace(_tfThis.name, "[^0-9]", ""));
    }

	void Update () {
		_tfThis.Rotate(_tfThis.up, 7.0f);
		
		BeatMotion();
		TransitBeat();
	}

	void BeatMotion() {
		_tfThis.position += new Vector3(0, Mathf.Sin(Time.time * _intShiftRate) * Time.deltaTime, 0);
	}

	void TransitBeat() {
		Vector3 _vec3Heading = _tfCamera.position - _tfThis.position;
		if (!(_vec3Heading.sqrMagnitude < 0.1f * 0.1f))
			_tfThis.position = Vector3.MoveTowards(_tfThis.position, _tfCamera.position, 0.1f);
		else
			Destroy(_tfThis.gameObject);
	}
}
