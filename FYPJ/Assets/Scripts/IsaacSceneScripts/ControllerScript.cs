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

public enum ControllerModes
{
    NONE,
    Movement,
    Calibrate
}

public enum GameColors
{
    Pink,
    Green,
    Blue,
    Gold,
    Rainbow,
}

public class ControllerScript : MonoBehaviour
{
    //public
    public bool isSecondaryMoveController = false;
    public PlayerStickScript currStick;
    public LaserPointer laserPointer;
    public PlayerControlsScript controlsScript;
    public Transform stickSlot; //the transform of where the equipped stick will be after picking it up
    public ControllerModes controllerMode;
    public GameColors pirmaryControllerColor;
    public GameColors secondaryControllerColor;

    //mode scripts
    public MovementScript movementScript;
    public ControlCalibrationScript calibrateScript;

    //private
    private GameObject highlightedStick; //indicate which stick is selected/where to grab the stick to equip
    private RaycastHit hit;
    private Vector3 startingPos;
    private bool canPickup = true; //check if hand is alread grabbing a stick
    private int controllerIndex = 0;

    //button checks
    private bool middleButtonDown = false;
    private bool TriggerButtonDown = false;
    private bool xButtonDown = false;
    private bool circleButtonDown = false;
    private bool triangleButtonDown = false;
    private bool squareButtonDown = false;

    private bool swapModeButtonDown = false;
    private bool togglePointerButtonDown = false;

    // Use this for initialization
    void Start()
    {
        if (isSecondaryMoveController) // init which controller this is
            controllerIndex = 1;

    }

    // Update is called once per frame
    void Update()
    {
        //Inputs Single Controllers
        //back trigger
        if (GetButtonDown(ControllerButtons.BackTrigger) && !TriggerButtonDown)
        {
            TriggerButtonDown = true;
            ButtonPressed(ControllerButtons.BackTrigger);
            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.BackTrigger);

        }
        else if (!GetButtonDown(ControllerButtons.BackTrigger) && TriggerButtonDown)
        {
            TriggerButtonDown = false;
            ButtonReleased(ControllerButtons.BackTrigger);
            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.BackTrigger);
        }

        //middle button
        if (GetButtonDown(ControllerButtons.MiddleButton) && !middleButtonDown)
        {
            middleButtonDown = true;
            ButtonPressed(ControllerButtons.MiddleButton);
            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.MiddleButton);
        }
        else if (!GetButtonDown(ControllerButtons.MiddleButton) && middleButtonDown)
        {
            middleButtonDown = false;
            ButtonReleased(ControllerButtons.MiddleButton);
            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.MiddleButton);
        }

        // X button
        if (GetButtonDown(ControllerButtons.X) && !xButtonDown)
        {
            xButtonDown = true;
            ButtonPressed(ControllerButtons.X);
            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.X);

        }
        else if (!GetButtonDown(ControllerButtons.X) && xButtonDown)
        {
            xButtonDown = false;
            ButtonReleased(ControllerButtons.X);
            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.X);
        }

        //Circle button
        if (GetButtonDown(ControllerButtons.Circle) && !circleButtonDown)
        {
            circleButtonDown = true;
            ButtonPressed(ControllerButtons.Circle);
            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.Circle);
        }
        else if (!GetButtonDown(ControllerButtons.Circle) && circleButtonDown)
        {
            circleButtonDown = false;
            ButtonReleased(ControllerButtons.Circle);
            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.Circle);
        }

        //Triangle button
        if (GetButtonDown(ControllerButtons.Triangle) && !triangleButtonDown)
        {
            triangleButtonDown = true;
            ButtonPressed(ControllerButtons.Triangle);
            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.Triangle);
        }
        else if (!GetButtonDown(ControllerButtons.Triangle) && triangleButtonDown)
        {
            triangleButtonDown = false;
            ButtonReleased(ControllerButtons.Triangle);
            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.Triangle);
        }

        //Square button
        if (GetButtonDown(ControllerButtons.Square) && !squareButtonDown)
        {
            squareButtonDown = true;
            ButtonPressed(ControllerButtons.Square);
            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.Square);
        }
        else if (!GetButtonDown(ControllerButtons.Square) && squareButtonDown)
        {
            squareButtonDown = false;
            ButtonReleased(ControllerButtons.Square);
            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.Square);
        }

        //BothControllers
        //swapmode
        if (GetKeyDown(controlsScript.swapModeButton) && !swapModeButtonDown)
        {
            Debug.Log("SwitchMode");
            swapModeButtonDown = true;
            if (controllerMode == ControllerModes.Movement)
            {
                //GetComponentInParent<ModeManagerScript>().ChangeMode(ControllerModes.Calibrate);

                movementScript.Exit();
                movementScript.enabled = false;
                calibrateScript.enabled = true;
                calibrateScript.Init();
                controllerMode = ControllerModes.Calibrate;
            }
            else
            {
                calibrateScript.Exit();
                calibrateScript.enabled = false;
                movementScript.enabled = true;
                movementScript.Init();
                controllerMode = ControllerModes.Movement;
            }
        }
        else if (!GetKeyDown(controlsScript.swapModeButton) && swapModeButtonDown)
        {
            swapModeButtonDown = false;
        }

        //toggle laser
        if (GetKeyDown(controlsScript.togglePointerButton) && !togglePointerButtonDown)
        {
            togglePointerButtonDown = true;
            laserPointer.gameObject.SetActive(!laserPointer.gameObject.activeInHierarchy);
        }
        else if (!GetKeyDown(controlsScript.togglePointerButton) && togglePointerButtonDown)
        {
            togglePointerButtonDown = false;
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

    bool GetKeyDown(ControllerButtons button)
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
                return Input.GetKeyDown(KeyCode.JoystickButton0);
            case ControllerButtons.Circle:
                return Input.GetKeyDown(KeyCode.JoystickButton1);
            case ControllerButtons.Square:
                return Input.GetKeyDown(KeyCode.JoystickButton2);
            case ControllerButtons.Triangle:
                return Input.GetKeyDown(KeyCode.JoystickButton3);
            case ControllerButtons.BackTrigger:
                return Input.GetKeyDown(KeyCode.JoystickButton4);
            case ControllerButtons.MiddleButton:
                return Input.GetKeyDown(KeyCode.JoystickButton5);
            case ControllerButtons.Start:
                return Input.GetKeyDown(KeyCode.JoystickButton7);
            default:
                return false;
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

            case ControllerModes.Calibrate:
                return calibrateScript;

            default:
                return null;
        }
    }

    public void VibrateController()
    {
        if (isSecondaryMoveController)
        {
            StartCoroutine(gameObject.GetComponent<VibrationScript>().VibrateLeft());
        }
        else
        {
            StartCoroutine(gameObject.GetComponent<VibrationScript>().VibrateRight());
        }
    }

    void ButtonPressed(ControllerButtons button)
    {
        if (button == controlsScript.interactButton)
        {
            //switch color
            PickUpStick();
            if (currStick)
            {
                currStick.ChangeStickColor(secondaryControllerColor);
            }
        }

        if(button == controlsScript.dropStickButton)
        {
            DropStick();
        }

        if (button == controlsScript.resetSceneButton)
        {
            ResetScene();
        }

        if(button == controlsScript.clickButton)
        {
            //interact with UI 
            if (laserPointer && laserPointer.LineRaycast().collider && laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>())
            {
                laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    void ButtonReleased(ControllerButtons button)
    {
        if (button == controlsScript.interactButton)
        {
            //switch color
            if (currStick)
            {
                currStick.ChangeStickColor(pirmaryControllerColor);
            }
        }
    }

    bool PickUpStick()
    {
        if (highlightedStick && canPickup)//if overlapping a stick and is not holding
        {
            //put stick in the hand's stick slot
            highlightedStick.transform.SetParent(stickSlot);
            highlightedStick.transform.position = stickSlot.position;
            highlightedStick.transform.rotation = stickSlot.rotation;
            //set controller currstick
            currStick = highlightedStick.GetComponentInChildren<PlayerStickScript>();
            currStick.Equip();

            //set player unable to pickup
            canPickup = false;
            return true;
        }
        return false;
    }

    void DropStick()
    {
        //if holding a stick
        if (currStick)
        {
            //drop
            currStick.Drop();
            //remove from parent
            currStick.gameObject.transform.parent.parent = null;
            //set currstick to null
            currStick = null;
            //allow the player to pick up again
            canPickup = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if other entered a sicks
        if (other.gameObject.tag == "PlayerStick" && !other.transform.parent)
        {
            //set current stick as "highlighted"
            highlightedStick = other.gameObject;
        }
        if (other.gameObject.GetComponent<PlayerStickScript>() && !other.transform.parent.parent)
        {
            PlayerStickScript stickScript = other.gameObject.GetComponent<PlayerStickScript>();
            stickScript.handle.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if other entered a sicks
        if (other.gameObject.tag == "PlayerStick" && !other.transform.parent)
        {
            //set current stick as "highlighted"
            highlightedStick = null;
        }
        if (other.gameObject.GetComponent<PlayerStickScript>() && !other.transform.parent.parent)
        {
            PlayerStickScript stickScript = other.gameObject.GetComponent<PlayerStickScript>();
            stickScript.handle.SetActive(false);
        }
    }

    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
