using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRange : MonoBehaviour {

	public GameObject prefab;
	public GameObject container;

	void Start () {
		for (int i = 0; i < 8; i++)
		{
			GameObject g1 = Instantiate(prefab, container.transform, false);
			g1.name = "Spawn Point " + i;
			g1.transform.eulerAngles = new Vector3(0, 9f * i, 0);
			g1.transform.position += g1.transform.forward * 16f;
			g1.transform.localScale *= 1.2f;
			g1.SetActive(true);
		}
		container.transform.eulerAngles = new Vector3(0, -31.5f, 0);
	}
}
