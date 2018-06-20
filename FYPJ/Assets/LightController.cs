using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    Shader sha = null;

    Light light = null;
    Material m = null;
    GameObject parent = null;
	void Start () {
        Debug.Assert(sha);
        parent = this.transform.parent.gameObject;
        Debug.Assert(parent);
        light = parent.GetComponentInChildren<Light>();
        Debug.Assert(light);
        m = new Material(sha);
        this.GetComponentInChildren<MeshRenderer>().material = m;

    }

    // Update is called once per frame
    void Update () {
        Vector4 color = light.color;
        m.SetVector("_ColorScales",new Vector3(color.x,color.y,color.z ));	
	}
}

// todo rotation stuff
