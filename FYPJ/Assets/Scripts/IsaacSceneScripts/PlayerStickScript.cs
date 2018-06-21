using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StickType
{
    Baton,
    Staff,
    Stick,
    Fish
}

[System.Serializable]
public class PlayerStick
{
    public ControllerColor color;
    public Material material;
}

[ExecuteInEditMode]
public class PlayerStickScript : MonoBehaviour
{
    public GameObject handle;

    public Mesh objectMesh;
    public ControllerScript heldController;
    public ControllerColor currColor;
    public List<PlayerStick> playerSticks; //needs to be moved into sticks script
    Dictionary<ControllerColor, Material> dicPlayerSticks;

    // Use this for initialization
    void Start()
    {
        dicPlayerSticks = new Dictionary<ControllerColor, Material>();
        foreach (PlayerStick var in playerSticks)
        {
            dicPlayerSticks.Add(var.color, var.material);
        }

        GetComponent<MeshFilter>().mesh = objectMesh;
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
        mat.material = dicPlayerSticks[newColor];
    }

    public void Equip()
    {
        heldController = GetComponentInParent<ControllerScript>();
    }

    void InitModel()
    {

    }
}
