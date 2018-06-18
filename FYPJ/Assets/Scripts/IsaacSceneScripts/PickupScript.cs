using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class PickupScript : ControllerModesScript
{
    //Pickup Mode
    //Can interact with objects
    //Move hand to object and interact to pick it up

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ////check witch controller is pressed
        //if ((PS4Input.MoveGetButtons(0, 0) > 0 ? true : false))
        //{
        //    Debug.Log(PS4Input.MoveGetButtons(0, 0));
        //    if (Input.GetKeyDown(KeyCode.JoystickButton5)) //key is pressed
        //    {
        //        if (rightHand.GetComponent<ControllerScript>())
        //        {
        //            LaserPointer lp = rightHand.GetComponent<ControllerScript>().laserPointer;
        //            lp.gameObject.SetActive(!lp.gameObject.activeInHierarchy);
        //        }
        //    }
        //}

        //if ((PS4Input.MoveGetButtons(0, 1) > 0 ? true : false))
        //{
        //    Debug.Log(PS4Input.MoveGetButtons(0, 1));
        //    if (Input.GetKeyDown(KeyCode.JoystickButton5)) //key is pressed
        //    {
        //        if (leftHand.GetComponent<ControllerScript>())
        //        {
        //            LaserPointer lp = leftHand.GetComponent<ControllerScript>().laserPointer;
        //            lp.gameObject.SetActive(!lp.gameObject.activeInHierarchy);
        //        }

        //    }
        //}

    }
}
