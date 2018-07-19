using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissiveSwap : MonoBehaviour {
    [SerializeField]
    List<Texture2D> maps = new List<Texture2D>();
    // Use this for initialization
    [SerializeField]
    float Freq = 1f;
    float timer = 0;
    Material m = null;
    Texture2D current = null;
	void Start () {
        Debug.Assert(maps.Count != 0);
        m = this.GetComponent<Renderer>().material;
        Debug.Assert(m);
        current = maps.PopRandom();
        m.SetTexture("_EmissionMap", current);

        
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > Freq)
        {
            timer = 0;
            Texture2D temp = maps.PopRandom();
            m.SetTexture("_EmissionMap", temp);
            maps.Add(current);
            current = temp;
        }
	}
}
