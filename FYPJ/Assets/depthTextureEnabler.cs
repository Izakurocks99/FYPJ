using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class depthTextureEnabler : MonoBehaviour {

    // Use this for initialization 
    //[SerializeField]
    //GameObject camera = null;
	void Start () {
        //camera = this.GetComponent<Camera>() as Camera;
        //Debug.Assert(camera);
        Camera cam = this.GetComponent<Camera>() as Camera;
        Debug.Assert(cam);
        cam.depthTextureMode = DepthTextureMode.Depth;

    }
	
}
