using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidePlatform : Platforms { // inhereted monobehaviour

    // Use this for initialization
    [SerializeField]
    Vector2 Dimensions  = new Vector3();
    [SerializeField]
    float   Rotation    = 0;
    float   RealRot     = 0;
	void Awake () {
        RealRot =  Rotation * Mathf.Deg2Rad;
	}
	
    public override Vector3 SpawnToPlatform(float scale)
    {
        float halfX = Dimensions.x / 2f;
        float halfy = Dimensions.y / 2f;
        float RandomX = Random.Range(-halfX, halfX);
        float RandomY = Random.Range(-halfy, halfy);

        Vector3 pos = new Vector3(0, 0, 0);
        pos.x = Mathf.Cos(RealRot) * RandomX + (Mathf.Sin(RealRot) * RandomY) * -1;
        pos.z = Mathf.Sin(RealRot) * RandomX + (Mathf.Cos(RealRot) * RandomY);

        pos += base.Position;
        return pos;
    }
    public override void InsertToInnerStructure(GameObject obj)
    {
        return;
    }
    private void OnDrawGizmos()
    {
        float halfX = Dimensions.x / 2f;
        float halfy = Dimensions.y / 2f;

        Vector3 p1 = new Vector3(Mathf.Cos(RealRot) * halfX + (Mathf.Sin(RealRot) * halfy) * -1,0,
            Mathf.Sin(RealRot) * halfX + (Mathf.Cos(RealRot) * halfy));
        Vector3 p2 = new Vector3(Mathf.Cos(RealRot) * -halfX + (Mathf.Sin(RealRot) * halfy) * -1,0,
            Mathf.Sin(RealRot) * -halfX + (Mathf.Cos(RealRot) * halfy));
        Vector3 p3 = new Vector3(Mathf.Cos(RealRot) * -halfX + (Mathf.Sin(RealRot) * -halfy) * -1,0,
            Mathf.Sin(RealRot) * -halfX + (Mathf.Cos(RealRot) * -halfy));
        Vector3 p4 = new Vector3(Mathf.Cos(RealRot) * halfX + (Mathf.Sin(RealRot) * -halfy) * -1,0,
            Mathf.Sin(RealRot) * halfX + (Mathf.Cos(RealRot) * -halfy));

        p1 += base.Position;
        p2 += base.Position;
        p3 += base.Position;
        p4 += base.Position;
        Gizmos.DrawLine(p1 , p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);

    }
}
