using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO do either blurr or somekind of lining for the object
// world position?
// model position?
// movement ?
// scale ?
// emission ?
//
public class BlurryScript : MonoBehaviour {

    //Material mat = null;    
    [SerializeField] 
    Shader sha = null;
	// Use this for initialization
    //[ImageEffectOpaque]
	void OnEnable () {
        this.GetComponent<Camera>().SetReplacementShader(sha,"Outlined"); // or outlined ?
	}
	void OnDisable () {
        this.GetComponent<Camera>().ResetReplacementShader();
	}

}
