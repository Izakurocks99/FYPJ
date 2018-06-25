using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour {

	void Update () {

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            Camera.main.transform.eulerAngles -= new Vector3(0.0f, 80.0f, 0.0f) * Time.deltaTime;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            Camera.main.transform.eulerAngles += new Vector3(0.0f, 80.0f, 0.0f) * Time.deltaTime;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            Camera.main.transform.eulerAngles -= new Vector3(80.0f, 0.0f, 0.0f) * Time.deltaTime;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            Camera.main.transform.eulerAngles += new Vector3(80.0f, 0.0f, 0.0f) * Time.deltaTime;
    }
}
