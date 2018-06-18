using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCalibrationScript : MonoBehaviour
{
    //adjust calibration area from headdevice to controllers

    //TODO use transform instead of gameobject
    public Transform playerCamera;
    public Transform calibrationObject;
    public Transform LeftController;
    public Transform RightController;

    private bool isLocked; //is calibrating?
    private bool followSecondary;
    private Transform currController;

    // Use this for initialization
    void Start()
    {
        isLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocked)
        {
            //get average dist of controllers
            if (followSecondary)
                currController = LeftController;
            else
                currController = RightController;

            //move cali obj
            calibrationObject.position = new Vector3(calibrationObject.position.x, calibrationObject.position.y, currController.position.z);

            Vector3 distance = (currController.position - calibrationObject.position);
            calibrationObject.localScale = new Vector3(distance.x * 2, distance.y * 2, calibrationObject.localScale.z);//keep y pos
        }
    }

    public void LockObject()
    {
        isLocked = true;
    }

    public void UnlockObject(bool isSecondary)
    {
        if (isLocked)
        {
            isLocked = false;
            followSecondary = isSecondary;
        }
    }
}
