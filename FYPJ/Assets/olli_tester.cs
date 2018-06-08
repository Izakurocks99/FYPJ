using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class olli_tester : MonoBehaviour {

    [SerializeField]
    Shader bloomShader;

    Material bloom;

	void OnRenderImage (RenderTexture source, RenderTexture destination) {

        if (bloom == null) {
			bloom = new Material(bloomShader);
			bloom.hideFlags = HideFlags.HideAndDontSave;
		}

        int widht = source.width / 8;
        int height = source.height / 8;
        RenderTexture r = RenderTexture.GetTemporary(
            widht, height, 0,source.format );  // 0 for indicating that we wont fo anything for depth buffer

		Graphics.Blit(source,r, bloom);
		Graphics.Blit(r, destination, bloom);
        RenderTexture.ReleaseTemporary(r);




        // shader stuff

	}

	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}
}
