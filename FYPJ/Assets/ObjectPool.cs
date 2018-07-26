using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    
	[System.Serializable]
	public class PoolingData 
	{
		[SerializeField]
		public GameObject Prefab;
		[SerializeField]
		public int numObj = 10;
		[SerializeField]
		public GameObject OptionalParent = null;
	}
	[SerializeField]
	List<PoolingData> poolData = new List<PoolingData>();
	
	List<List<GameObject>> Pools = new List<List<GameObject>>();

	void Start () {
		Debug.Assert(poolData.Count > 0);
        Vector3 Position = new Vector3(0,0,0);
		for(int i1 = 0; i1 < poolData.Count;i1++)
		{
			Pools.Add(new List<GameObject>());
			for(int i2 = 0; i2 < poolData[i1].numObj;i2++)
			{
				GameObject temp = GameObject.Instantiate(poolData[i1].Prefab,Position,new Quaternion());
				Debug.Assert(temp);
				temp.transform.parent =  poolData[i1].OptionalParent != null ? poolData[i2].OptionalParent.transform : null;// 
				temp.SetActive(false);
				Pools[i1].Add(temp);
                Position += new Vector3(10,0,0);
			}
	
		}
	}
	public GameObject GetObjectFromPool(int PoolIndex)
	{
		Debug.Assert(Pools[PoolIndex].Count > 0);
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
