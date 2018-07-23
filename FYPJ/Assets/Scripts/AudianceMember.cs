using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudianceMember : MonoBehaviour {

    // Use this for initialization
    [HideInInspector]
    public GameObject manager;
	Animator animator;
	public float hype = 0;
    


    void Awake()
    {
		animator = this.gameObject.GetComponent<Animator>();
		StartCoroutine("AnimationSwitch");



    }

    // Update is called once per frame
    void Update()
    {
		if (hype < 15)
		{
			animator.SetBool("mediumHype", false);
			animator.SetBool("highHype", false);
		}

		else if (hype < 100)
		{
			animator.SetBool("mediumHype", true);
			animator.SetBool("highHype", false);
		}

		else
		{
			animator.SetBool("mediumHype", true);
			animator.SetBool("highHype", true);
		}
    }


	IEnumerator AnimationSwitch()
	{
		animator.SetBool("switch", false);
		yield return new WaitForSeconds(Random.Range(5, 9));
		animator.SetBool("switch", true);
		yield return new WaitForSeconds(.3f);
		StartCoroutine("AnimationSwitch");
		yield break;
	}
}


//TODO
// how character moves?
// can we lerp the movement?