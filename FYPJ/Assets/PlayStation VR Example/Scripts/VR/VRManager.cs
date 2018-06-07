﻿using UnityEngine;
using UnityEngine.VR;
using System.Collections;
#if UNITY_PS4
using UnityEngine.PS4.VR;
using UnityEngine.PS4;
#endif

public class VRManager : MonoBehaviour
{
    public float renderScale = 1.4f; // 1.4 is Sony's recommended scale for PlayStation VR
    public bool showHmdViewOnMonitor = true; // Set this to 'false' to use the monitor/display as the Social Screen
    private static VRManager _instance;

    public static VRManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<VRManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != _instance)
        {
            // There can be only one!
            Destroy(gameObject);
        }
    }

    public void BeginVRSetup()
    {
        StartCoroutine(SetupVR());
    }

    IEnumerator SetupVR()
    {
#if UNITY_PS4
        // Register the callbacks needed to detect resetting the HMD
		Utility.onSystemServiceEvent += OnSystemServiceEvent;
        PlayStationVR.onDeviceEvent += onDeviceEvent;

        // Post-reproject for camera locked items, in this case the reticle. Must be
        // set before we change the VR Device. See VRPostReprojection.cs for more info
        if (Camera.main.actualRenderingPath == RenderingPath.Forward)
        {
            if (FindObjectOfType<VRPostReprojection>())
                PlayStationVRSettings.postReprojectionType = PlayStationVRPostReprojectionType.PerEye;
            else
                Debug.LogError("You are trying to enable support for post-reprojection, but no post-reprojection script was found!");
        }
        else
        {
            Debug.LogError("Post reprojection is not yet fully supported in non-Forward Rendering Paths.");
        }
#endif

#if UNITY_5_3
        VRSettings.loadedDevice = VRDeviceType.PlayStationVR;
#elif UNITY_5_4_OR_NEWER
        UnityEngine.XR.XRSettings.LoadDeviceByName(VRDeviceNames.PlayStationVR);
#endif

        // WORKAROUND: At the moment the device is created at the end of the frame so
        // changing almost any VR settings needs to be delayed until the next frame
        yield return null;
        UnityEngine.XR.XRSettings.enabled = true;
        UnityEngine.XR.XRSettings.eyeTextureResolutionScale = renderScale;
        UnityEngine.XR.XRSettings.showDeviceView = showHmdViewOnMonitor;
    }

    public void BeginShutdownVR()
    {
        StartCoroutine(ShutdownVR());
    }

    IEnumerator ShutdownVR()
    {
#if UNITY_5_3
        VRSettings.loadedDevice = VRDeviceType.None;
#elif UNITY_5_4_OR_NEWER
        UnityEngine.XR.XRSettings.LoadDeviceByName(VRDeviceNames.None);
#endif

        // WORKAROUND: At the moment the device is created at the end of the frame so
        // we need to wait a frame until the VR device is changed back to 'None', and
        // then reset the Main Camera's FOV and Aspect
        yield return null;

        UnityEngine.XR.XRSettings.enabled = false;
#if UNITY_PS4
        PlayStationVR.SetOutputModeHMD(false);
#endif
        //Camera.main.ResetFieldOfView();
        Camera.main.ResetAspect();
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void SetupHMDDevice()
    {
#if UNITY_PS4
        HmdSetupDialog.OpenAsync(0, OnHmdSetupDialogCompleted);
#endif
    }

    public void ToggleHMDViewOnMonitor(bool showOnMonitor)
    {
        showHmdViewOnMonitor = showOnMonitor;
        UnityEngine.XR.XRSettings.showDeviceView = showHmdViewOnMonitor;
    }

    public void ToggleHMDViewOnMonitor()
    {
        showHmdViewOnMonitor = !showHmdViewOnMonitor;
        UnityEngine.XR.XRSettings.showDeviceView = showHmdViewOnMonitor;
    }

    public void ChangeRenderScale(float scale)
    {
        UnityEngine.XR.XRSettings.eyeTextureResolutionScale = scale;
    }

#if UNITY_PS4
    // HMD recenter happens in this event
    void OnSystemServiceEvent(UnityEngine.PS4.Utility.sceSystemServiceEventType eventType)
    {
        Debug.LogFormat("OnSystemServiceEvent: {0}", eventType);

        switch (eventType)
        {
            case Utility.sceSystemServiceEventType.RESET_VR_POSITION:
                UnityEngine.XR.InputTracking.Recenter();
                break;
        }
    }
#endif

#if UNITY_PS4
    // Detect completion of the HMD dialog and either proceed to setup VR, or throw a warning
    void OnHmdSetupDialogCompleted(DialogStatus status, DialogResult result)
    {
        Debug.LogFormat("OnHmdSetupDialogCompleted: {0}, {1}", status, result);

        switch (result)
        {
            case DialogResult.OK:
                StartCoroutine(SetupVR());
                break;
            case DialogResult.UserCanceled:
                Debug.LogWarning("User Cancelled HMD Setup!");
                BeginShutdownVR();
                break;
        }
    }
#endif

#if UNITY_PS4
    // This handles disabling VR in the event that the HMD has been disconnected
    bool onDeviceEvent(PlayStationVR.deviceEventType eventType, int value)
    {
        Debug.LogFormat("### onDeviceEvent: {0}, {1}", eventType, value);
        bool handledEvent = false;

        switch (eventType)
        {
            case PlayStationVR.deviceEventType.deviceStopped:
                BeginShutdownVR();
                handledEvent = true;
                break;
            case PlayStationVR.deviceEventType.StatusChanged:   // e.g. HMD unplugged
                VRDeviceStatus devstatus = (VRDeviceStatus)value;
                Debug.LogFormat("DeviceStatus: {0}", devstatus);
                if (devstatus != VRDeviceStatus.Ready)
                {
                    if(UnityEngine.XR.XRSettings.loadedDeviceName == UnityEngine.XR.XRSettings.supportedDevices[0])
                        SetupHMDDevice();
                    else
                        BeginShutdownVR();
                }
                handledEvent = true;
                break;
            case PlayStationVR.deviceEventType.MountChanged:
                VRHmdMountStatus status = (VRHmdMountStatus)value;
                Debug.LogFormat("VRHmdMountStatus: {0}", status);
                handledEvent = true;
                break;
        }

        return handledEvent;
    }
#endif
}