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

public enum ControllerColor
{
    Red,
    Pink,
    Blue,
    Gold
}

public enum ControllerModes
{
    Movement,
    Pickup,
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
    public MovementScript movementScript;
    public PickupScript pickupScript;
    public ControllerModes controllerMode;
    public ControllerColor controllerColor;
    public ControllerButtons swapModeButton;
    public ControllerButtons interactButton;
    public ControllerButtons resetPosButton;
    public List<PlayerStick> playerSticks;
    Dictionary<ControllerColor, Material> dicPlayerSticks;

    //private
    private RaycastHit hit;
    private Vector3 startingPos;
    private int controllerIndex = 0;

    //button checks
    private bool buttonSwapModeDown = false;
    private bool buttonInteractDown = false;
    private bool buttonResetPosDown = false;


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
        if (isSecondaryMoveController)
        {
            controllerColor = playerSticks[0].color;
            UpdateControllerMaterial(controllerColor);
        }

        //setting conttroller mode
        if(controllerMode == ControllerModes.Movement)
        {
            laserPointer.gameObject.SetActive(true);
            movementScript.enabled = true;
            pickupScript.enabled = false;
        }
        else if(controllerMode == ControllerModes.Pickup)
        {
            laserPointer.gameObject.SetActive(false);
            movementScript.enabled = false;
            pickupScript.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetButtonDown(interactButton) && !buttonInteractDown)
        {
            buttonInteractDown = true;
            //switch color
            controllerColor = playerSticks[1].color;
            UpdateControllerMaterial(controllerColor);

            //interact with UI
            if (laserPointer.LineRaycast().collider && laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>())
            {
                laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>().onClick.Invoke();
            }
            //Movement
            if (controllerMode==ControllerModes.Movement)
            {
                movementScript.SpawnMarker();
            }
            
        }
        else if (!GetButtonDown(interactButton) && buttonInteractDown)
        {
            buttonInteractDown = false;
            //switch color
            controllerColor = playerSticks[0].color;
            UpdateControllerMaterial(controllerColor);

            if (controllerMode == ControllerModes.Movement)
            {
                movementScript.Move();
            }
        }

        //swap modes
        if (GetButtonDown(swapModeButton) && !buttonSwapModeDown)
        {
            buttonSwapModeDown = true;
            ToggleMode();
        }
        else if (PS4Input.MoveGetButtons(0, controllerIndex) == 0 && buttonSwapModeDown)
        {
            buttonSwapModeDown = false;
        }

        //reset pos
        if (GetButtonDown(resetPosButton) && !buttonResetPosDown)
        {
            buttonResetPosDown = true;
            ToggleMode();
        }
        else if (PS4Input.MoveGetButtons(0, controllerIndex) == 0 && buttonResetPosDown)
        {
            buttonResetPosDown = false;
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

    //Move to Location

    // When fired the controller will vibrate for 0.1 seconds
    //    IEnumerator Vibrate()
    //    {
    //#if UNITY_PS4
    //        if (isMoveController)
    //        {
    //            if (isSecondaryMoveController)
    //                PS4Input.MoveSetVibration(0, 1, 128);
    //            else
    //                PS4Input.MoveSetVibration(0, 0, 128);
    //        }
    //        else
    //        {
    //            PS4Input.PadSetVibration(0, 0, 255);
    //        }
    //#endif

    //        yield return new WaitForSeconds(0.1f);

    //#if UNITY_PS4
    //        if (isMoveController)
    //        {
    //            if (isSecondaryMoveController)
    //                PS4Input.MoveSetVibration(0, 1, 0);
    //            else
    //                PS4Input.MoveSetVibration(0, 0, 0);
    //        }
    //        else
    //        {
    //            PS4Input.PadSetVibration(0, 0, 0);
    //        }
    //#endif
    //    }

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
            case ControllerColor.Pink:
                mat.material = dicPlayerSticks[ControllerColor.Pink];
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

    void ToggleMode()
    {
        if (controllerMode == ControllerModes.Pickup)
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
