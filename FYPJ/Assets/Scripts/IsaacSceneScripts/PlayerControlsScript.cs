using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerButtons
{
    X,
    Circle,
    Square,
    Triangle,
    BackTrigger,
    MiddleButton,
    Start,
}

public class PlayerControlsScript : MonoBehaviour
{
    public ControllerButtons swapModeButton;
    public ControllerButtons interactButton;
    public ControllerButtons resetPosButton;
    public ControllerButtons resetSceneButton;
}
