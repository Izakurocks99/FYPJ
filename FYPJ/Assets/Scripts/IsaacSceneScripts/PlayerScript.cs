using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public int comboMeter = 0;
    ControllerScript[] controllers;

    private void Start()
    {
        controllers = GetComponentsInChildren<ControllerScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ModifyCombo(bool hit)
    {
        if (hit)
            comboMeter++;
        else
            comboMeter = 0;

        foreach (ControllerScript controller in controllers) //for each controllers
        {
            List<BatonCapsuleFollower> followers = controller.currStick.BatonFollowers;
            foreach (BatonCapsuleFollower follower in followers)//for each baton followers
            {
                follower.gameObject.GetComponent<Rigidbody>().mass = comboMeter + 1;
            }
        }

        Debug.Log(comboMeter);
    }
}
