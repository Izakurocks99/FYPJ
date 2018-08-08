using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinematicCamera : MonoBehaviour {
    [SerializeField]
    List<Vector3> WayPoints = new List<Vector3>();
    [SerializeField]
    Vector3 LookAt = new Vector3(0,0,0);
	// Use this for initialization
	void OnEnable () {
	    Debug.Assert(WayPoints.Count > 1);
        transform.position = WayPoints[0];
	}
	
    [SerializeField]
    float MaxSpeed = 1f;
    [SerializeField]
    float Acceleration = 0.1f;
    
    float speed = 0;

    int currentIndex = 1;
    bool running = true;

    Vector3 myForce = new Vector3(0,0,0);
	void Update () {
	    if(running) 
        {
            if(Vector3.Magnitude(transform.position - WayPoints[currentIndex]) < 1f)
            {
                currentIndex++;
                if(currentIndex >= WayPoints.Count)
                {
                    //running = false;
                    currentIndex = 0;
                    //return;
                }
            }

            Vector3 CurrentLine = WayPoints[currentIndex] - transform.position;
            CurrentLine.Normalize();
            CurrentLine *= Acceleration;
            myForce += CurrentLine * Time.deltaTime;
            myForce = Vector3.ClampMagnitude(myForce, MaxSpeed);
            transform.position += myForce * Time.deltaTime;

            transform.LookAt(LookAt);
        }
	}
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach(Vector3 i in WayPoints) 
        {
            Gizmos.DrawSphere(i,1);
        }
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(LookAt,1);
    }
}
