using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class MainMenuManager : MonoBehaviour
{
    public Text ds4StatusText;
    public Slider ds4Slider;
    public Text moveStatusText;
    public Slider moveSlider;
    public Text renderScaleText;
    public Slider renderScaleSlider;
    public Text socialScreenText;
    public Slider socialScreenSlider;

    void Start()
    {
        socialScreenText.text = string.Format("<b>Social Screen</b>: {0}", UnityEngine.XR.XRSettings.showDeviceView == true ? "Disabled" : "Enabled");
        renderScaleText.text = string.Format("<b>Renderscale</b>: {0}", UnityEngine.XR.XRSettings.eyeTextureResolutionScale.ToString("F1"));
    }

    void Update()
    {
        DualShock4Check();
        MoveCheck();
    }

    void DualShock4Check()
    {
#if UNITY_PS4
        if (PS4Input.PadIsConnected(0))
        {
            ds4StatusText.text = "<b>Status: Ready!</b>";
            ds4Slider.interactable = true;
        }
        else
        {
            ds4StatusText.text = "<b>Status: Error!</b>\n\nWireless controller not detected!";
            ds4Slider.interactable = false;
        }
#endif
    }

    void MoveCheck()
    {
        int connectedControllers = 0;

#if UNITY_PS4
        if (PS4Input.MoveIsConnected(0, 0))
            connectedControllers++;
        if (PS4Input.MoveIsConnected(0, 1))
            connectedControllers++;
#endif

        if (connectedControllers == 2)
        {
            moveStatusText.text = "<b>Status: Ready!</b>";
            moveSlider.interactable = true;
        }
        else
        {
            moveStatusText.text = string.Format("<b>Status: Error!</b>\n\n{0} motion controllers detected! 2 are required.", connectedControllers);
            moveSlider.interactable = false;
        }
    }
    
    // Shooter Scene controlled by a DualShock 4 controller
    public void GoToDualShock4Scene()
    {
        FindObjectOfType<SceneSwitcher>().SwitchToScene("PSVRExample_DualShock4");
    }

    // Shooter Scene controlled by the Move controllers
    public void GoToMoveScene()
    {
        FindObjectOfType<SceneSwitcher>().SwitchToScene("PSVRExample_MoveControllers");
    }

    // Toggle social screen on/off
    public void OptionsToggleShowHmdView()
    {
        VRManager.instance.ToggleHMDViewOnMonitor();

        socialScreenText.text = string.Format("<b>Social Screen</b>: {0}", UnityEngine.XR.XRSettings.showDeviceView == true ? "Disabled" : "Enabled");
        socialScreenSlider.value = 0;
    }

    // As an example, change the renderscale between low and recommended levels
    public void SwitchRenderScale()
    {
        if(UnityEngine.XR.XRSettings.eyeTextureResolutionScale == 1.4f)
            VRManager.instance.ChangeRenderScale(0.7f);
        else
            VRManager.instance.ChangeRenderScale(1.4f);
        
        renderScaleText.text = string.Format("<b>Renderscale</b>: {0}", UnityEngine.XR.XRSettings.eyeTextureResolutionScale.ToString("F1"));
        renderScaleSlider.value = 0;
    }
}
