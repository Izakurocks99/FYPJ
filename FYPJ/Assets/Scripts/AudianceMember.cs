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
    Vector3 startPos;

   public  void OnPoolAwake()
    {
		animator = gameObject.GetComponent<Animator>();
		StartCoroutine("SwitchAnim");
		auManager = manager.GetComponent<AudianceManager>();

        Vector3 player = FindObjectOfType<PlayerStats>().gameObject.transform.position;
        Vector3 tempforward = (player - this.gameObject.transform.position);
        transform.forward = new Vector3(tempforward.x, 0 , tempforward.z).normalized;
        startPos = transform.position;
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

        float x = Mathf.PerlinNoise(Time.time, 0);
        float y = Mathf.PerlinNoise(0,Time.time);

        transform.position = startPos + new Vector3(x,y,0);
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
