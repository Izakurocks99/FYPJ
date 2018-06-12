using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class render_test : MonoBehaviour {

    Material material = null;
    [SerializeField]
    Shader sha = null;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Debug.Assert(sha != null);
        if (material == null)
        {
            material = new Material(sha);
        }

    }
}
