using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    Shader sha = null;
    [SerializeField]
    Shader GlassShader = null;
    Light light = null;
    Material m = null;
    GameObject parent = null;
    Material GlassMat = null;
	void Start () {
        Debug.Assert(sha);
        Debug.Assert(GlassShader);
        parent = this.transform.parent.gameObject;
        Debug.Assert(parent);
        light = parent.GetComponentInChildren<Light>();
        Debug.Assert(light);
        m = new Material(sha);
        this.GetComponentInChildren<MeshRenderer>().material = m;
        GlassMat = new Material(GlassShader);
        parent.GetComponent<MeshRenderer>().material = GlassMat;
    }

    // Update is called once per frame
    void Update () {
        Vector4 color = light.color;
        m.SetVector("_ColorScales",new Vector3(color.x,color.y,color.z ));
        GlassMat.SetVector("_Color",color);
	}
}

// todo rotation stuff
