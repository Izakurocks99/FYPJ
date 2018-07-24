using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
	// Use this for initialization
	
	[SerializeField]
	uint poolSize = 20;

	List<List<GameObject>> Pools = new List<List<GameObject>>();
	
	[SerializeField]
	List<GameObject> Prefabs = new List<GameObject>();
	void Start () {
		Debug.Assert(Prefabs.Count > 0);
		for(int i1 = 0; i1 < Prefabs.Count;i1++)
		{
			Pools.Add(new List<GameObject>());
			for(int i2 = 0; i2 < poolSize;i2++)
			{
				GameObject temp = GameObject.Instantiate(Prefabs[i1]);
				temp.SetActive(false);
				Pools[i1].Add(temp);
			}
	
		}
	}
	public GameObject GetObjectFromPool(int PoolIndex)
	{
		GameObject go = Pools[PoolIndex].PopBack();
		go.SetActive(true);
		return go;
	}
	public void ReturnObjectToPool(GameObject obj,int index)
	{
		obj.SetActive(false);
		Pools[index].Add(obj);
	}
}
