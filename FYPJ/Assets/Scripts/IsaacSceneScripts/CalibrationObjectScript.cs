using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationObjectScript : MonoBehaviour {

    public bool controlledBySecondary;
    public bool calibrateMode = true;
    // Use this for initialization
    void Start () {
        if (PlayerPrefs.HasKey("calivert") && PlayerPrefs.HasKey("calihori"))
        {
            gameObject.transform.localScale = new Vector3(PlayerPrefs.GetFloat("calivert"), PlayerPrefs.GetFloat("calihori"), gameObject.transform.localScale.z);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
