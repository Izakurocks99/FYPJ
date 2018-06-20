using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

//Controls color of stick
//checks for controller input

public enum ControllerColor
{
    Red,
    Green,
    Blue,
    Gold
}

public enum ControllerModes
{
    Movement,
    Pickup,
    Dance,
    Calibrate
}

[System.Serializable]
public class PlayerStick
{
    public ControllerColor color;
    public Material material;
}

public class ControllerScript : MonoBehaviour
{
    //public
    public bool isSecondaryMoveController = false;
    //public GameObject player;
    public GameObject currStick;
    public LaserPointer laserPointer;
    public PlayerControlsScript controlsScript;
    public ControllerModes controllerMode;
    public ControllerColor controllerColor;
    public List<PlayerStick> playerSticks;
    Dictionary<ControllerColor, Material> dicPlayerSticks;

    //mode scripts
    public MovementScript movementScript;
    public PickupScript pickupScript;
    public ControlCalibrationScript calibrateScript;

    //private
    private RaycastHit hit;
    private Vector3 startingPos;
    private int controllerIndex = 0;

    //button checks
    private bool middleButtonDown = false;
    private bool TriggerButtonDown = false;
    private bool xButtonDown = false;
    private bool circleButtonDown = false;
    //private bool buttonSwapModeDown = false;
    //private bool buttonInteractDown = false;
    //private bool buttonResetPosDown = false;
    //private bool buttonResetSceneDown = false;


    // Use this for initialization
    void Start()
    {
        //startingPos = player.transform.position; //add starting position, for resetting of position
        if (isSecondaryMoveController) // init which controller this is
            controllerIndex = 1;

        //init dic to switch controller color
        dicPlayerSticks = new Dictionary<ControllerColor, Material>();
        foreach (PlayerStick var in playerSticks)
        {
            dicPlayerSticks.Add(var.color, var.material);
        }

        //init controllerColor
        if (isSecondaryMoveController && currStick)
        {
            controllerColor = playerSticks[0].color;
            UpdateControllerMaterial(controllerColor);
        }

        //TODO: CHANGE THIS TO USE CONTROLLER MODE BASE CLASS INIT FUNC
        //setting conttroller mode
        if (controllerMode == ControllerModes.Calibrate)
        {
            laserPointer.gameObject.SetActive(false);
            movementScript.enabled = false;
            pickupScript.enabled = true;
            calibrateScript.enabled = true;
        }
        if (controllerMode == ControllerModes.Movement)
        {
            laserPointer.gameObject.SetActive(true);
            movementScript.enabled = true;
            pickupScript.enabled = false;
            calibrateScript.enabled = false;
        }
        else if(controllerMode == ControllerModes.Pickup)
        {
            laserPointer.gameObject.SetActive(false);
            movementScript.enabled = false;
            pickupScript.enabled = true;
            calibrateScript.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Inputs
        if (GetButtonDown(ControllerButtons.BackTrigger) && !TriggerButtonDown)
        {
            TriggerButtonDown = true;
            //switch color
            if (currStick)
            {
                controllerColor = playerSticks[1].color;
                UpdateControllerMaterial(controllerColor);
            }

            //interact with UI
            if (laserPointer.LineRaycast().collider && laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>())
            {
                laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>().onClick.Invoke();
            }

            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.BackTrigger,isSecondaryMoveController);

        }
        else if (!GetButtonDown(ControllerButtons.BackTrigger) && TriggerButtonDown)
        {
            TriggerButtonDown = false;
            //switch color
            if (currStick)
            {
                controllerColor = playerSticks[0].color;
                UpdateControllerMaterial(controllerColor);
            }

            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.BackTrigger, isSecondaryMoveController);
        }

        //swap modes
        if (GetButtonDown(ControllerButtons.MiddleButton) && !middleButtonDown)
        {
            middleButtonDown = true;

            ToggleMode();

            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.MiddleButton, isSecondaryMoveController);
        }
        else if (!GetButtonDown(ControllerButtons.MiddleButton)&& middleButtonDown)
        {
            middleButtonDown = false;

            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.MiddleButton, isSecondaryMoveController);
        }

        //reset pos
        if (GetButtonDown(ControllerButtons.X) && !xButtonDown)
        {
            xButtonDown = true;

            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.X, isSecondaryMoveController);

        }
        else if (!GetButtonDown(ControllerButtons.X) && xButtonDown)
        {
            xButtonDown = false;

            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.X, isSecondaryMoveController);
        }

        if (GetButtonDown(ControllerButtons.Circle) && !circleButtonDown)
        {
            circleButtonDown = true;

            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.Circle, isSecondaryMoveController);
        }
        else if (!GetButtonDown(ControllerButtons.Circle) && circleButtonDown)
        {
            circleButtonDown = false;

            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.Circle, isSecondaryMoveController);
        }
    }

    // Move controlelrs use an API for their analog button, DualShock 4 uses an axis name for R2
    bool CheckForInput()
    {
#if UNITY_PS4

        if (!isSecondaryMoveController)
        {
            return (PS4Input.MoveGetAnalogButton(0, 0) > 0 ? true : false);
        }
        else
        {
            return (PS4Input.MoveGetAnalogButton(0, 1) > 0 ? true : false);
        }
#else
    		return Input.GetButton("Fire1");
#endif
    }

    int GetButtonIndex(ControllerButtons button)
    {
        //FOR INPUT CONTROLLER 
        // * x                -0 
        // * Circle           -1 
        // * Square           -2
        // * Triangle         -3
        // * BackTrigger      -4
        // * MiddleButton     -5
        // * TouchPad         -6
        // * Start         -7
        switch (button)
        {
            case ControllerButtons.X:
                return 64;
            case ControllerButtons.Circle:
                return 32;
            case ControllerButtons.Square:
                return 128;
            case ControllerButtons.Triangle:
                return 16;
            case ControllerButtons.BackTrigger:
                return 2;
            case ControllerButtons.MiddleButton:
                return 4;
            case ControllerButtons.Start:
                return 8;
            default:
                return 0;
        }
    }

    void UpdateControllerMaterial(ControllerColor newColor)
    {
        Renderer mat = currStick.GetComponent<Renderer>();
        switch (newColor)
        {
            case ControllerColor.Red:
                mat.material = dicPlayerSticks[ControllerColor.Red];
                return;
            case ControllerColor.Green:
                mat.material = dicPlayerSticks[ControllerColor.Green];
                return;
            case ControllerColor.Blue:
                mat.material = dicPlayerSticks[ControllerColor.Blue];
                return;
            case ControllerColor.Gold:
                mat.material = dicPlayerSticks[ControllerColor.Gold];
                return;

        }
    }

    bool GetButtonDown(ControllerButtons button)
    {
        if (button != ControllerButtons.BackTrigger)
            return PS4Input.MoveGetButtons(0, controllerIndex) == (GetButtonIndex(button));
        else
            return CheckForInput();
    }

    ControllerModesScript GetControllerMode(ControllerModes currMode)
    {
        switch (currMode)
        {
            case ControllerModes.Movement:
                return movementScript;

            case ControllerModes.Pickup:
                return pickupScript;

            case ControllerModes.Calibrate:
                return calibrateScript;

            default:
                return null;
        }
    }

    void ToggleMode()
    {
        if(controllerMode == ControllerModes.Calibrate)
        {
            controllerMode = ControllerModes.Movement;
            pickupScript.enabled = false;
        }
        else if (controllerMode == ControllerModes.Pickup)
        {
            controllerMode = ControllerModes.Movement;
        }
        else if (controllerMode == ControllerModes.Movement)
        {
            controllerMode = ControllerModes.Pickup;
        }
        laserPointer.gameObject.SetActive(!laserPointer.gameObject.activeInHierarchy);
        movementScript.enabled = !movementScript.enabled;
        pickupScript.enabled = !pickupScript.enabled;
    }

    public void VibrateController()
    {
        if (isSecondaryMoveController)
        {
            StartCoroutine(currStick.GetComponent<VibrationScript>().VibrateLeft());
        }
        else
        {
            StartCoroutine(currStick.GetComponent<VibrationScript>().VibrateRight());
        }
    }
    
}
