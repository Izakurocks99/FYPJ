using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour {

    [SerializeField]
    float speed = 1f;
	void Update () {

        float _speed = speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            Camera.main.transform.eulerAngles -= new Vector3(0.0f, _speed, 0.0f) ;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            Camera.main.transform.eulerAngles += new Vector3(0.0f, _speed, 0.0f) ;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            Camera.main.transform.eulerAngles -= new Vector3(_speed, 0.0f, 0.0f) ;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            Camera.main.transform.eulerAngles += new Vector3(_speed, 0.0f, 0.0f) ;
    }
}
