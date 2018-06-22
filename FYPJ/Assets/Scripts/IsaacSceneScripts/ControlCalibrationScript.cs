﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCalibrationScript : ControllerModesScript
{
    //adjust calibration area from headdevice to controllers

    public GameObject calibrationObject;
    private CalibrationObjectScript calibrationObjectScript;

    public Transform playerCamera;

    //public GameObject edges;

    public float horizontalSize;
    public float verticleSize;
    public float distFromPlayer;

    private bool isLocked;
    private bool followSecondary;
    private Transform currController;
    private ControllerScript controller;

    // Use this for initialization
    void Awake()
    {
        isLocked = true;
        controller = gameObject.GetComponent<ControllerScript>();
        calibrationObjectScript = calibrationObject.GetComponent<CalibrationObjectScript>();
        currController = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocked && calibrationObjectScript.controlledBySecondary == controller.isSecondaryMoveController)
        {
            //move cali obj
            calibrationObject.transform.position = new Vector3(calibrationObject.transform.position.x, calibrationObject.transform.position.y, currController.position.z);

            //scale cali obj
            Vector3 distance = (currController.position - calibrationObject.transform.position);
            calibrationObject.transform.localScale = new Vector3(distance.x * 2, distance.y * 2, calibrationObject.transform.localScale.z);//keep y pos
        }
    }

    public override void Init() //when this mode is entered
    {
        calibrationObject.SetActive(true);
        controller.laserPointer.gameObject.SetActive(false);
    }

    public override void Exit() //when this mode is exited
    {
        calibrationObject.SetActive(false);
        controller.laserPointer.gameObject.SetActive(true);
    }

    public override void ButtonPressed(ControllerButtons button)
    {
        if(button == controlsScript.interactButton)
        {
            UnlockObject();
        }
        if (button == controlsScript.togglePointerButton)
        {
            controller.laserPointer.gameObject.SetActive(true);
            calibrationObjectScript.calibrateMode = false;
        }
    }

    public override void ButtonReleased(ControllerButtons button)
    {
        if (button == controlsScript.interactButton)
        {
            LockObject();
        }
        if (button == controlsScript.togglePointerButton)
        {
            controller.laserPointer.gameObject.SetActive(false);
        }
    }

    void LockObject()
    {
        if (calibrationObjectScript.controlledBySecondary == controller.isSecondaryMoveController)
        {
            isLocked = true;
            //set variables
            distFromPlayer = (calibrationObject.transform.position - playerCamera.position).magnitude;
            horizontalSize = Mathf.Abs(calibrationObject.transform.localScale.y * 0.5f);
            verticleSize = Mathf.Abs(calibrationObject.transform.localScale.x * 0.5f);
        }
        //DEBUG
        ////Debug.Log(horizontalSize + " : " + verticleSize);
        //Vector3 debugPoints = new Vector3(horizontalSize, verticleSize, 0);
        ////spawn debug spheres
        //GameObject TR = Instantiate(edges, calibrationObject.transform.transform.position + debugPoints, calibrationObject.transform.rotation);
        //Debug.Log(TR.transform.position);
        //GameObject BL = Instantiate(edges, calibrationObject.transform.transform.position - debugPoints, calibrationObject.transform.rotation);
        //Debug.Log(BL.transform.position);

    }

    void UnlockObject()
    {
        if (isLocked && calibrationObjectScript.calibrateMode)
        {
            isLocked = false;
            calibrationObjectScript.controlledBySecondary = controller.isSecondaryMoveController;
        }
    }
}
