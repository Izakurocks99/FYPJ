﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraFollower : MonoBehaviour {
    [SerializeField]
    Camera MainCamera = null;
    [SerializeField]
    bool moveposition = false;
	// Use this for initialization
	void Start () {
        //MainCamera = this.GetComponent("MainCamera") as Camera;
        Debug.Assert(MainCamera);
        this.transform.position = new Vector3(0, 0, 0);//MainCamera.transform.position;
        this.transform.rotation = MainCamera.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Assert(MainCamera);
        if (moveposition)
            this.transform.position = MainCamera.transform.position;//new Vector3(0, 0, 0);
        else
            this.transform.position = new Vector3(0, 0, 0);
        this.transform.rotation = MainCamera.transform.rotation;
	}
}
