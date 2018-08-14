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

    bool move = false;
    float seed = 0;
    public void OnPoolAwake(bool mov,float _seed)
    {
		animator = gameObject.GetComponent<Animator>();
		StartCoroutine("SwitchAnim");
		auManager = manager.GetComponent<AudianceManager>();

        Vector3 player = FindObjectOfType<PlayerStats>().gameObject.transform.position;
        Vector3 tempforward = (player - this.gameObject.transform.position);
        transform.forward = new Vector3(tempforward.x, 0 , tempforward.z).normalized;
        startPos = transform.position;
        move = mov;
        seed = _seed;
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
        if(!move) return;

        float x = (Mathf.PerlinNoise(Time.time, seed) * 2 - 1) * 0.01f;
        float y = (Mathf.PerlinNoise(seed,Time.time) * 2 - 1) * 0.01f;

        transform.position = transform.position + new Vector3(y,0,x);

        if(transform.position.z < -5f)
            transform.position = new Vector3(transform.position.x,transform.position.y,-5f);
        if(transform.position.z > 9f)
            transform.position = new Vector3(transform.position.x,transform.position.y,9f);
        if(transform.position.x < -13f)
            transform.position = new Vector3(-13f,transform.position.y,transform.position.z);
        if(transform.position.x > 13f)
            transform.position = new Vector3(13f,transform.position.y,transform.position.z);

        transform.LookAt(new Vector3(0f,-0.9f,-10.4f));
        transform.rotation = new Quaternion(0,transform.rotation.y ,0f,transform.rotation.w);
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

