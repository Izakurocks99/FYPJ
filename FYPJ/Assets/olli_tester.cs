using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class olli_tester : MonoBehaviour {

    [SerializeField]
    Shader bloomShader = null;
    [SerializeField]
    [Range(0,10)]
    int numIterations = 4;

    Material bloom;


   	RenderTexture[] textures = new RenderTexture[16];


	void OnRenderImage (RenderTexture source, RenderTexture destination) {

        if (bloom == null) {
			bloom = new Material(bloomShader);
			bloom.hideFlags = HideFlags.HideAndDontSave;
		}
		RenderTextureFormat format = source.format;
#if false
        int width = source.width / 5;
		int height = source.height / 5;

		RenderTexture r =
			RenderTexture.GetTemporary(width, height, 0, format);
        	Graphics.Blit(source, r);
		Graphics.Blit(r, destination);
		RenderTexture.ReleaseTemporary(r);
#endif

#if true
        int widht = source.width;
        int height = source.height;
        RenderTexture currentDestination,currentSource = RenderTexture.GetTemporary(widht, height, 0, format);

        int i = 1;
        for (; i < numIterations; i++) {
			widht /= 2;
			height /= 2;
			if (height < 2) {
				break;
			}

            currentDestination = textures[i] = RenderTexture.GetTemporary(widht, height, 0, format);
			Graphics.Blit(currentSource, currentDestination);
			currentSource = currentDestination;
		}

        	for (i -= 2; i >= 0; i--) {
			currentDestination = textures[i];
			textures[i] = null;
			Graphics.Blit(currentSource, currentDestination);
			RenderTexture.ReleaseTemporary(currentSource);
			currentSource = currentDestination;
		}


        Graphics.Blit(currentSource, destination);

#endif





		//Graphics.Blit(source, currentDestination);
		// currentSource = currentDestination;
		//Graphics.Blit(currentSource, destination);
		//RenderTexture.ReleaseTemporary(currentSource);



  //      RenderTexture r = RenderTexture.GetTemporary(
  //          source.width, source.height, 0,source.format );  // 0 for indicating that we wont fo anything for depth buffer

		//Graphics.Blit(source,r, bloom);
		//Graphics.Blit(r, destination, bloom);
  //      RenderTexture.ReleaseTemporary(r);




        // shader stuff

	}

	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}
}
