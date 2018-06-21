using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StickType
{
    Baton,
    Staff,
    Stick,
    //Fish
}

[System.Serializable]
public class StickColor
{
    public ControllerColor color;
    public Material material;
}

[System.Serializable]
public class StickMesh
{
    public StickType type;
    public Mesh mesh;

    public List<StickColor> stickColors;
    public Dictionary<ControllerColor,Material> dicStickColors;
}

[ExecuteInEditMode]
public class PlayerStickScript : MonoBehaviour
{
    public GameObject handle;

    public ControllerScript heldController;
    public StickType objectMesh;
    public ControllerColor currColor;
    //public List<PlayerStick> playerSticks;
    //Dictionary<ControllerColor, Material> dicPlayerSticks;
    public List<StickMesh> stickMeshes;
    Dictionary<StickType, StickMesh> dicSticks;

    // Use this for initialization
    void Start()
    {
        dicSticks= new Dictionary<StickType, StickMesh>();
        foreach (StickMesh var in stickMeshes)
        {
            dicSticks.Add(var.type,var);

            var.dicStickColors = new Dictionary<ControllerColor, Material>();
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

    public void ChangeStickColor(ControllerColor newColor)
    {
        //update var
        currColor = newColor;

        Renderer mat = gameObject.GetComponent<Renderer>();
        //switch (newColor)
        //{
        //    case ControllerColor.Pink:
        //        mat.material = dicPlayerSticks[ControllerColor.Pink];
        //        return;
        //    case ControllerColor.Green:
        //        mat.material = dicPlayerSticks[ControllerColor.Green];
        //        return;
        //    case ControllerColor.Blue:
        //        mat.material = dicPlayerSticks[ControllerColor.Blue];
        //        return;
        //    case ControllerColor.Gold:
        //        mat.material = dicPlayerSticks[ControllerColor.Gold];
        //        return;
        //}
        
        //swap mats
        mat.material = dicSticks[objectMesh].dicStickColors[newColor];
    }

    public void Equip()
    {
        heldController = GetComponentInParent<ControllerScript>();
    }

    void InitModel()
    {

    }
}
