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
    public const uint NumWidhtGrids = 10;

    [SerializeField]
    [Range (10 , 100)]
    public const uint NumHeightGrids = 10;  // const cant be seriliazed(reeeeeeeeeeeee)
    [SerializeField]
    Vector3 WorldPos = new Vector3(0, 0, 0);

    [SerializeField]
    [Range(0.5f, 10f)]
    float CharacterRadius = 1f;

    [SerializeField]
    const float maxAcceleration = 0.1f;

    [SerializeField]
    Vector2 StagePosition = new Vector2(0, 0);

    [SerializeField]
    float Friction = 0.99f;

    //[SerializeField]
    //float Rotation = 0;

    float FastRadius = 0;
    // helper class so we can create reference array to this
    // prehaps move velocity and acceleration somewhere else?
    class Collider
    {
        public GameObject Object = null;
        public int X = 0; 
        public int Y = 0;
        public Vector2 velocity = new Vector2( 0, 0); 
        public Vector2 acceleration = new Vector2( 0, 0);
    }

    private List<Collider>[,] Grid = new List<Collider>[NumHeightGrids, NumWidhtGrids];
    List<Collider> Colliders = new List<Collider>();

    void Start () {
        FastRadius = CharacterRadius * CharacterRadius;
        for (int y = 0; y < NumHeightGrids; y++)
        {
            for (int x = 0; x < NumWidhtGrids; x++)
            {
                Grid[y, x] = new List<Collider>();
            }
        }
	}

    struct CollisionTable
    {
        public Collider a;
        public Collider b;
    }
    // Update is called once per frame

    bool CircleCollisionXZPlane(GameObject a , GameObject b)
    {
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
                if (Object.ReferenceEquals( Grid[coll.Y, coll.X][i] , coll.Object))
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


	void FixedUpdate () {
        // here update grid and construct the collision table 
        List<CollisionTable> CollisionBuffer = new List<CollisionTable>();
        for (int i = 0; i < Colliders.Count; i++ )// Collider current in Colliders) // construct Collision table
        {
            Collider current = Colliders[i];
            for (int i2 = -1; i2 < 2; i2++)
            {
                if (current.X - 1 < 0 || current.X >= NumWidhtGrids) continue;
                if ((current.X - 1 < 0) == true)
                {
                    Debug.Log("Dont be here");
                    int error = 0;
                }
                
                if ((current.Y + i2 < 0 || current.Y + i2 >= NumHeightGrids) == true) continue;

                if (UpdatePosition(current) == true) continue; // if object gets new place in grid lets not check its collisions in this update

                if ((current.Y + i2 < 0 || current.X - 1 < 0 || current.Y + i2 >= NumHeightGrids || current.X >= NumWidhtGrids) == true)
                {
                    Debug.Log("Dont be here");
                    int error = 0;
                }
                List<Collider> tempList = Grid[current.Y + i2, current.X - 1];


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

                if (UpdatePosition(current)) continue; // if object gets new place in grid lets not check its collisions in this update

                List<Collider> tempList = Grid[current.Y + i2, current.X];

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

        for(int i = 0; i < CollisionBuffer.Count; i++) //handle all collisions 
        {
            Vector2 dist1 = new Vector2( CollisionBuffer[i].a.Object.transform.position.x , CollisionBuffer[i].a.Object.transform.position.z )- StagePosition; 
            Vector2 dist2 = new Vector2( CollisionBuffer[i].b.Object.transform.position.x , CollisionBuffer[i].b.Object.transform.position.z )- StagePosition;

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

            CollisionBuffer[i].b.acceleration += distvec; 
        }
        for (int i = 0; i < Colliders.Count;i++) // update positions
        {
            Colliders[i].velocity += Colliders[i].acceleration;
            Colliders[i].velocity.x *= Friction;
            Colliders[i].velocity.y *= Friction;
            Colliders[i].Object.transform.position += new Vector3(Colliders[i].velocity.x , 0 , Colliders[i].velocity.y);
             
        }
	}

    const float ADD_ON = 10000f;
    [SerializeField]
    const float GRID_SCALE = 5f;
    public void Insert(GameObject obj)
    {
        IntVec2 GridPos = GetPosition(obj);
        if (GridPos.x == -1 && GridPos.y == -1)
        {
            Debug.Log("Object inserting is not inside spatial grid!");
            return;
        }
        Collider temp = new Collider();
        temp.X = GridPos.x;
        temp.Y = GridPos.y;
        temp.Object = obj;

        Colliders.Add(temp);
        List<Collider> tempo = Grid[temp.Y , temp.Y];
        int k = tempo.Count;
        Grid[temp.Y, temp.X].Add(temp);
    }

    void OnDrawGizmos()
    {
        float halfDimX = ((float)NumWidhtGrids  * CharacterRadius * GRID_SCALE) / 2f;
        float halfDimY = ((float)NumHeightGrids * CharacterRadius * GRID_SCALE) / 2f;

        Gizmos.DrawLine(new Vector3( WorldPos.x  - halfDimX, WorldPos.y,WorldPos.y - halfDimY) , new Vector3(  WorldPos.x - halfDimX , WorldPos.y,WorldPos.z + halfDimY));
        Gizmos.DrawLine(new Vector3( WorldPos.x  - halfDimX, WorldPos.y,WorldPos.y + halfDimY) , new Vector3(  WorldPos.x + halfDimX ,WorldPos.y, WorldPos.z + halfDimY));
        Gizmos.DrawLine(new Vector3( WorldPos.x  + halfDimX, WorldPos.y,WorldPos.y + halfDimY) , new Vector3(  WorldPos.x + halfDimX ,WorldPos.y, WorldPos.z - halfDimY));
        Gizmos.DrawLine(new Vector3( WorldPos.x  + halfDimX, WorldPos.y,WorldPos.y - halfDimY) , new Vector3(  WorldPos.x - halfDimX , WorldPos.y,WorldPos.y - halfDimY));

        float oneGridWidht = CharacterRadius * GRID_SCALE;

        for (int i = 0; i < NumHeightGrids - 1; i++)
        {
            
            Gizmos.DrawLine(new Vector3( WorldPos.x  - halfDimX, WorldPos.y,WorldPos.y - halfDimY + (oneGridWidht * (i  + 1))) , 
                new Vector3(  WorldPos.x + halfDimX , WorldPos.y,WorldPos.z - halfDimY + (oneGridWidht * (i  + 1))));
        }
         for (int i = 0; i < NumWidhtGrids - 1; i++)
        {
            
            Gizmos.DrawLine(new Vector3( WorldPos.x  - halfDimX + (oneGridWidht * (i  + 1)) , WorldPos.y,WorldPos.y - halfDimY) , 
                new Vector3(  WorldPos.x - halfDimX + (oneGridWidht * (i  + 1)), WorldPos.y,WorldPos.z + halfDimY ));
        }
    }
    //public List<Vector3> GetDrawPoints()
    //{
    //    List<Vector3> points = new List<Vector3>();
    //    // outer area 

    //    Vector3[] p = new Vector3[4];
    //    p[0] = new Vector3(WorldPos.x - halfDimX, WorldPos.y,WorldPos.y - halfDimY);
    //    p[1] = new Vector3(WorldPos.x - halfDimX, WorldPos.y,WorldPos.y + halfDimY);
    //    p[2] = new Vector3(WorldPos.x + halfDimX, WorldPos.y,WorldPos.y + halfDimY);
    //    p[3] = new Vector3(WorldPos.x + halfDimX, WorldPos.y,WorldPos.y - halfDimY);

    //    points.Add(p[0]);
    //    points.Add(p[1]);
    //    points.Add(p[1]);
    //    points.Add(p[2]);
    //    points.Add(p[2]);
    //    points.Add(p[3]);
    //    points.Add(p[3]);
    //    points.Add(p[0]);

    //    return points;
    //}
}
 // TODO 
 //do corrent axis
 // how to refence box inside of the grid
 // todo rotation?
 // object pool for colliders?
 // GET ARENA FROM DRIVE
 // Test should i neg or add the friction
 // clean up comments  
 // fix checked area