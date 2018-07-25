using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandTargetMenuScript : MonoBehaviour {

    [SerializeField]
    Button straightButton;
    [SerializeField]
    Button RandomButton;

    // Use this for initialization
    void Start ()
    {
        if (PlayerPrefs.GetInt("randomarea") == 1)
            straightButton.gameObject.SetActive(false);
        else
            RandomButton.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
