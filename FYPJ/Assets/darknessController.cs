using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darknessController : MonoBehaviour {
    //DELETE THIS AFTER DARKNESS PASS


    //DELETE THIS AFTER DARKNESS PASS

    Material mat = null;

    [SerializeField]
    Shader sha = null;


    Camera _camera = null;

    [SerializeField]
    float dist = 50f;
    //[SerializeField]
    //[Range(0f, 1f)]
    //float percent = 0;

    [SerializeField]
    float thicc = 1f;
    [SerializeField]
    float waveLenght = 1f;
    [SerializeField]
    float  wavestrenght = 1f;

    [SerializeField]
    [ColorUsage(false, true)]
    Color edgecolor = new Color(1, 1, 1, 1);
    //[ImageEffect]

    [SerializeField]
    float TimeToFade = 5f;
    float timer = 0;
    [SerializeField]
    bool show = false;
    [SerializeField]
    GameObject DarkDome = null;
    void Start()
    {
        Debug.Assert(DarkDome);
    }
    public void Show()
    {
        show = true;
    }
    void Update()
    {
        if (mat == null)
        {
            mat = new Material(sha);
            _camera = GetComponent<Camera>();
        }


        if (show)
        {
            timer += Time.deltaTime;
            if (timer > TimeToFade)
            {
                //this.enabled = false;
                //DarkDome.SetActive(false);
                DarkDome.GetComponent<StartShow>().Show();
                this.enabled = false;
                //show = false;
                //timer = 0;
                return;
            }
            float _percent = timer / TimeToFade;
            mat.SetFloat("_percent", _percent);
        }
       
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
       
        Debug.Assert(sha);
        if (mat == null)
        {
            mat = new Material(sha);
            _camera = GetComponent<Camera>();
        }

        mat.SetFloat("_dist", dist);
        //mat.SetFloat("_percent", percent);
        mat.SetFloat("_WaveLenght", waveLenght);
        mat.SetFloat("_WaveStrenght", wavestrenght);
        mat.SetFloat("_Thicc", thicc);
        mat.SetColor("_EdgeColor", edgecolor);

        RaycastCornerBlit(source, destination, mat);
    }
    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
    {
        // Compute Frustum Corners
        float camFar = _camera.farClipPlane;
        float camFov = _camera.fieldOfView;
        float camAspect = _camera.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
        float camScale = topLeft.magnitude * camFar;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (_camera.transform.forward + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= camScale;

        // Custom Blit, encoding Frustum Corners as additional Texture Coordinates
        RenderTexture.active = dest;

        mat.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.MultiTexCoord(1, bottomLeft);
        GL.Vertex3(0.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.MultiTexCoord(1, bottomRight);
        GL.Vertex3(1.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.MultiTexCoord(1, topRight);
        GL.Vertex3(1.0f, 1.0f, 0.0f);

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.MultiTexCoord(1, topLeft);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
    }




    // Use this for initialization
    //void Start () {
		
	//}
	
	// Update is called once per frame
	//void Update () {
		
	//}
}
