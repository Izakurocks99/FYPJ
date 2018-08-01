using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColorChanger : MonoBehaviour {

    [SerializeField]
    [ColorUsageAttribute(false,true)]
    List<Color> Colors = new List<Color>();
	// Use this for initialization
    Material mat = null;
    [SerializeField]
    float timeToChange = 1f;
    float timer = 0;
    int index = 0;
	void Start () {
        Debug.Assert(Colors.Count > 1);
        mat = this.GetComponent<Renderer>().material;
        Debug.Assert(mat);

        mat.SetColor("_Color",Colors[0]);
        mat.SetColor("_Color1",Colors[1]);
        index++;
	}
	
	// Update is called once per frame
	void Update () {
	    timer += Time.deltaTime;	
        if(timer > timeToChange)
        {
            timer = 0;
            int nextIndex = index + 1 < Colors.Count ? index + 1 : 0;
            mat.SetColor("_Color",Colors[index]);
            mat.SetColor("_Color1",Colors[nextIndex]);
            index = nextIndex;
        }

        mat.SetFloat("_Interpolation",timer / timeToChange);
	}
}
