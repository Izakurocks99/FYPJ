using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRange : MonoBehaviour {

	public GameObject prefab;
	public GameObject container;
	public float _ftSegregation;

	void Start () {
		_ftSegregation = 90.0f / _ftSegregation;

		for (int i = 0; i < 8; i++)
		{
			GameObject g1 = Instantiate(prefab, container.transform, false);
			g1.name = "Spawn Point " + i;
			g1.transform.eulerAngles = new Vector3(0, _ftSegregation * i, 0);
			g1.transform.position += g1.transform.forward * 12f;
			g1.transform.localScale *= 6.0f;
			g1.transform.LookAt(Camera.main.transform);
			g1.SetActive(true);
		}
		container.transform.eulerAngles = new Vector3(0, -31.5f, 0);
	}
}
