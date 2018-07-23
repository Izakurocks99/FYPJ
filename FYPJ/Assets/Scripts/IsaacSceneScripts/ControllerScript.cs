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
    Black,
    NONE,
}

public class ControllerScript : MonoBehaviour
{
    //public
    public bool spawnSticks = false;
    public bool isSecondaryMoveController = false;
    public GameObject stickPrefab;
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
    private GameObject highlightedObject; //indicate which stick is selected/where to grab the stick to equip
    private GameObject heldObject;
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

        if (spawnSticks)
        {
            SpawnStick();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (currStick && !currStick.Equip())
        //{
        //        currStick.transform.parent.SetParent(stickSlot);
        //        currStick.transform.parent.localPosition = Vector3.zero;
        //        currStick.transform.localRotation = Quaternion.identity;
        //        currStick.transform.localScale = Vector3.one;

        //        currStick.Equip();
        //        currStick.ChangeStickColor(pirmaryControllerColor);
        //}
        //Inputs Single Controllers
        //back trigger
        if (GetButtonDown(ControllerButtons.BackTrigger) && !TriggerButtonDown)
        {
            TriggerButtonDown = true;
            ButtonPressed(ControllerButtons.BackTrigger);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.BackTrigger);

        }
        else if (!GetButtonDown(ControllerButtons.BackTrigger) && TriggerButtonDown)
        {
            TriggerButtonDown = false;
            ButtonReleased(ControllerButtons.BackTrigger);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.BackTrigger);
        }

        //middle button
        if (GetButtonDown(ControllerButtons.MiddleButton) && !middleButtonDown)
        {
            middleButtonDown = true;
            ButtonPressed(ControllerButtons.MiddleButton);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.MiddleButton);
        }
        else if (!GetButtonDown(ControllerButtons.MiddleButton) && middleButtonDown)
        {
            middleButtonDown = false;
            ButtonReleased(ControllerButtons.MiddleButton);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.MiddleButton);
        }

        // X button
        if (GetButtonDown(ControllerButtons.X) && !xButtonDown)
        {
            xButtonDown = true;
            ButtonPressed(ControllerButtons.X);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.X);

        }
        else if (!GetButtonDown(ControllerButtons.X) && xButtonDown)
        {
            xButtonDown = false;
            ButtonReleased(ControllerButtons.X);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.X);
        }

        //Circle button
        if (GetButtonDown(ControllerButtons.Circle) && !circleButtonDown)
        {
            circleButtonDown = true;
            ButtonPressed(ControllerButtons.Circle);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.Circle);
        }
        else if (!GetButtonDown(ControllerButtons.Circle) && circleButtonDown)
        {
            circleButtonDown = false;
            ButtonReleased(ControllerButtons.Circle);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.Circle);
        }

        //Triangle button
        if (GetButtonDown(ControllerButtons.Triangle) && !triangleButtonDown)
        {
            triangleButtonDown = true;
            ButtonPressed(ControllerButtons.Triangle);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.Triangle);
        }
        else if (!GetButtonDown(ControllerButtons.Triangle) && triangleButtonDown)
        {
            triangleButtonDown = false;
            ButtonReleased(ControllerButtons.Triangle);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonReleased(ControllerButtons.Triangle);
        }

        //Square button
        if (GetButtonDown(ControllerButtons.Square) && !squareButtonDown)
        {
            squareButtonDown = true;
            ButtonPressed(ControllerButtons.Square);
            if (controllerMode != ControllerModes.NONE)
                GetControllerMode(controllerMode).ButtonPressed(ControllerButtons.Square);
        }
        else if (!GetButtonDown(ControllerButtons.Square) && squareButtonDown)
        {
            squareButtonDown = false;
            ButtonReleased(ControllerButtons.Square);
            if (controllerMode != ControllerModes.NONE)
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
#if UNITY_PS4
			return PS4Input.MoveGetButtons(0, controllerIndex) == (GetButtonIndex(button));
#else
			return false;
#endif
		else
			return CheckForInput();
        //return false;
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
            //PickUpStick();
            if (highlightedObject)
            {
                Debug.Log(highlightedObject.name + "Held");
                heldObject = highlightedObject;
                heldObject.GetComponent<CDscript>().Hold(this.transform);
            }

            if (currStick)
            {
                currStick.ChangeStickColor(secondaryControllerColor);
            }
        }

        if (button == controlsScript.dropStickButton)
        {
            DropStick();
        }

        if (button == controlsScript.resetSceneButton)
        {
            ResetScene();
        }

        if (button == controlsScript.swapSticksButton)
        {
            currStick.SwapStick();
        }

        if (button == controlsScript.resetScoreButton)
        {
            transform.root.GetComponentInChildren<PlayerStats>()._intPlayerScoring = 0;
        }

        if (button == controlsScript.clickButton)
        {
            //interact with UI 
            PickUpStick();
            if (laserPointer && laserPointer.LineRaycast() && laserPointer.LineRaycast().gameObject.GetComponent<Button>())
            {
                laserPointer.LineRaycast().gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    void ButtonReleased(ControllerButtons button)
    {
        if (heldObject)
        {
            Debug.Log(heldObject.name + "Released");
            heldObject.GetComponent<CDscript>().Release();
            heldObject = null;
        }
        if (button == controlsScript.interactButton)
        {
            //switch color
            if (currStick)
            {
                currStick.ChangeStickColor(pirmaryControllerColor);
            }
        }
    }

    public void SpawnStick()
    {
        GameObject go = Instantiate(stickPrefab);
        if (isSecondaryMoveController)
            go.GetComponentInChildren<PlayerStickScript>().InitMesh(PlayerPrefs.GetInt("secstick"));
        else if (!isSecondaryMoveController)
            go.GetComponentInChildren<PlayerStickScript>().InitMesh(PlayerPrefs.GetInt("pristick"));

        go.transform.SetParent(stickSlot);
        go.transform.position = stickSlot.position;
        go.transform.rotation = stickSlot.rotation;
        go.transform.localScale = stickSlot.localScale;
        currStick = go.GetComponentInChildren<PlayerStickScript>();
        currStick.Equip();

        //set player unable to pickup
        canPickup = false;
    }

    bool PickUpStick()
    {
        Collider hit = laserPointer.LineRaycast();
        if (hit != null)
        {
            if (hit.gameObject.GetComponent<PlayerStickScript>())
            {
                PlayerStickScript stick = hit.gameObject.GetComponent<PlayerStickScript>();
                if (hit.gameObject.GetComponent<PlayerStickScript>() && canPickup)//if overlapping a stick and is not holding
                {
                    //put stick in the hand's stick slot
                    hit.gameObject.transform.parent.SetParent(stickSlot);
                    hit.gameObject.transform.parent.position = stickSlot.position;
                    hit.gameObject.transform.parent.rotation = stickSlot.rotation;
                    hit.gameObject.transform.parent.localScale = stickSlot.localScale;
                    //set controller currstick
                    currStick = hit.gameObject.GetComponent<PlayerStickScript>();
                    currStick.Equip();

                    //set player unable to pickup
                    canPickup = false;
                    return true;
                }
            }
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
        if (other.gameObject.GetComponent<CDscript>())
        {
            //Debug.Log(other.gameObject.name + "Entered");
            //set current stick as "highlighted"
            highlightedObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //add if trigger is still pressed dont enter
        //if other entered a sicks
        if (other.gameObject.GetComponent<CDscript>())
        {
            //Debug.Log(other.gameObject.name + "Exited");
            //set current stick as "highlighted"
            highlightedObject = null;
        }
    }

    void ResetScene()
    {
        transform.root.GetComponentInChildren<SceneSwitch>().LoadScene();
    }
}