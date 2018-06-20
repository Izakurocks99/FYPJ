using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerModesScript : MonoBehaviour
{
    public PlayerControlsScript controlsScript;

    // Use this for initialization
    void Start()
    {
        controlsScript = FindObjectOfType<PlayerControlsScript>();
    }

    public virtual void Init() //when this mode is entered
    {

    }

    public virtual void Exit() //when this mode is exited
    {

    }

    public virtual void ButtonPressed(ControllerButtons button)
    {

    }

    public virtual void ButtonReleased(ControllerButtons button)
    {

    }
}
