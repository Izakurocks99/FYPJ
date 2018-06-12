using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlatform : Platforms  { // inhereted monobehaviour

    [SerializeField]
    Vector2 StageDimension = new Vector2(100, 100);
    [SerializeField]
    float MinRangeFromStage = 10;


    private Vector2 stagePosition;
    private float VerticalScale = 0;

    [SerializeField]
    GameObject CollisionGridPreFab = null;

    private SpatialPartition Grid = null;
    // Use this for initialization, 
	void Awake () {                                                                 // awake vs start?
        //int hash = this.gameObject.GetHashCode();
		stagePosition = new Vector2(base.Position.x, base.Position.y - (StageDimension.y / 2f));
        VerticalScale = StageDimension.x / (StageDimension.y * 2f);
        Debug.Assert(CollisionGridPreFab);
        GameObject temp = GameObject.Instantiate(CollisionGridPreFab);
        temp.SetActive(true);
        Grid = temp.GetComponent<SpatialPartition>();
        Debug.Assert(Grid);

        Debug.Log(string.Format( "Platform {0}", this.gameObject.GetHashCode()));
	}

    // Update is called once per frame
    //void Update () {}

    //float testpos = -10;
    public override Vector3 SpawnToPlatform(float scale)
    {
 
        float maxpos = scale * StageDimension.y; // todo fix to correct scale
        maxpos = maxpos < MinRangeFromStage ? MinRangeFromStage : maxpos;

        float realpos = Random.Range(0f, maxpos);
        float axis = Random.Range(0f, Mathf.PI);

        
        float x = Mathf.Cos(axis) * realpos;
        float y = Mathf.Sin(axis) * realpos;

        Vector3 pos = new Vector3(x * VerticalScale,0,y) ; //
        pos += new Vector3(stagePosition.x, Position.z,stagePosition.y);

        //pos.z = testpos;
        //testpos += 0.5f;
        //pos.x = 0;
        //pos.y = 0;
        return pos;
    }
    override public void InsertToInnerStructure(GameObject obj)
    {
        //int hash = this.gameObject.GetHashCode();
        Grid.Insert(obj);
    }

    void OnDrawGizmos()
    {
        Vector2 halfDims = StageDimension / 2f;
        Gizmos.DrawLine(new Vector3( Position.x  - halfDims.x, Position.z,Position.y - halfDims.y) , new Vector3(  Position.x - halfDims.x , Position.z,Position.y + halfDims.y));
        Gizmos.DrawLine(new Vector3( Position.x  - halfDims.x, Position.z,Position.y + halfDims.y) , new Vector3(  Position.x + halfDims.x ,Position.z, Position.y + halfDims.y));
        Gizmos.DrawLine(new Vector3( Position.x  + halfDims.x, Position.z,Position.y + halfDims.y) , new Vector3(  Position.x + halfDims.x ,Position.z, Position.y - halfDims.y));
        Gizmos.DrawLine(new Vector3( Position.x  + halfDims.x, Position.z,Position.y - halfDims.y) , new Vector3(  Position.x - halfDims.x , Position.z,Position.y - halfDims.y));



        //List<Vector3> points = CollisionGrid.GetDrawPoints();

        //for (int i = 0; i < points.Count; i += 2)
        //{
        //    Gizmos.DrawLine(points[i] , points[i+1]);
        //}

        //Vector2[] points = new Vector2[4];


        //points[0] = new Vector2(Position.x - halfDims.x, Position.y - halfDims.y);
        //points[1]= new Vector2(Position.x - halfDims.x, Position.y + halfDims.y);
        //points[2]= new Vector2(Position.x + halfDims.x, Position.y + halfDims.y);
        //points[3]= new Vector2(Position.x - halfDims.x, Position.y + halfDims.y);

        //for (int i = 0; i < 4; i++)
        //{
        //    debug_points.Add(points[i]); 
        //}

    }
}

// TODO
// do random position implementation