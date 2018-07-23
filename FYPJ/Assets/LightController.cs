using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    Shader sha = null;
    [SerializeField]
    Shader GlassShader = null;
    Light _light = null;
    Material m = null;
    GameObject parent = null;
    Material GlassMat = null;
    [SerializeField]
    [Range(1, 10)]
    float scale = 1f;
	void Start () {
        Debug.Assert(sha);
        Debug.Assert(GlassShader);
        parent = this.transform.parent.gameObject;
        Debug.Assert(parent);
        _light = parent.GetComponentInChildren<Light>();
        Debug.Assert(_light);
        m = new Material(sha);
        this.GetComponentInChildren<MeshRenderer>().material = m;
        GlassMat = new Material(GlassShader);
        parent.GetComponent<MeshRenderer>().material = GlassMat;
        Vector4 color = _light.color;
        m.SetVector("_ColorScales",new Vector3(color.x,color.y,color.z ));
        GlassMat.SetVector("_Color",color);
        GlassMat.SetFloat("Scale",scale);
    }

    // Update is called once per frame
   // void Update () {
	//}
}

// todo rotation stuff
