using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour {

	void Update () {

        if (Input.GetKey(KeyCode.A))
            Camera.main.transform.eulerAngles -= new Vector3(0.0f, 80.0f, 0.0f) * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            Camera.main.transform.eulerAngles += new Vector3(0.0f, 80.0f, 0.0f) * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            Camera.main.transform.eulerAngles -= new Vector3(80.0f, 0.0f, 0.0f) * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            Camera.main.transform.eulerAngles += new Vector3(80.0f, 0.0f, 0.0f) * Time.deltaTime;
    }
}
