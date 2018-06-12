using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
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

public class ControllerScript : MonoBehaviour
{
    //public bool isSecondaryMoveController = false;
    public GameObject player;
    public GameObject movementMarker;
    public LaserPointer laserPointer;
    public MovementScript moveScript;
    public ControllerButtons resetButton;
    private RaycastHit hit;
    private Vector3 startingPos;

    // Use this for initialization
    void Start ()
    {
        startingPos = player.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(GetControllerKey(ControllerButtons.BackTrigger)))
        {
            //interact with UI
            if (laserPointer.LineRaycast().collider && laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>())
            {
                laserPointer.LineRaycast().collider.gameObject.GetComponent<Button>().onClick.Invoke();
            }
            else if (!movementMarker.activeInHierarchy) //create icon
                SpawnMarker();
        }
        else if(Input.GetKeyUp(GetControllerKey(ControllerButtons.BackTrigger)))
        {
            //move player and remove icon
            Move();
        }

        //update marker to controller forward
        if (Input.GetKey(GetControllerKey(ControllerButtons.BackTrigger)) && movementMarker.activeInHierarchy)
        {
            movementMarker.transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        }

        //reset player position
        if (Input.GetKeyDown(GetControllerKey(resetButton)))
        {
            player.transform.position = startingPos;
        }
    }

    // Move controlelrs use an API for their analog button, DualShock 4 uses an axis name for R2
    //    bool CheckForInput()
    //    {
    //#if UNITY_PS4
    //        if (isMoveController)
    //        {
    //            if (!isSecondaryMoveController)
    //            {
    //                return (PS4Input.MoveGetAnalogButton(0, 0) > 0 ? true : false);
    //            }
    //            else
    //            {
    //                return (PS4Input.MoveGetAnalogButton(0, 1) > 0 ? true : false);
    //            }
    //        }
    //        else
    //        {
    //            return (Input.GetAxisRaw("TriggerRight") > 0 ? true : false);
    //        }
    //#else
    //		return Input.GetButton("Fire1");
    //#endif
    //    }

    //Move to Location

    void Move()
    {
        if (movementMarker.activeInHierarchy)
        {
            Vector3 height = new Vector3(0f, 0.5f, 0f); //set player height
            player.transform.position = movementMarker.transform.position + height;//move player
            
            player.transform.forward = movementMarker.transform.forward; 
            movementMarker.SetActive(false);
        }
    }

    void SpawnMarker()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.transform.gameObject.tag == "Ground") //hits te ground
            {
                movementMarker.SetActive(true); // create marker
                movementMarker.transform.position = hit.point;
            }
        }
    }

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
}
