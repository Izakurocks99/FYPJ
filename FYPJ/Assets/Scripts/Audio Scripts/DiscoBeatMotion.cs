using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

public class DiscoBeatMotion : MonoBehaviour {
	
	Transform _tfCamera;
	Transform _tfParent;
	Transform _tfThis;
    public float _intShiftRate;
	public Vector3 _vec3Area;
	public Vector3 _vec3Shift;

	void Start() {
		_tfThis = this.transform;
		_tfParent = this.transform.parent.transform;
		_tfCamera = Camera.main.transform;
		_vec3Area = new Vector3(-0.5f, 0, 0);
    }

	void Update () {		
		_tfThis.Rotate(_tfThis.forward, 7.0f);
		
		BeatMotion();
		TransitBeat();
	}

	void BeatMotion() {
		_tfThis.position += new Vector3(0, Mathf.Sin(Time.time * _intShiftRate) * Time.deltaTime, 0);
	}

	void TransitBeat() {
		Vector3 _vec3Heading = _vec3Area + _vec3Shift + _tfCamera.position + _tfCamera.forward * 0.5f - _tfThis.position;
		if (!(_vec3Heading.sqrMagnitude < 0.1f * 0.1f))
			_tfThis.position = Vector3.MoveTowards(_tfThis.position,
												   _vec3Area + _vec3Shift + _tfCamera.position + _tfCamera.forward * 0.5f,
			 									   0.075f);
		else
			Destroy(_tfThis.gameObject);
	}
}
