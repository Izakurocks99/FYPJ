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

public enum ControllerColor
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
    public ControllerModes controllerMode;
    public ControllerColor pirmaryControllerColor;
    public ControllerColor secondaryControllerColor;
    
    //mode scripts
    public MovementScript movementScript;
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
    private bool triangleButtonDown = false;
    private bool squareButtonDown = false;

    private bool swapModeButtonDown = false;

    // Use this for initialization
    void Start()
    {
        if (isSecondaryMoveController) // init which controller this is
            controllerIndex = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Inputs
        //back trigger
        if (GetButtonDown(ControllerButtons.BackTrigger) && !TriggerButtonDown)
        {
            TriggerButtonDown = true;
            //switch color
            if (currStick)
            {
                currStick.ChangeStickColor(secondaryControllerColor);
            }

            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.BackTrigger);

        }
        else if (!GetButtonDown(ControllerButtons.BackTrigger) && TriggerButtonDown)
        {
            TriggerButtonDown = false;
            //switch color
            if (currStick)
            {
                currStick.ChangeStickColor(pirmaryControllerColor);
            }

            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.BackTrigger);
        }

        //middle button
        if (GetButtonDown(ControllerButtons.MiddleButton) && !middleButtonDown)
        {
            middleButtonDown = true;

            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.MiddleButton);
        }
        else if (!GetButtonDown(ControllerButtons.MiddleButton)&& middleButtonDown)
        {
            middleButtonDown = false;
            //interact with UI 
            if (laserPointer && laserPointer.LineRaycast().collider && laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>())
            {
                laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>().onClick.Invoke();
            }

            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.MiddleButton);
        }

        // X button
        if (GetButtonDown(ControllerButtons.X) && !xButtonDown)
        {
            xButtonDown = true;

            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.X);

        }
        else if (!GetButtonDown(ControllerButtons.X) && xButtonDown)
        {
            xButtonDown = false;

            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.X);
        }

        //Circle button
        if (GetButtonDown(ControllerButtons.Circle) && !circleButtonDown)
        {
            circleButtonDown = true;

            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.Circle);
        }
        else if (!GetButtonDown(ControllerButtons.Circle) && circleButtonDown)
        {
            circleButtonDown = false;

            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.Circle);
        }

        //Triangle button
        if (GetButtonDown(ControllerButtons.Triangle) && !triangleButtonDown)
        {
            triangleButtonDown = true;

            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.Triangle);
        }
        else if (!GetButtonDown(ControllerButtons.Triangle) && triangleButtonDown)
        {
            triangleButtonDown = false;

            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.Triangle);
        }

        //Square button
        if (GetButtonDown(ControllerButtons.Square) && !squareButtonDown)
        {
            squareButtonDown = true;

            GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.Square);
        }
        else if (!GetButtonDown(ControllerButtons.Square) && squareButtonDown)
        {
            squareButtonDown = false;

            GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.Square);
        }

        //Test
        //swapmode
        if (GetKeyDown(controlsScript.swapModeButton)&& !swapModeButtonDown)
        {
            Debug.Log("SwitchMode");
            swapModeButtonDown = true;
            if(controllerMode == ControllerModes.Movement)
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
        else if (!GetButtonDown(controlsScript.swapModeButton) && swapModeButtonDown)
        {
            swapModeButtonDown = false;
            
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
    
}
