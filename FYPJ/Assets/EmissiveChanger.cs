using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissiveChanger : MonoBehaviour {


    // Use this for initialization
    [SerializeField]
    List<Texture2D> EmissiveTextures = new List<Texture2D>();
    [SerializeField]
    float changeTime = 2;
    [SerializeField]
    float lerpTime = 0.5f;
    float timer = 0;

    int findex = 0;
    int sindex = 1;
    Material mat = null;
	void Start () {
        Debug.Assert(EmissiveTextures.Count >= 2);
        mat = this.GetComponent<Renderer>().material;
        mat.SetTexture("_EmissiveTexAdd", EmissiveTextures[findex]);
        mat.SetTexture("_EmissiveTexAdd2", EmissiveTextures[sindex]);
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        float lerp = Mathf.Clamp(timer / lerpTime,0,1);
        if (timer > changeTime)
        {
            timer = 0;
            findex = findex >= EmissiveTextures.Count - 1 ? 0 : findex+1;
            sindex = sindex >= EmissiveTextures.Count - 1? 0 : sindex+1;
            mat.SetTexture("_EmissiveTexAdd", EmissiveTextures[findex]);
            mat.SetTexture("_EmissiveTexAdd2", EmissiveTextures[sindex]);
            lerp = 0;
        }
        mat.SetFloat("_LerpVal", lerp);
	}
}
