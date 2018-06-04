using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlatform : Platforms  {




    [SerializeField]
    Vector2 StageDimension = new Vector2(100, 100);
    [SerializeField]
    float MinRangeFromStage = 10;


    private Vector2 stagePosition;
    private float VerticalScale = 0;

	// Use this for initialization, 
	void Start () {
		stagePosition = new Vector2(base.Position.x, base.Position.y - (StageDimension.y / 2f));
        VerticalScale = StageDimension.x / (StageDimension.y * 2f);
	}
	
	// Update is called once per frame
	//void Update () {}


    public override Vector3 SpawnToPlatform(float scale)
    {
 
        float maxpos = scale * StageDimension.y; // todo fix to correct scale
        maxpos = maxpos < MinRangeFromStage ? MinRangeFromStage : maxpos;

        float realpos = Random.Range(0f, maxpos);
        float axis = Random.Range(0f, Mathf.PI);

        
        float x = Mathf.Cos(axis) * realpos;
        float y = Mathf.Sin(axis) * realpos;

        Vector3 pos = new Vector3(x * VerticalScale,y,0) ; //
        pos += (Vector3)stagePosition; //
                            

        return pos;
    }


    void OnDrawGizmos()
    {
        Vector2 halfDims = StageDimension / 2f;
        Gizmos.DrawLine(new Vector2( Position.x  - halfDims.x, Position.y - halfDims.y) , new Vector2(  Position.x - halfDims.x , Position.y + halfDims.y));
        Gizmos.DrawLine(new Vector2( Position.x  - halfDims.x, Position.y + halfDims.y) , new Vector2(  Position.x + halfDims.x , Position.y + halfDims.y));
        Gizmos.DrawLine(new Vector2( Position.x  + halfDims.x, Position.y + halfDims.y) , new Vector2(  Position.x + halfDims.x , Position.y - halfDims.y));
        Gizmos.DrawLine(new Vector2( Position.x  + halfDims.x, Position.y - halfDims.y) , new Vector2(  Position.x - halfDims.x , Position.y - halfDims.y));




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