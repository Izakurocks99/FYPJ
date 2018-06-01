using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public static class MyExtension
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (var i = 0; i < list.Count; i++)
            list.Swap(i, Random.Range(i, list.Count));
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}


public sealed class AudianceManager : MonoBehaviour {



    [SerializeField]
    Vector2 StageDimension = new Vector2(100, 100);
    [SerializeField]
    float MinRangeFromStage = 10;
    

    [SerializeField]
    [Range(0, 100)]
    public int HypeMeter = 0;
    [SerializeField]
    uint MaxAudianceAmount = 100;
    [SerializeField]
    public GameObject Member = null;

    [SerializeField]
    float SpawnTime = 0;
    float _Timer = 0;
    List<GameObject> _ObjectPool = new List<GameObject>();

    [SerializeField]
    float Frequenzy = 0;


    float lastHype = 0;
    const int MaxHype = 100;
    // Use this for initialization
    //Vector3[] TempPosArray;

    [SerializeField]
    int currentlyActiveAmount = 0;
    void Start()
    {
      /*  TempPosArray = new Vector3[10 * 10];
        for (uint y = 0; y < 10; y++)
        {
            for (uint x = 0; x < 10; x++)
            {
                TempPosArray[y * 10 + x] = new Vector3(y * 2, x * 2);  
            }
        }
        */

        Debug.Assert(Member != null);
        Quaternion q = new Quaternion();
        for (uint i = 0; i < MaxAudianceAmount; i++)
        {
            GameObject temp = GameObject.Instantiate(Member);
            temp.SetActive(false);
            AudianceMember tempCom = temp.GetComponent(typeof(AudianceMember)) as AudianceMember;
            Debug.Assert(tempCom != null);
            tempCom.manager = this.gameObject;
            temp.transform.parent = this.transform;
            _ObjectPool.Add(temp);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {



        Debug.Assert(HypeMeter >= 0);
        Debug.Assert(currentlyActiveAmount >= 0);

        _Timer += Time.deltaTime;
        if (_Timer > SpawnTime) // check do we need to spawn/ despawn members
        {
            _Timer = 0;

            float diff = HypeMeter - lastHype;
            if (Mathf.Abs(diff) > Frequenzy)
            {
                bool setActive = diff > 0;    // turn on or off
                int DesiredAmount = (int)(MaxAudianceAmount * (Mathf.Abs(diff) / MaxHype));

                int i = 0;
                int count = 0;
                while (count < DesiredAmount)
                {
                    if (_ObjectPool[i].activeInHierarchy != setActive)
                    {
                        // calculate position for the object


                        _ObjectPool[i].SetActive(setActive);
                        count++;
                        currentlyActiveAmount = setActive ? currentlyActiveAmount + 1 : currentlyActiveAmount - 1;

                        if (setActive)
                        {
                            float maxpos = (MaxAudianceAmount /  currentlyActiveAmount ) * StageDimension.y; // todo fix to correct scale
                            maxpos = maxpos < MinRangeFromStage ? MinRangeFromStage : maxpos;
                            float realpos = Random.Range(0f, maxpos);
                            float axis = Random.Range(0f, Mathf.PI);

                            
                            float x = Mathf.Cos(axis) * realpos;
                            float y = Mathf.Sin(axis) * realpos;

                            _ObjectPool[i].transform.position = new Vector3(x,y,0);//
                        }
                    }
                    if (++i == _ObjectPool.Count) // assert? 
                        break;
                }
                lastHype = HypeMeter;
                //_ObjectPool.Shuffle();
            }
        }
    }
}

// TODO 
// where to spawn, and rotation
// take asserts away
// randomize despawn and spawn or the positions 
// check the rounding error!