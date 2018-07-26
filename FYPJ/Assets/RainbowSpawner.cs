using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowSpawner : MonoBehaviour {

	public GameObject[] normalSpawners = new GameObject[8];
	public GameObject rainbowTarget;
	public GameObject pool;
	public float duration = 5f;
	float count = 0;

	private void Start()
	{
		pool = GameObject.Find("pool");
	}

	void OnEnable()
	{
		count = 0;

		for(int i = 0; i<normalSpawners.Length; i++)																//desactivate other spawner
			normalSpawners[i].SetActive(false);

		StartCoroutine("RainbowSpawn");																				//start Rainbow Mode
	}


	//rainbow Mode
	IEnumerator RainbowSpawn()
	{
		yield return new WaitForSeconds(.6f);																		//wait before until the other beats disappear before spawning rainbows


		//Rainbow Spawn
		while(count < duration)
		{
			GameObject _instance = pool.GetComponent<ObjectPool>().GetObjectFromPool(2);							//Get Rainbow from pool
			//_instance.transform.parent = this.transform;															//give them their new parent(no more needed) and position
			_instance.transform.position = this.transform.position;
			_instance.GetComponent<NoteComponent>().typeIndex = 2;													//giving them their index so they'll return into their pool
			_instance.GetComponent<NoteComponent>().destination = GameObject.Find("target 8").transform.position;	//setting their  destination
			yield return new WaitForSeconds(.15f);																	//waiting a bit between 2 rainbow beats
		}


		StartCoroutine("ActivateNormalSpawner");																	//reactivates normal spawner and desactivate this one

		                                                                        


		yield break;
	}

	private void Update()
	{
		count += Time.deltaTime;																					//updating the duration time of rainbow mode
	}

	IEnumerator ActivateNormalSpawner()
	{
		yield return new WaitForSeconds(.6f);																		//waiting a bit before reactivating other spawners

		for (int i = 0; i < normalSpawners.Length; i++)                                                             //reactivating others spawners
			normalSpawners[i].SetActive(true);


		this.gameObject.SetActive(false);                                                                            //desactivating rainbow Spawner
	}

}
