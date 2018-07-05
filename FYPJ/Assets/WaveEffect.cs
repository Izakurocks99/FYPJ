using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffect : MonoBehaviour {

    [SerializeField]
    Shader WaveShader = null;
    [SerializeField]
    Vector3 pos = new Vector3(0, 0, 0);
    [SerializeField]
    float ray = 2;
    [SerializeField]
    float ThiccBoi = 2;
    [SerializeField]
    Color col = new Color(1, 1, 1, 1);
    [SerializeField]
    Color col1 = new Color(1, 1, 1, 1);
    [SerializeField]
    Color col2 = new Color(1, 1, 1, 1);


    Material mat = null;

    Camera _camera = null;
    // Update is called once per frame
    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (mat == null)
        {
            mat = new Material(WaveShader);
            _camera = GetComponent<Camera>();
            _camera.depthTextureMode = DepthTextureMode.Depth;


        }
        mat.SetFloat("_Ray", ray);
        mat.SetFloat("_Thicc", ThiccBoi);
        mat.SetVector("_EffectPosition", pos + transform.position);//(Vector3)(this.transform.localToWorldMatrix * this.transform.position));
        mat.SetVector("_EffectColor", col);//(Vector3)(this.transform.localToWorldMatrix * this.transform.position));
        mat.SetVector("_EffectColor2", col1);//(Vector3)(this.transform.localToWorldMatrix * this.transform.position));
        mat.SetVector("_EffectColor3", col2);//(Vector3)(this.transform.localToWorldMatrix * this.transform.position));
        RaycastCornerBlit(source, destination, mat);
    }
    [SerializeField]
    float TimeToComplete = 2f;
    [SerializeField]
    float Range = 40;
    float Timer = 0;
    [SerializeField]
    bool pulsing = false;
    public void Pulse()
    {
        pulsing = true;
    }
    private void Update()
    {
        if (pulsing)
        {
            Timer += Time.deltaTime;
            ray = Range * (Timer / TimeToComplete);
            if (ray > Range)
            {
                Timer = 0;
                pulsing = false;
            }

        }
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





    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(pos, ray); 
    //}
}
