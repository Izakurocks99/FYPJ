using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerButtons
{
    NONE,
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
    public ControllerButtons clickButton;
    public ControllerButtons interactButton;
    public ControllerButtons resetSceneButton;
    public ControllerButtons dropStickButton;
    public ControllerButtons togglePointerButton;
    public ControllerButtons calibrateButton;
}
