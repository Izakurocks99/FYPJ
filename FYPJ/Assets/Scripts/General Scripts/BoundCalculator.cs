using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundCalculator : MonoBehaviour {

	Renderer _rend;
	public Vector3[] _vec3Points;
	public Vector3 _vec3Center;
	public float _ftXRadius;
	public float _ftYRadius;

	void Start () {
		_rend = GetComponent<Renderer>();
		_vec3Points = new Vector3[4];
	}
	
	void Update () {
		_vec3Center = _rend.bounds.center;
		_ftXRadius = _rend.bounds.extents.x;
		_ftYRadius = _rend.bounds.extents.y;

		getVec3Points();
	}

	Vector3[] getVec3Points () {
		_vec3Points[0] = _vec3Center + new Vector3(-_ftXRadius, +_ftYRadius, 0);
		_vec3Points[1] = _vec3Center + new Vector3(+_ftXRadius, +_ftYRadius, 0);
		_vec3Points[2] = _vec3Center + new Vector3(-_ftXRadius, -_ftYRadius, 0);
		_vec3Points[3] = _vec3Center + new Vector3(+_ftXRadius, -_ftYRadius, 0);

		return _vec3Points;
	}
}
