using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public static class MyExtension2
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
            list.Swap(i, Random.Range(i, list.Count));
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        T temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
    public static void FastDelete<T>(this IList<T> list, int i) // wont keep the order
    {
        list.Swap(i , list.Count - 1);
        list.RemoveAt(list.Count - 1);
    }
    public static T PopBack<T>(this IList<T> list)
    {
        T ret = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return ret;
    }
    public static T PopAt<T>(this IList<T> list, int i)
    {
        T ret = list[i];
        list.FastDelete(i);
        return ret;
    }

}


public sealed class AudianceManager : MonoBehaviour {

    [SerializeField]
    List<GameObject> Platforms = new List<GameObject>();

    [SerializeField]
    [Range(0, 100)]
    public int HypeMeter = 0;
    [SerializeField]
    [Range(0, 100)]
    int temp = 0;
    [SerializeField]
    uint MaxAudianceAmount = 100;
    [SerializeField]
    List<GameObject> Member = new List<GameObject>();

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


    private List<Platforms> PlatformList = new List<Platforms>();

    float SpawnChance = 0f;
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
        //Debug.Log(string.Format("AudianceManager {0}", this.gameObject.GetHashCode()));

        Debug.Assert(Member != null);
        //Quaternion q = new Quaternion();
        for (uint i = 0; i < MaxAudianceAmount; i++)
        {
            GameObject temp = GameObject.Instantiate(Member.GetRandom());
            temp.SetActive(false);
            AudianceMember tempCom = temp.GetComponentInChildren(typeof(AudianceMember)) as AudianceMember;
            Debug.Assert(tempCom != null);
            tempCom.manager = this.gameObject;
            temp.transform.parent = this.transform;
            _ObjectPool.Add(temp);
        }

        this.transform.position = new Vector3(0,0,0);

        Debug.Assert(Platforms.Count != 0);
        for (int i = 0; i < Platforms.Count; i++)
        {
            GameObject temp =  GameObject.Instantiate(Platforms[i]);
            temp.SetActive(true);

            Platforms t = temp.GetComponent<Platforms>();
            Debug.Assert(t);

            SpawnChance += t.SpawnChance;
            PlatformList.Add(t);
        }

       // stagePosition = new Vector2(MiddlePosition.x, MiddlePosition.y - (StageDimension.y / 2f));


        //VerticalScale = StageDimension.x / (StageDimension.y * 2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
//#if false


        Debug.Assert(HypeMeter >= 0);
        Debug.Assert(currentlyActiveAmount >= 0);

        _Timer += Time.deltaTime;
        if (_Timer > SpawnTime) // check do we need to spawn/ despawn members
        {
            _Timer = 0;

            //float diff = HypeMeter - lastHype;
            float diff = temp - lastHype;
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
                        _ObjectPool[i].SetActive(setActive);
                        count++;
                        currentlyActiveAmount = setActive ? currentlyActiveAmount + 1 : currentlyActiveAmount - 1;

                        if (setActive)
                        {
                            Debug.Log("SPAWNING");
                            float scale = (float)currentlyActiveAmount / (float)MaxAudianceAmount;

                            float platformIndicator = Random.Range(0f, SpawnChance);

                            float currentChance = 0;
                            int i2 = 0;
                            Platforms temp = null;
                            while (true)
                            {
                                if (platformIndicator <= PlatformList[i2].SpawnChance + currentChance && platformIndicator >= currentChance)
                                {
                                    temp = PlatformList[i2];
                                    break;
                                }
                                else if (i2 >= PlatformList.Count)
                                {
                                    Debug.Assert(false);
                                    temp = PlatformList[0];
                                    break;
                                }
                                currentChance += PlatformList[i2].SpawnChance; 
                                i2++;
                            }
                            //temp = PlatformList[0];
                            _ObjectPool[i].transform.position = temp.SpawnToPlatform(scale);
                            temp.InsertToInnerStructure(_ObjectPool[i]);
							_ObjectPool[i].GetComponent<AudianceMember>().OnPoolAwake();
                        }

                    }
                    if (++i == _ObjectPool.Count) // assert? 
                        break;
                }
                //lastHype = HypeMeter;
                lastHype = temp;
                //_ObjectPool.Shuffle();
            }
        }
//#endif
    }
}

// TODO 
// where to spawn, and rotation
// take asserts away
// check the rounding error! gone? 
