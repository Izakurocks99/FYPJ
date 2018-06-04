using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialPartition/* : MonoBehaviour*/ {

	// Use this for initialization
    [SerializeField]
    [Range (10 , 100)]
    const uint NumWidhtGrids = 10;

    [SerializeField]
    [Range (10 , 100)]
    const uint NumHeightGrids = 10;

    [SerializeField]
    Vector3 WorldPos = new Vector3(0, 0, 0);

    [SerializeField]
    [Range(0.5f, 10f)]
    const float CharacterRadius = 1f;



    // helper class so we can create reference array to this
    class Collider
    {
        public GameObject Object = null;
        public int X = 0; 
        public int Y = 0; 
    }

    private List<Collider>[,] Grid = new List<Collider>[NumHeightGrids, NumWidhtGrids];
    List<Collider> Colliders = new List<Collider>();

	public SpatialPartition () {
	}
	
	// Update is called once per frame
	void Update () {
        // here update grid and construct the collision table 

        for (int y = 0; y < NumHeightGrids; y++)
        {
            for (int x = 0; x < NumWidhtGrids; x++)
            {
                             
            }
        }
	}

    const float ADD_ON = 10000f;
    [SerializeField]
    const float GRID_SCALE = 1.5f;
    void Insert(GameObject obj)
    {
        float DimX = (float)NumWidhtGrids * CharacterRadius * GRID_SCALE;
        float DimY = (float)NumHeightGrids * CharacterRadius * GRID_SCALE;

        float halfDimX = ((float)NumWidhtGrids  / 2f) * CharacterRadius;
        float halfDimY = ((float)NumHeightGrids / 2f) * CharacterRadius;
        // adding is done to ensure that we stay in positive side of coordinate system
        WorldPos.x += ADD_ON;
        WorldPos.y += ADD_ON;
        WorldPos.z += ADD_ON;

        Vector3 Corner = new Vector3(WorldPos.x - halfDimX,0 ,WorldPos.z - halfDimY );
        Vector3 ObjPos = new Vector3(ADD_ON, ADD_ON, ADD_ON) + obj.transform.position;
        Vector3 ToCorner = ObjPos - Corner;

        // check if object is inside of the box
        if (ToCorner.x >= 0 && ToCorner.z <= 0 && ToCorner.x <= DimX && ToCorner.z <= DimY)
        {
            Collider temp = new Collider();
            temp.X = Mathf.FloorToInt(ToCorner.x * CharacterRadius * GRID_SCALE);
            temp.Y = Mathf.FloorToInt(ToCorner.z * CharacterRadius * GRID_SCALE);

            temp.Object = obj;
            //Grid[posX , posY].
        }
        else
        {
            Debug.Log("Object inserting is not inside spatial grid!");
            return;
        }

        
        





        WorldPos.x -= 10000f;
        WorldPos.y -= 10000f;
        WorldPos.z -= 10000f;


    }
}
 // TODO 
 //do corrent axis
 // how to refence box inside of the grid