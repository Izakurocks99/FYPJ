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

    public float lifetime = 2f;
    float currlifetime = 0;

    // Use this for initialization
    void Start () {
        missAnimator = missIndicator.GetComponent<Animator>();
        hitAnimator = hitIndicator.GetComponent<Animator>();
        currlifetime = lifetime;
    }
	
	// Update is called once per frame
	void Update () {
        currlifetime -= Time.deltaTime;
        if (currlifetime <= 0)
        {
            missIndicator.SetActive(false);
            hitIndicator.SetActive(false);
        }

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
        currlifetime = lifetime;
        StopAnim();
        hitIndicator.SetActive(true);
        hitAnimator.Play("SkillfulHit");
    }

    public void PlayMissAnim()
    {
        currlifetime = lifetime;
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
