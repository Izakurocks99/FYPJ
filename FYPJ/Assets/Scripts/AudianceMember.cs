using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class AudianceMember : MonoBehaviour {

    // Use this for initialization
    [HideInInspector]
    public GameObject manager;
	Animator animator;
	//float timer = 0;
	AudianceManager auManager = null;

   public  void OnPoolAwake()
    {
		animator = gameObject.GetComponent<Animator>();
		StartCoroutine("SwitchAnim");
		auManager = manager.GetComponent<AudianceManager>();
    }

    // Update is called once per frame
    void Update()
    {
		float hype = auManager.HypeMeter;

        if (hype < 25)
		{
			animator.SetBool("mediumHype", false);
			animator.SetBool("highHype", false);
		}

		else if (hype < 95)
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


	IEnumerator SwitchAnim()
	{
		yield return new WaitForSeconds(Random.Range(5f, 8f));
		animator.SetBool("switch", true);
		yield return new WaitForSeconds(.5f);
		animator.SetBool("switch", false);
		StartCoroutine("SwitchAnim");
		yield break;
	}
}


//TODO
// how character moves?
// can we lerp the movement?
