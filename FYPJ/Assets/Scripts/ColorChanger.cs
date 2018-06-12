using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    float ChangeTime = 2f;

    [SerializeField]
    float InterpolationTime = 1f;
    float[] scales = new float[4];

    [SerializeField]
    GameObject SpaceLayer = null;
    [SerializeField]
    GameObject CloudLayer = null;
    [SerializeField]
    GameObject MountainLayer = null;


    [SerializeField]
    Texture2D[] SpaceTextures = new Texture2D[4];

    [SerializeField]
    Texture2D[] CloudTextures = new Texture2D[4];

    [SerializeField]
    Texture2D[] MountainTextures = new Texture2D[4];

    Renderer SpaceRenderer = null;
    Renderer CloudRenderer = null;
    Renderer MountainRenderer = null;

    int currentIndex = 0;
    int nextIndex = 1;

    float CurrentScale = 1;
    float NextScale = 0;

	void Start () {
        Debug.Assert((SpaceLayer != null && CloudLayer != null && MountainLayer != null) == true);

        SpaceRenderer = SpaceLayer.GetComponent<Renderer>();
        CloudRenderer = CloudLayer.GetComponent<Renderer>();
        MountainRenderer = MountainLayer.GetComponent<Renderer>();

        SpaceRenderer.material.SetTexture("_CurrentTexture",SpaceTextures[currentIndex]); 
        SpaceRenderer.material.SetTexture("_NextTexture",SpaceTextures[nextIndex]); 
        SpaceRenderer.material.SetFloat("CurrentScale",CurrentScale); 
        SpaceRenderer.material.SetFloat("NextIndex",NextScale); 

        CloudRenderer.material.SetTexture("_CurrentTexture",CloudTextures[currentIndex]); 
        CloudRenderer.material.SetTexture("_NextTexture",CloudTextures[nextIndex]); 
        CloudRenderer.material.SetFloat("CurrentScale",CurrentScale); 
        CloudRenderer.material.SetFloat("NextIndex",NextScale); 

        MountainRenderer.material.SetTexture("_CurrentTexture",MountainTextures[currentIndex]); 
        MountainRenderer.material.SetTexture("_NextTexture",MountainTextures[nextIndex]); 
        MountainRenderer.material.SetFloat("CurrentScale",CurrentScale); 
        MountainRenderer.material.SetFloat("NextIndex",NextScale); 

	}

    float Timer = 0;
    float rot = 0;
    [SerializeField]
    float RotationRate = 3f;

    float spacerot = 0f;

    [SerializeField]
    float SpaceRate = 3f;

    float mountainrot = 0;
	    [SerializeField]
    float MountainRate = 3f;// Update is called once per frame
	void Update () {

        rot += RotationRate * Time.deltaTime;
        mountainrot += MountainRate * Time.deltaTime;
        spacerot += SpaceRate * Time.deltaTime;
        SpaceRenderer.material.SetTexture("_CurrentTexture",SpaceTextures[currentIndex]); 
        SpaceRenderer.material.SetTexture("_NextTexture",SpaceTextures[nextIndex]); 
        SpaceRenderer.material.SetFloat("CurrentScale",CurrentScale); 
        SpaceRenderer.material.SetFloat("NextScale",NextScale); 
        SpaceRenderer.material.SetFloat("_Rotation",spacerot); 

        CloudRenderer.material.SetTexture("_CurrentTexture",CloudTextures[currentIndex]); 
        CloudRenderer.material.SetTexture("_NextTexture",CloudTextures[nextIndex]); 
        CloudRenderer.material.SetFloat("CurrentScale",CurrentScale); 
        CloudRenderer.material.SetFloat("NextScale",NextScale); 
        CloudRenderer.material.SetFloat("_Rotation",rot); 

        MountainRenderer.material.SetTexture("_CurrentTexture",MountainTextures[currentIndex]); 
        MountainRenderer.material.SetTexture("_NextTexture",MountainTextures[nextIndex]); 
        MountainRenderer.material.SetFloat("CurrentScale",CurrentScale); 
        MountainRenderer.material.SetFloat("NextScale",NextScale); 
        MountainRenderer.material.SetFloat("_Rotation",mountainrot); 

        Timer += Time.deltaTime;

        float cur = Timer/ InterpolationTime;
        if (cur > 1) cur = 1;
        NextScale = cur;

        CurrentScale = 1 - NextScale;
        if (Timer > ChangeTime)
        {
            Timer = 0;
            currentIndex = currentIndex == 3 ? 0 : currentIndex + 1;
            nextIndex = nextIndex == 3 ? 0 : nextIndex + 1;
            CurrentScale = 1f;
            NextScale = 0f;
        }

    //public static float Lerp(float a, float b, float t);
	}
}
