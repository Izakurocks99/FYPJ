using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionVisualizer : MonoBehaviour {

    [SerializeField]
    bool negative = false;
    [SerializeField]
    public bool selected = false;
	// Use this for initialization
    
    const float lenght = 2f;
    Vector3 startPosition ;
    Vector3 endPosition ;
    void Start(){
        startPosition = transform.position; 
        endPosition = startPosition;
        endPosition.x = startPosition.x - (negative ? -1 : 1) * lenght;
    }
	// Update is called once per frame
    float state = 0;
	void Update () {
	    state += selected ? Time.deltaTime : -Time.deltaTime;
        state = Mathf.Clamp01(state);

        float newX = (1- ((1 - state)*(1 - state)*(1 - state)*(1 - state))) * (negative ? -1 : 1) * lenght;
        Vector3 p = startPosition;
        p.x += newX; 
        transform.position = p; 

        //Audia
        //Mathf.Smoo
        //selected = false;
        // spring interpolation when  inter and webs comes back

	}
}
