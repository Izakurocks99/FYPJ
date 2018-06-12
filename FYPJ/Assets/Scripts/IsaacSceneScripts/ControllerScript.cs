using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public enum ControllerButtons
{
    X,
    Circle,
    Square,
    Triangle,
    BackTrigger,
    MiddleButton,
    TouchPad,
    Start,
}

public enum ControllerColor
{
    Red,
    Pink,
    Blue,
    Gold
}

[System.Serializable]
public class PlayerStick
{
    public ControllerColor color;
    public Material material;
}

public class ControllerScript : MonoBehaviour
{
    public bool isSecondaryMoveController = false;
    public GameObject player;
    public GameObject currStick;
    public LaserPointer laserPointer;
    public ControllerButtons resetButton;
    public ControllerColor controllerColor;
    public List<PlayerStick> playerSticks;
    Dictionary<ControllerColor, Material> dicPlayerSticks;

    private RaycastHit hit;
    private Vector3 startingPos;

    //move this into a new script


    // Use this for initialization
    void Start()
    {
        startingPos = player.transform.position;

        //init dic
        dicPlayerSticks = new Dictionary<ControllerColor, Material>();
        foreach (PlayerStick var in playerSticks)
        {
            dicPlayerSticks.Add(var.color, var.material);
        }

        //init controllerColor
        if (isSecondaryMoveController)
        {
            controllerColor = ControllerColor.Red;
            UpdateControllerMaterial(controllerColor);
        }
        else
        {
            controllerColor = ControllerColor.Blue;
            UpdateControllerMaterial(controllerColor);
        }
    }

    // Update is called once per frame
    void Update() {
        if (CheckForInput())
        {
            //switch color
            if (isSecondaryMoveController)
            {
                controllerColor = ControllerColor.Pink;
                UpdateControllerMaterial(controllerColor);
            }
            else
            {
                controllerColor = ControllerColor.Gold;
                UpdateControllerMaterial(controllerColor);
            }

            //interact with UI
            if (laserPointer.LineRaycast().collider && laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>())
            {
                laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }
        else if (!CheckForInput())
        {
            //switch color
            if (isSecondaryMoveController)
            {
                controllerColor = ControllerColor.Red;
                UpdateControllerMaterial(controllerColor);
            }
            else
            {
                controllerColor = ControllerColor.Blue;
                UpdateControllerMaterial(controllerColor);
            }
        }

        //reset player position
        if (Input.GetKeyDown(GetControllerKey(resetButton)))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    KeyCode GetControllerKey(ControllerButtons button)
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
                return KeyCode.JoystickButton0;
            case ControllerButtons.Circle:
                return KeyCode.JoystickButton1;
            case ControllerButtons.Square:
                return KeyCode.JoystickButton2;
            case ControllerButtons.Triangle:
                return KeyCode.JoystickButton3;
            case ControllerButtons.BackTrigger:
                return KeyCode.JoystickButton4;
            case ControllerButtons.MiddleButton:
                return KeyCode.JoystickButton5;
            case ControllerButtons.TouchPad:
                return KeyCode.JoystickButton6;
            case ControllerButtons.Start:
                return KeyCode.JoystickButton7;
            default:
                return KeyCode.None;
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

    public void VibrateController()
    {
        if(isSecondaryMoveController)
        {
            StartCoroutine(currStick.GetComponent<VibrationScript>().VibrateLeft());
        }
        else
        {
            StartCoroutine(currStick.GetComponent<VibrationScript>().VibrateRight());
        }
    }
}
