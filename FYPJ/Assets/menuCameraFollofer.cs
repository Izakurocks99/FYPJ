using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuCameraFollofer : MonoBehaviour {


    [SerializeField]
    GameObject MainCamera = null;
	// Use this for initialization
	void Start () {
        Debug.Assert(MainCamera);
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = MainCamera.transform.position;	
        this.transform.rotation = MainCamera.transform.rotation;	
	}
}
