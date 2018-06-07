//#define SPATIAL_DEBUG 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct IntVec2
{
    public IntVec2(int _a,int _b)
    {
        x = _a;
        y = _b;
    }
    public int x;
    public int y;
};

public sealed class SpatialPartition : MonoBehaviour { // can we work without monobehaviour?

	// Use this for initialization
    [SerializeField]
    [Range (10 , 100)]
    public const uint NumWidhtGrids = 100;

    [SerializeField]
    [Range (10 , 100)]
    public const uint NumHeightGrids = 50;  // const cant be seriliazed(reeeeeeeeeeeee)
    [SerializeField]
    Vector3 WorldPos = new Vector3(0, 0, 0);

    [SerializeField]
    [Range(0.5f, 10f)]
    float CharacterRadius = 1f;

    [SerializeField]
    const float maxAcceleration = 0.002f;

    [SerializeField]
    Vector2 StagePosition = new Vector2(0, 0);

    [SerializeField]
    float Friction = 0.00f;

    //[SerializeField]
    //float Rotation = 0;

    float FastRadius = 0;

    List<Collider> InActiveColliders = new List<Collider>(new Collider[10000]);

    // helper class so we can create reference array to this
    // prehaps move velocity and acceleration somewhere else?
    class Collider
    {
        public GameObject Object = null;
        public int X = 0; 
        public int Y = 0;
        public Vector2 velocity = new Vector2( 0, 0); 
        public Vector2 acceleration = new Vector2( 0, 0);
        public bool Active = false;
    }

    private List<Collider>[,] Grid = new List<Collider>[NumHeightGrids, NumWidhtGrids];
    List<Collider> Colliders = new List<Collider>();

    void Awake () {
        Debug.Log(string.Format( "Spatial {0}", this.gameObject.GetHashCode()));

        FastRadius = CharacterRadius * CharacterRadius;
        for (int y = 0; y < NumHeightGrids; y++)
        {
            for (int x = 0; x < NumWidhtGrids; x++)
            {
                Grid[y, x] = new List<Collider>();
            }
        }
        
        //int teeest = InActiveColliders.Count;
        for (int i = 0; i < InActiveColliders.Count; i++)
        {
            InActiveColliders[i] = new Collider();
        }
	}

    struct CollisionTable
    {
        public Collider a;
        public Collider b;
    }
#if (SPATIAL_DEBUG)
    int numCollisionsChecked = 0;
#endif
    bool CircleCollisionXZPlane(GameObject a , GameObject b)
    {
#if (SPATIAL_DEBUG)
        numCollisionsChecked++;
#endif
        Vector2 dist = new Vector2(a.transform.position.x - b.transform.position.x, a.transform.position.z - b.transform.position.z);
        if (dist.SqrMagnitude() < FastRadius) return true;
        return false;
    }


    bool UpdatePosition(Collider coll) // if false position changed
    {
        IntVec2 newPos = GetPosition(coll.Object);
        
        if (newPos.x != coll.X || newPos.y != coll.Y) // move position in grid
        {

            for (int i = 0; i < Grid[coll.Y, coll.X].Count; i++)
            {
                if (Object.ReferenceEquals(Grid[coll.Y, coll.X][i].Object, coll.Object))
                {
                    Grid[coll.Y, coll.X].FastDelete(i);
                    break;
                }
            }
            if ((newPos.y != -1 && newPos.x != -1) == true) // invalid position 
            {
                coll.X = newPos.x;
                coll.Y = newPos.y;
                Grid[coll.Y, coll.X].Add(coll);
            }
            else
            {
                coll.Active = false;
            }
            return true;
        }
        return false;
    }

    IntVec2 GetPosition(GameObject obj)
    {
        float DimX = (float)NumWidhtGrids * CharacterRadius * GRID_SCALE;
        float DimY = (float)NumHeightGrids * CharacterRadius * GRID_SCALE;

        float halfDimX = DimX / 2f ;
        float halfDimY = DimY / 2f;
        // adding is done to ensure that we stay in positive side of coordinate system
        WorldPos.x += ADD_ON;
        WorldPos.y += ADD_ON;
        WorldPos.z += ADD_ON;

        Vector3 Corner = new Vector3(WorldPos.x - halfDimX,0 ,WorldPos.z - halfDimY );
        Vector3 ObjPos = new Vector3(ADD_ON, ADD_ON, ADD_ON) + obj.transform.position;
        Vector3 ToCorner = ObjPos - Corner;

        float oneGridWidht = CharacterRadius * GRID_SCALE;
        // check if object is inside of the box
        IntVec2 ret = new IntVec2(-1, -1);
        if ((ToCorner.x >= 0 && ToCorner.z >= 0 && ToCorner.x <= DimX && ToCorner.z <= DimY) == true)
        {
            ret.x = Mathf.FloorToInt(ToCorner.x / oneGridWidht);
            ret.y = Mathf.FloorToInt(ToCorner.z / oneGridWidht);
        }
        WorldPos.x -= ADD_ON;
        WorldPos.y -= ADD_ON;
        WorldPos.z -= ADD_ON;
        return ret;
    }


#if (SPATIAL_DEBUG)
    List<IntVec2> checkedPositions = new List<IntVec2>();
#endif
    void FixedUpdate () {
#if (SPATIAL_DEBUG)
        bool addposes = checkedPositions.Count == 0;
#endif
        // here update grid and construct the collision table 
        Vector3 midpos = new Vector3(0, 0, 0);
        List<CollisionTable> CollisionBuffer = new List<CollisionTable>();
        for (int i = 0; i < Colliders.Count; i++ )// Collider current in Colliders) // construct Collision table
        {
            Collider current = Colliders[i];
            midpos += current.Object.transform.position;
                if (UpdatePosition(current) == true) continue; // if object gets new place in grid lets not check its collisions in this update
            for (int i2 = -1; i2 < 2; i2++)
            {
                if (current.X - 1 < 0 || current.X >= NumWidhtGrids) continue;
#if (SPATIAL_DEBUG)
                if ((current.X - 1 < 0) == true)
                {
                    Debug.Log("Dont be here");
                    ///int error = 0;
                }
#endif
                
                if ((current.Y + i2 < 0 || current.Y + i2 >= NumHeightGrids) == true) continue;


#if (SPATIAL_DEBUG)
                if ((current.Y + i2 < 0 || current.X - 1 < 0 || current.Y + i2 >= NumHeightGrids || current.X >= NumWidhtGrids) == true)
                {
                    Debug.Log("Dont be here");
                    //int error = 0;
                }
#endif
                List<Collider> tempList = Grid[current.Y + i2, current.X - 1];

#if (SPATIAL_DEBUG)
                if (addposes)
                {
                IntVec2 t = new IntVec2();
                t.x = current.X - 1;
                t.y = current.Y + i2;
                    checkedPositions.Add(t);
                }
#endif
                for (int i3 = 0; i3 < tempList.Count; i3++)
                {
                    if (!Object.ReferenceEquals(tempList[i3].Object, current.Object)  && CircleCollisionXZPlane(tempList[i3].Object, current.Object)) // collides?
                    {
                        bool Handled = false;
                        for (int i4 = 0; i4 < CollisionBuffer.Count; i4++) // check if collision is already handled
                        {
                            if (Object.ReferenceEquals(CollisionBuffer[i4].a, current.Object))
                            {
                                Handled = true;
                                break;
                            }
                        }
                        if (!Handled)
                        {
                            CollisionTable col = new CollisionTable();
                            col.a = tempList[i3];//.Object;
                            col.b = current;//.Object;
                            CollisionBuffer.Add(col);
                        }
                    }
                }


            } 
            for (int i2 = 0; i2 < 2; i2++) // current grid and one above, same check
            {
                if (current.X < 0|| current.X >= NumWidhtGrids) continue;

                if (current.Y + i2 < 0 || current.Y + i2 >= NumHeightGrids) continue;


                List<Collider> tempList = Grid[current.Y + i2, current.X];
#if (SPATIAL_DEBUG)
                if (addposes)
                {
                IntVec2 t = new IntVec2();
                t.x = current.X;
                t.y = current.Y + i2;
                    checkedPositions.Add(t);
                }
#endif
                for (int i3 = 0; i3 < tempList.Count; i3++)
                {
                    if (!Object.ReferenceEquals(tempList[i3].Object, current.Object)  && CircleCollisionXZPlane(tempList[i3].Object, current.Object)) // collides?
                    {
                        bool Handled = false;
                        for (int i4 = 0; i4 < CollisionBuffer.Count; i4++) // check if collision is already handled
                        {
                            if (Object.ReferenceEquals(CollisionBuffer[i4].a, current.Object))
                            {
                                Handled = true;
                                break;
                            }
                        }
                        if (!Handled)
                        {
                            CollisionTable col = new CollisionTable();
                            col.a = tempList[i3];//.Object;
                            col.b = current;//.Object;
                            CollisionBuffer.Add(col);
                        }
                    }
                }


            } 
        }

        if (midpos.x > 0 || midpos.z > 0)
        {
            midpos /= Colliders.Count;
        }
        for(int i = 0; i < CollisionBuffer.Count; i++) //handle all collisions 
        {
            Vector2 dist1 = new Vector2( CollisionBuffer[i].a.Object.transform.position.x , CollisionBuffer[i].a.Object.transform.position.z )- StagePosition; 
            Vector2 dist2 = new Vector2( CollisionBuffer[i].b.Object.transform.position.x , CollisionBuffer[i].b.Object.transform.position.z )- StagePosition;
            Vector2 fromMid = new Vector2(CollisionBuffer[i].a.Object.transform.position.x, CollisionBuffer[i].a.Object.transform.position.z) - new Vector2( midpos.x , midpos.z);

            if (dist1.SqrMagnitude() > dist2.SqrMagnitude()) // a should be closer and it pushes b
            {
                CollisionTable n = new CollisionTable();
                n.a = CollisionBuffer[i].b;
                n.b = CollisionBuffer[i].a;
                CollisionBuffer[i] = n;
            }

            Vector2 distvec = new Vector2(CollisionBuffer[i].b.Object.transform.position.x - CollisionBuffer[i].a.Object.transform.position.x ,
                CollisionBuffer[i].b.Object.transform.position.z - CollisionBuffer[i].a.Object.transform.position.z);  

            distvec.Normalize();
            distvec.x *= maxAcceleration;
            distvec.y *= maxAcceleration;
            fromMid.Normalize();
            
            fromMid.x *= maxAcceleration * 0.4f;
            fromMid.y *= maxAcceleration * 0.4f;
            distvec += fromMid;
            CollisionBuffer[i].b.acceleration += distvec; 
        }


        for (int i = 0; i < Colliders.Count;i++) // update positions
        {
            if ((Colliders[i].Active == false || Colliders[i].Object.activeInHierarchy == false) == true)
            {
                Collider t = Colliders.PopAt(i);
                t.Active = false;
                InActiveColliders.Add(t);
                if (!t.Object.activeInHierarchy)
                {
                    // find it from the grid and remove it
                    List<Collider> tempList = Grid[t.Y, t.X];
                    for (int i2 = 0; i2 < tempList.Count; i2++)
                    {
                        if (!tempList[i2].Object.activeInHierarchy)
                        {
                             tempList.FastDelete(i2);
                             break;
                        }
                    }
                }
                continue;
            }
            Colliders[i].velocity += Colliders[i].acceleration;
            Colliders[i].velocity.x *= Friction;
            Colliders[i].velocity.y *= Friction;
            Colliders[i].Object.transform.position += new Vector3(Colliders[i].velocity.x , 0 , Colliders[i].velocity.y);
            Colliders[i].acceleration *= 0;
        }
#if (SPATIAL_DEBUG)
        Debug.Log(string.Format("num checks {0} ", numCollisionsChecked));
        numCollisionsChecked = 0;
#endif
	}

    const float ADD_ON = 10000f;
    [SerializeField]
    const float GRID_SCALE = 1.4f;
    int debuginsertions = 0;
    public void Insert(GameObject obj)
    {
        IntVec2 GridPos = GetPosition(obj);
        if (GridPos.x == -1 && GridPos.y == -1)
        {

#if (SPATIAL_DEBUG)
            Debug.Log("Object inserting is not inside spatial grid!");
#endif
            return;
        }
        Collider temp = InActiveColliders.PopBack();
        temp.X = GridPos.x;
        temp.Y = GridPos.y;
        temp.Object = obj;
        temp.Active = true;
        Colliders.Add(temp);
        List<Collider> tempo = Grid[temp.Y , temp.Y];
        int k = tempo.Count;
        Grid[temp.Y, temp.X].Add(temp);
        debuginsertions++;
    }

    void OnDrawGizmos()
    {
        float halfDimX = ((float)NumWidhtGrids  * CharacterRadius * GRID_SCALE) / 2f;
        float halfDimY = ((float)NumHeightGrids * CharacterRadius * GRID_SCALE) / 2f;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(new Vector3( WorldPos.x  - halfDimX, WorldPos.y,WorldPos.z - halfDimY) , new Vector3(  WorldPos.x - halfDimX , WorldPos.y,WorldPos.z + halfDimY));
        Gizmos.DrawLine(new Vector3( WorldPos.x  - halfDimX, WorldPos.y,WorldPos.z + halfDimY) , new Vector3(  WorldPos.x + halfDimX ,WorldPos.y, WorldPos.z + halfDimY));
        Gizmos.DrawLine(new Vector3( WorldPos.x  + halfDimX, WorldPos.y,WorldPos.z + halfDimY) , new Vector3(  WorldPos.x + halfDimX ,WorldPos.y, WorldPos.z - halfDimY));
        Gizmos.DrawLine(new Vector3( WorldPos.x  + halfDimX, WorldPos.y,WorldPos.z - halfDimY) , new Vector3(  WorldPos.x - halfDimX , WorldPos.y,WorldPos.z - halfDimY));

        float oneGridWidht = CharacterRadius * GRID_SCALE;

        for (int i = 0; i < NumHeightGrids - 1; i++)
        {
            
            Gizmos.DrawLine(new Vector3( WorldPos.x  - halfDimX, WorldPos.y,WorldPos.z - halfDimY + (oneGridWidht * (i  + 1))) , 
                new Vector3(  WorldPos.x + halfDimX , WorldPos.y,WorldPos.z - halfDimY + (oneGridWidht * (i  + 1))));
        }
         for (int i = 0; i < NumWidhtGrids - 1; i++)
        {
            
            Gizmos.DrawLine(new Vector3( WorldPos.x  - halfDimX + (oneGridWidht * (i  + 1)) , WorldPos.y,WorldPos.z - halfDimY) , 
                new Vector3(  WorldPos.x - halfDimX + (oneGridWidht * (i  + 1)), WorldPos.y,WorldPos.z + halfDimY ));
        }

#if (SPATIAL_DEBUG)
        Gizmos.color = Color.green;
        Vector3 Corner = WorldPos - new Vector3(halfDimX ,0 , halfDimY);
        for (int i = 0; i < checkedPositions.Count; i++)
        {

            Vector3 pos = Corner + new Vector3(oneGridWidht * checkedPositions[i].x + 0.5f, 0, oneGridWidht * checkedPositions[i].y + 0.5f);
            Gizmos.DrawSphere(pos, 1f); 
        }
        checkedPositions.Clear();
#endif
    }
    
}
 // TODO 
 // todo rotation?
 // object pool for colliders?
 // GET ARENA FROM DRIVE
 // clean up comments  
 // fix checked area