using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode , ImageEffectAllowedInSceneView]
public class BlurTesting : MonoBehaviour {

    [SerializeField]
    [Range(0,8)]
    int Iterations = 0;

    RenderTexture[] textures = new RenderTexture[16];

    Material material = null;
    [SerializeField]
    Shader shader = null;

    [Range(0, 10)]
	public float threshold = 1;

    [Range(0, 2)]
    public float softTreshold = 0.5f;

    [SerializeField]
    bool DebugBuffer = false;

    [SerializeField]
    [Range(0,6)]
    float Intensity = 1;


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Debug.Assert(shader);
        if (material == null)
        {
            material = new Material(shader); 	
            material.hideFlags = HideFlags.HideAndDontSave;

        }
        material.SetFloat("_Threshold", threshold);
        material.SetFloat("_SoftThreshold", softTreshold);
        material.SetFloat("_Intensity",  Mathf.GammaToLinearSpace(Intensity));

        RenderTextureFormat format = source.format;
        int width = source.width;
        int height = source.height;
        RenderTexture CurrentDestination = textures[0] = RenderTexture.GetTemporary(
			 width,height, 0,format
		);
        Graphics.Blit(source, CurrentDestination,material,0);
		RenderTexture currentSource = CurrentDestination;

        int i = 1;
        for (; i < Iterations; i++) {
			width /= 2;
			height /= 2;
            	if (height < 2) {
				break;
			}
			CurrentDestination = textures[i] = 
				RenderTexture.GetTemporary(width, height, 0, format);
			Graphics.Blit(currentSource, CurrentDestination,material,1);
			currentSource = CurrentDestination;
		}

        
		for (i -= 2; i >= 0; i--) {
			CurrentDestination = textures[i];
			textures[i] = null;
			Graphics.Blit(currentSource, CurrentDestination,material,2);
			RenderTexture.ReleaseTemporary(currentSource);
			currentSource = CurrentDestination;
		}

        if (DebugBuffer) {
			Graphics.Blit(currentSource, destination, material, 4);
		}
		else {
			material.SetTexture("_SourceTex", source);
			Graphics.Blit(currentSource, destination, material, 3);
		}
		RenderTexture.ReleaseTemporary(currentSource);
    }
}
