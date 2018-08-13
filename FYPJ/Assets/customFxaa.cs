using UnityEngine;
using System;

//[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class customFxaa : MonoBehaviour
{

    [SerializeField]
    Shader fxaaShader = null;

    [NonSerialized]
    Material fxaaMaterial = null;
#if (LUMINANCE)
    [SerializeField]
    Shader luminanceShader = null;

    [NonSerialized]
    Material luminanceMaterial = null;
#endif 

    public enum LuminanceMode { Alpha, Green, Calculate }

    public LuminanceMode luminanceSource = LuminanceMode.Calculate;

    [Range(0.0312f, 0.0833f)]
    public float contrastThreshold = 0.0312f;
    [Range(0.063f, 0.333f)]
    public float relativeThreshold = 0.063f;
    [Range(0f, 1f)]
    public float subpixelBlending = 0.75f;
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        if (fxaaMaterial == null)
        {
            fxaaMaterial = new Material(fxaaShader);
            fxaaMaterial.hideFlags = HideFlags.HideAndDontSave;
#if (LUMINANCE)
            luminanceMaterial = new Material(luminanceShader);
            luminanceMaterial.hideFlags = HideFlags.HideAndDontSave;
#endif
        }
        fxaaMaterial.SetFloat("_ContrastThreshold", contrastThreshold);
        fxaaMaterial.SetFloat("_RelativeThreshold", relativeThreshold);
        fxaaMaterial.SetFloat("_SubpixelBlending", subpixelBlending);

        if (luminanceSource == LuminanceMode.Calculate)
        {
#if (LUMINANCE)
            fxaaMaterial.DisableKeyword("LUMINANCE_GREEN");
            RenderTexture luminanceTex = RenderTexture.GetTemporary(
                source.width, source.height, 0, source.format
            );
            Graphics.Blit(source, luminanceTex, luminanceMaterial);
            Graphics.Blit(luminanceTex, destination, fxaaMaterial);
            RenderTexture.ReleaseTemporary(luminanceTex);
#endif
        }
        else
        {
            if (luminanceSource == LuminanceMode.Green)
            {
                fxaaMaterial.EnableKeyword("LUMINANCE_GREEN");
            }
            else
            {
                fxaaMaterial.DisableKeyword("LUMINANCE_GREEN");
            }
            Graphics.Blit(source, destination, fxaaMaterial);
        }
    }
}
