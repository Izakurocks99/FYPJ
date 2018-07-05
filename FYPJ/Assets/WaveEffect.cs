using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffect : MonoBehaviour {

    [SerializeField]
    Shader WaveShader = null;
    [SerializeField]
    Vector3 pos = new Vector3(0,0,0);
    [SerializeField]
    float ray = 2;

    Material mat = null;
	// Update is called once per frame
	void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (mat == null)
        {
            mat = new Material(WaveShader);
        }
        mat.SetFloat("_Ray", ray);
        mat.SetVector("_EffectPosition", pos);
        Graphics.Blit(source, destination, mat);
    }
}
