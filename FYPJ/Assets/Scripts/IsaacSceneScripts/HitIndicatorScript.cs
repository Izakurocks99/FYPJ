using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIndicatorScript : MonoBehaviour {

    [SerializeField]
    GameObject missIndicator = null;
    Animation missAnim;

    [SerializeField]
    bool playMissAnim;

    [SerializeField]
    GameObject hitIndicator = null;
    Animation hitAnim;

    [SerializeField]
    bool playHitAnim;

    // Use this for initialization
    void Start () {
        missAnim = missIndicator.GetComponent<Animation>();
        hitAnim = hitIndicator.GetComponent<Animation>();
    }
	
	// Update is called once per frame
	void Update () {
        if (playHitAnim)
        {
            playHitAnim = false;
            PlayHitAnim(); 
        }
        if (playMissAnim)
        {
            playMissAnim = false;
            PlayMissAnim();
        }
    }

    public void PlayHitAnim()
    {
        StopAnim();
        hitIndicator.SetActive(true);
        hitAnim.Play();
    }

    public void PlayMissAnim()
    {
        StopAnim();
        missIndicator.SetActive(true);
        missAnim.Play();
        
    }
    
    void StopAnim()
    {
        missIndicator.SetActive(false);
        hitIndicator.SetActive(false);
    }
}
