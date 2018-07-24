using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSetter : MonoBehaviour {

    [SerializeField]
    Shader sha = null;

    [SerializeField]
    List<Color> colors = new List<Color>();
	// Use this for initialization
	void Start () {
        Debug.Assert(sha);
        Material m = new Material(sha);
        this.GetComponent<MeshRenderer>().material = m;
        Debug.Assert(colors.Count != 0);
        m.SetColor("_Color",colors[Random.Range(0,colors.Count -1)]);
        colors = null;
	}
}
