using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneManagerUser : MonoBehaviour {

 
    
    // if different scenes use this
    //[SerializeField]
    //List<string> SceneNames = new List<string>();

    [SerializeField]
    GameObject AudianceManagerPrefab = null;
    [SerializeField]
    GameObject AudianceMemberPrefab = null;
	// Use this for initialization
	void Start () {
        Debug.Assert(AudianceMemberPrefab != null);
        Debug.Assert(AudianceManagerPrefab != null);

        GameObject tempAudianceManager = GameObject.Instantiate(AudianceManagerPrefab);
        tempAudianceManager.SetActive(true);
        (tempAudianceManager.GetComponent(typeof(AudianceManager)) as AudianceManager).Member = AudianceMemberPrefab;

        DontDestroyOnLoad(this.gameObject);       //when scene changes this will stay

    }

	
	// Update is called once per frame
	void FixedUpdate () {
        // not needed ? 
        // or here we could check if we want to load new level?
#if false
        if(true)
        {
            // load next scene
            SceneManager.LoadSceneAsync("nameofnextscene", LoadSceneMode.Additive);
            
        }
#endif
    }


}

// TODO
// when scene loads set default parameters to everything we need
// set seats to audiancemanager
// load start level in start or separate function