using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCalibrationScript : ControllerModesScript
{
    //adjust calibration area from headdevice to controllers

    //TODO use transform instead of gameobject
    public Transform playerCamera;
    public Transform calibrationObject;
    public Transform LeftController;
    public Transform RightController;

    public GameObject edges;

    public float horizontalSize;
    public float verticleSize;
    public float distFromPlayer;

    private bool isLocked; //is calibrating?
    private bool followSecondary;
    private Transform currController;

    // Use this for initialization
    void Awake()
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

            //scale cali obj
            Vector3 distance = (currController.position - calibrationObject.position);
            calibrationObject.localScale = new Vector3(distance.x * 2, distance.y * 2, calibrationObject.localScale.z);//keep y pos
        }
    }

    public override void Init() //when this mode is entered
    {

    }

    public override void Exit() //when this mode is exited
    {

    }

    public override void ButtonPressed(ControllerButtons button, bool isSecondaryController)
    {
        if(button == controlsScript.interactButton)
        {
            UnlockObject(isSecondaryController);
        }
    }

    public override void ButtonReleased(ControllerButtons button, bool isSecondaryController)
    {
        if (button == controlsScript.interactButton)
        {
            LockObject();
        }
    }

    void LockObject()
    {
        isLocked = true;
        //set variables
        distFromPlayer = (calibrationObject.position - playerCamera.position).magnitude;
        horizontalSize = Mathf.Abs(calibrationObject.localScale.y * 0.5f);
        verticleSize = Mathf.Abs(calibrationObject.localScale.x * 0.5f);

        //DEBUG
        ////Debug.Log(horizontalSize + " : " + verticleSize);
        //Vector3 debugPoints = new Vector3(horizontalSize, verticleSize, 0);
        ////spawn debug spheres
        //GameObject TR = Instantiate(edges, calibrationObject.transform.position + debugPoints, calibrationObject.rotation);
        //Debug.Log(TR.transform.position);
        //GameObject BL = Instantiate(edges, calibrationObject.transform.position - debugPoints, calibrationObject.rotation);
        //Debug.Log(BL.transform.position);

    }

    void UnlockObject(bool isSecondary)
    {
        if (isLocked)
        {
            isLocked = false;
            followSecondary = isSecondary;
        }
    }
}
