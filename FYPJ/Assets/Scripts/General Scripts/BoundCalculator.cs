using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundCalculator : MonoBehaviour {

	public GameObject _goPrefab;
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
		// SpawnPoints();

		// Spawn8x();
	}

	Vector3[] getVec3Points () {
		_vec3Points[0] = _vec3Center + new Vector3(-_ftXRadius, +_ftYRadius, 0);
		_vec3Points[1] = _vec3Center + new Vector3(+_ftXRadius, +_ftYRadius, 0);
		_vec3Points[2] = _vec3Center + new Vector3(-_ftXRadius, -_ftYRadius, 0);
		_vec3Points[3] = _vec3Center + new Vector3(+_ftXRadius, -_ftYRadius, 0);

		return _vec3Points;
	}

	void SpawnPoints () {
		foreach (Vector3 _vec3 in _vec3Points) {
			GameObject go = Instantiate(_goPrefab, _vec3, Quaternion.identity);
		}
	}

	void Spawn8x () {
		GameObject go0 = Instantiate(_goPrefab, _vec3Points[0] + new Vector3(+1.0f, +0.0f, 0.0f), Quaternion.identity);
		// go0.name = "go0";
		GameObject go5 = Instantiate(_goPrefab, _vec3Points[0] + new Vector3(+0.0f, -1.0f, 0.0f), Quaternion.identity);
		// go5.name = "go5";
		GameObject go1 = Instantiate(_goPrefab, _vec3Points[1] + new Vector3(+0.0f, -1.0f, 0.0f), Quaternion.identity);
		// go1.name = "go1";
		GameObject go4 = Instantiate(_goPrefab, _vec3Points[1] + new Vector3(-1.0f, +0.0f, 0.0f), Quaternion.identity);
		// go4.name = "go4";
		GameObject go2 = Instantiate(_goPrefab, _vec3Points[2] + new Vector3(+0.0f, +1.0f, 0.0f), Quaternion.identity);
		// go2.name = "go2";
		GameObject go7 = Instantiate(_goPrefab, _vec3Points[2] + new Vector3(+1.0f, +0.0f, 0.0f), Quaternion.identity);
		// go7.name = "go7";
		GameObject go3 = Instantiate(_goPrefab, _vec3Points[3] + new Vector3(-1.0f, +0.0f, 0.0f), Quaternion.identity);
		// go3.name = "go3";
		GameObject go6 = Instantiate(_goPrefab, _vec3Points[3] + new Vector3(+0.0f, +1.0f, 0.0f), Quaternion.identity);
		// go6.name = "go6";
	}
}
