using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteComponent : MonoBehaviour {

	public float speed = 1;
	public GameObject cible;
	public float limitDistance = 0.1f;
	public int number;
	float count = 0;
	


	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		

		cible = GameObject.Find("Cible " + number);
		transform.position = Vector3.Lerp(transform.parent.position, cible.transform.position, count);
		count += Time.deltaTime;
		if (Vector3.Distance(transform.position,cible.transform.position)< limitDistance)
		{
			Destroy(gameObject);
			
		}
	}

	private void Awake()
	{
		
	}




}
