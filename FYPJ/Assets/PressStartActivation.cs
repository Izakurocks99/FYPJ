using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.PS4;

public class PressStartActivation : MonoBehaviour {

	public GameObject pressStartGO;
	Animator anim;
    SceneSwitch sceneSwitcher;

	void Start ()
	{
		anim = pressStartGO.GetComponent<Animator>();
        sceneSwitcher = FindObjectOfType<SceneSwitch>();

        StartCoroutine("Init");
	}

	IEnumerator Init()
	{
		yield return new WaitForSeconds(4.25f);
		pressStartGO.SetActive(true);

		yield break;
	}

	private void Update()
    {
        if (pressStartGO.activeInHierarchy)
        {
            //if (Input.anyKeyDown)
            //{
            //    anim.Play("PressStartSelected");
            //    StartCoroutine(SceneSwitch());
            //}
#if UNITY_PS4
            if (PS4Input.MoveGetButtons(0, 1) > 0)
            {
                anim.Play("PressStartSelected");
                StartCoroutine(SceneSwitch());
            }
            else if (PS4Input.MoveGetButtons(0, 0) > 0)
            {
                anim.Play("PressStartSelected");
                StartCoroutine(SceneSwitch());
            }
#else
            if (Input.anyKeyDown)
            {
                anim.Play("PressStartSelected");
                StartCoroutine(SceneSwitch());
            }
#endif
        }
    }

    IEnumerator SceneSwitch()
	{
		yield return new WaitForSeconds(1);
        sceneSwitcher.LoadScene();
            }
}
