using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AudioMotion : MonoBehaviour {

	Transform _tfCamera;
	Transform _tfParent;
	Transform _tfThis;
    public float _intShiftRate;
	public float _ftRange;
	float _ftX, _ftY, _ftZ;
	int _intNumber;
	Vector3 _vec3Area;
    ControlCalibrationScript calibration;

	void Start() {
		_tfThis = this.transform;
		_tfParent = this.transform.parent.transform;
		_tfCamera = Camera.main.transform;

		_intNumber = int.Parse(Regex.Replace(_tfThis.name, "[^0-9]", ""));

        calibration = FindObjectOfType<ControlCalibrationScript>();
		_ftX = Random.Range(-calibration.horizontalSize, calibration.horizontalSize);
		_ftY = Random.Range(-calibration.verticleSize, calibration.verticleSize);
		_ftZ = 1.2f;
    }

	void Update () {
		_vec3Area = (_tfCamera.position + _tfCamera.forward * 0.5f) + new Vector3(_ftX,
													 _ftY,
													 _ftZ);
		_tfThis.Rotate(_tfThis.up, 7.0f);
		
		BeatMotion();
		TransitBeat();
	}

	void BeatMotion() {
		_tfThis.position += new Vector3(0, Mathf.Sin(Time.time * _intShiftRate) * Time.deltaTime, 0);
	}

	void TransitBeat() {
		Vector3 _vec3Heading = _vec3Area - _tfThis.position;
		if (!(_vec3Heading.sqrMagnitude < 0.1f * 0.1f))
			_tfThis.position = Vector3.MoveTowards(_tfThis.position, _vec3Area, 0.1f);
		else
			Destroy(_tfThis.gameObject);
	}
}
