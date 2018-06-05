using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Platforms : MonoBehaviour {

    [SerializeField]
    protected Vector3 Position = new Vector3(0,0,0);

    [SerializeField]
    protected Vector2 Rotations = new Vector2(0, 0);

    [SerializeField]
    float spawnChance = 10;

    public float SpawnChance
    {
        get
        {
            return spawnChance;
        }

    }

    public abstract Vector3 SpawnToPlatform(float scale);
    public abstract void InsertToInnerStructure(GameObject obj);
    






    // Use this for initialization
    //void Start () {}

    // Update is called once per frame   
    //void Update () {}
}


// todo change rotations to objects native rotations ?
// check if monoBehaviour is needed? propably if we need want to give them as list
// give position where to look at
