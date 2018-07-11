using System.Collections;
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

[ExecuteInEditMode]
public class PlayerStickScript : MonoBehaviour
{
    public GameObject handle;
    public bool RNGSticks;

    public ControllerScript heldController;
    public StickType objectMesh;
    public GameColors currColor;
    //public List<PlayerStick> playerSticks;
    //Dictionary<ControllerColor, Material> dicPlayerSticks;
    public List<StickMesh> stickMeshes;
    Dictionary<StickType, StickMesh> dicSticks;

    public List<BatonCapsuleFollower> BatonFollowers;

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

        GetComponent<MeshFilter>().mesh = dicSticks[objectMesh].mesh;
        ChangeStickColor(currColor);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeStickColor(GameColors newColor)
    {
        //update var
        currColor = newColor;

        Renderer mat = gameObject.GetComponent<Renderer>();
        
        //swap mats

        mat.materials = dicSticks[objectMesh].dicStickColors[newColor].ToArray();
    }

    public void Equip()
    {
        heldController = GetComponentInParent<ControllerScript>();
        Debug.Log(heldController.gameObject.name);
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

}
