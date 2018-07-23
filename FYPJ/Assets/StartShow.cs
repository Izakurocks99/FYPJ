using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartShow : MonoBehaviour {


    [SerializeField]
    float _Time = 3;
    float timer = 0;

    Material mat = null;

    bool show = false;
	// Use this for initialization
	void Start () {
        mat = this.GetComponent<Renderer>().material;
	}

    public void Show()
    {
        show = true;
    }
	// Update is called once per frame
	void Update () {
        if (show)
        {
            timer += Time.deltaTime;
            if (timer > _Time)
            {
                this.gameObject.SetActive(false);
                return;
            }
            float percent = timer / _Time;
            mat.SetFloat("_Alpha", percent);
        }
	}
}
