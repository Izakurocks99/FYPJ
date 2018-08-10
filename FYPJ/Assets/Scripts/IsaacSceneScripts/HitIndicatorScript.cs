using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIndicatorScript : MonoBehaviour {

    [SerializeField]
    GameObject missIndicator = null;
    Animator missAnimator;

    [SerializeField]
    bool playMissAnim;

    [SerializeField]
    GameObject hitIndicator = null;
    Animator hitAnimator;

    [SerializeField]
    bool playHitAnim;

    // Use this for initialization
    void Start () {
        missAnimator = missIndicator.GetComponent<Animator>();
        hitAnimator = hitIndicator.GetComponent<Animator>();
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
        hitAnimator.Play("SkillfulHit");
    }

    public void PlayMissAnim()
    {
        StopAnim();
        missIndicator.SetActive(true);
        missAnimator.Play("PoorHit");
        
    }
    
    void StopAnim()
    {
        missIndicator.SetActive(false);
        hitIndicator.SetActive(false);
    }
}
