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
	public Vector3 _vec3Shift;
	public Vector3 _vec3Target;
	public Vector3 _vec3Area;
    public int speed;

	void Start() {
		_tfThis = this.transform;
		if (this.transform.parent.transform != null)
		_tfParent = this.transform.parent.transform;
		_tfCamera = Camera.main.transform;
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
		Vector3 _vec3Heading = _vec3Area + _vec3Shift + _vec3Target - _tfThis.position;
		if (!(_vec3Heading.sqrMagnitude < 0.1f * 0.1f))
			_tfThis.position = Vector3.MoveTowards(_tfThis.position,
												   _vec3Area + _vec3Shift + _vec3Target,
			 									   speed * Time.deltaTime);
		else
			Destroy(_tfThis.gameObject);
	}
}
