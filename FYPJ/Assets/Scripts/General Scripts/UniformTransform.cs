using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniformTransform : MonoBehaviour {

	void Update () {
		if (this.transform.localScale != Vector3.one)
			this.transform.localScale = Vector3.one;
	}
}
