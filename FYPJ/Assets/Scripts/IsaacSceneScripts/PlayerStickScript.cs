﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StickType
{
    Baton,
    //Staff,
    //Stick,
    Wakizashi,
    Candy,
    Bat,
    MAX,
    //Fish
}

[System.Serializable]
public class StickColor
{
    public GameColors color;
    public List<Material> material;
}

[System.Serializable]
public class StickMesh
{
    public StickType type;
    public Mesh mesh;

    public List<StickColor> stickColors;
    public Dictionary<GameColors, List<Material>> dicStickColors;
}

[System.Serializable]
public class TrailColor
{
    public GameColors gamecolor;
    public Color[] color;
}

[ExecuteInEditMode]
public class PlayerStickScript : MonoBehaviour
{
    public bool RNGSticks;

    public ControllerScript heldController;
    public StickType objectMesh;
    public GameColors currColor;
    //public List<PlayerStick> playerSticks;
    //Dictionary<ControllerColor, Material> dicPlayerSticks;
    public List<StickMesh> stickMeshes;
    Dictionary<GameColors, Color[]> dicTrail;

    public List<TrailColor> trailList;
    Dictionary<StickType, StickMesh> dicSticks;

    public List<BatonCapsuleFollower> BatonFollowers;

    Vector3 startingPos;
    Vector3 startingScale;

    [SerializeField]
    Transform stickRack = null;

    // Use this for initialization
    void Start()
    {
        if(RNGSticks)
        {
            objectMesh = (StickType)Random.Range(0, (int)StickType.MAX);
        }

        dicSticks= new Dictionary<StickType, StickMesh>();
        foreach (StickMesh var in stickMeshes)
        {
            dicSticks.Add(var.type,var);

            var.dicStickColors = new Dictionary<GameColors, List<Material>>();
            foreach (StickColor color in var.stickColors)
                var.dicStickColors.Add(color.color, color.material);
        }

        dicTrail = new Dictionary<GameColors, Color[]>();
        foreach (TrailColor var in trailList)
        {
            dicTrail.Add(var.gamecolor, var.color);
        }

        startingPos = gameObject.transform.parent.position;
        startingScale = gameObject.transform.parent.localScale;

        GetComponent<MeshFilter>().mesh = dicSticks[objectMesh].mesh;
        ChangeStickColor(currColor);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitMesh(int index)
    {
        objectMesh = (StickType)index;
    }

    public void ChangeStickColor(GameColors newColor)
    {
        //update var
        currColor = newColor;
        Renderer mat = gameObject.GetComponent<Renderer>();
        //swap mats
        mat.materials = dicSticks[objectMesh].dicStickColors[newColor].ToArray();

        gameObject.GetComponent<MeleeWeaponTrail>()._colors = dicTrail[currColor];
    }

    public bool Equip()
    {
        if (gameObject.transform.parent.parent)
        {
            heldController = GetComponentInParent<ControllerScript>();

            if (heldController.isSecondaryMoveController)
            {
                PlayerPrefs.SetInt("secstick", (int)objectMesh);
            }
            else if (!heldController.isSecondaryMoveController)
            {
                PlayerPrefs.SetInt("pristick", (int)objectMesh);
            }

            foreach (BatonCapsuleFollower follower in BatonFollowers)
            {
                follower.gameObject.SetActive(true);
            }

            GetComponent<Collider>().enabled = false;
            return true;
        }
        return false;
    }

    public void Drop()
    {
        heldController = null;
    }

    public void SwapStick()
    {
        objectMesh = (StickType)((int)objectMesh + 1);
        if(objectMesh == StickType.MAX)
        {
            objectMesh = StickType.Baton;
        }
        
        GetComponent<MeshFilter>().mesh = dicSticks[objectMesh].mesh;
        ChangeStickColor(currColor);
    }

    public void Return()
    {
        gameObject.transform.parent.SetParent(stickRack);
        gameObject.transform.parent.position = startingPos;
        gameObject.transform.parent.rotation= Quaternion.identity;
        gameObject.transform.parent.localScale = startingScale;
        GetComponent<Collider>().enabled = true;

        foreach (BatonCapsuleFollower follower in BatonFollowers)
        {
            follower.gameObject.SetActive(false);
        }
    }
}
