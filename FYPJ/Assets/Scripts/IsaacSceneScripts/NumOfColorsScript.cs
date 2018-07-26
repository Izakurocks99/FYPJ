using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumOfColorsScript : MonoBehaviour {

    [SerializeField]
    Button fourColors;
    [SerializeField]
    Button twoColors;

    // Use this for initialization
    void Start () {
        if (PlayerPrefs.GetInt("dualcolor") == 0)
            twoColors.gameObject.SetActive(false);
        else
            fourColors.gameObject.SetActive(false);


    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
