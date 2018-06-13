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
    public ControllerColor controllerColor;
    public ControllerButtons swapModeButton;
    public ControllerButtons interactButton;
    public List<PlayerStick> playerSticks;
    Dictionary<ControllerColor, Material> dicPlayerSticks;

    private RaycastHit hit;
    private Vector3 startingPos;
    private int controllerIndex = 0;

    //button checks
    private bool isMiddleButtonDown = false;


    // Use this for initialization
    void Start()
    {
        startingPos = player.transform.position;
        if (isSecondaryMoveController)
            controllerIndex = 1;

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

        //swap modes
        if (PS4Input.MoveGetButtons(0,controllerIndex) == (GetButtonIndex(swapModeButton)) && !isMiddleButtonDown)
        {
            laserPointer.gameObject.SetActive(!laserPointer.gameObject.activeInHierarchy);
            isMiddleButtonDown = true;
        }
        else if(PS4Input.MoveGetButtons(0, controllerIndex) == 0 && isMiddleButtonDown)
        {
            isMiddleButtonDown = false;
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
