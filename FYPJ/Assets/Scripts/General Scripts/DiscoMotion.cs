using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoMotion : MonoBehaviour {

    public GameObject _goAudio;
    public GameObject _goChild;
    Transform _transform;
    public Vector3 _vec3Spawn;
    /*
    Vector3 _vec3Velocity;
    Vector3 _vec3Previous;
    float _floatTime;
    float _floatWait;
    bool _blFlip;
    */
    [SerializeField]
    float RotationSpeed = 100f;
    [SerializeField]
    float RotationStrenght = 1f;

    //[SerializeField]
    //float MovementScale = 1f;

    [SerializeField]
    Vector3 ZeroPosition = new Vector3(0,0,0);
    [SerializeField]
    Vector3 Dimensions = new Vector3(0,0,0);
    [SerializeField]
    float MovementSpeed = 1f;
    //[SerializeField]
    //float DirectionFreq = 1f;

    void Start () {
        _transform = this.transform;
        //_vec3Previous = new Vector3(0, 6.45f, 2.75f);
        //_floatTime = 0.0f;
        //_floatWait = 1.0f;
        //_blFlip = false;
    }
	
	void Update () {
        //if (_goAudio.GetComponent<AudioSource>().clip != null &&
          //  _goAudio.GetComponent<AudioSource>().isPlaying == true) {
                
            float currentSpeed = RotationStrenght * Time.time;
            float xrot = Mathf.PerlinNoise(currentSpeed , 0) * 2 - 1;
            float yrot = Mathf.PerlinNoise(0,currentSpeed) * 2 - 1;

            
            xrot *= 2 * Mathf.PI * RotationSpeed;
            yrot *= 2 * Mathf.PI * RotationSpeed;
            Vector2 rots = new Vector2(xrot,yrot);
            //rots.Normalize();
            //rots *= RotationSpeed * Time.deltaTime;
            Quaternion q = new Quaternion(rots.x,rots.y,0,1);
            //q.Normalize();
            //Quaternion.
            //Quaternion.Normalize(q);
            //q.Normalize();
            _goChild.transform.rotation *= (q);// * RotationSpeed);
            /*
            if ((_floatTime += 1 * Time.deltaTime) > _floatWait)
            {
                _blFlip = !_blFlip;
                _floatTime = 0;
            }
            if (_blFlip == true)
                _goChild.transform.Rotate(new Vector3(7, 0, 7) * Time.deltaTime * RotationSpeed);
            else
                _goChild.transform.Rotate(new Vector3(0, 7, 7) * Time.deltaTime * RotationSpeed);
        
             * */
            TransitDiscoBall();

            _vec3Spawn = _goChild.transform.position;
        //}
    }

    void TransitDiscoBall()
    {

        float step = Time.time * MovementSpeed;
        float nx = (Mathf.PerlinNoise(step,0) * 2 - 1 )* Dimensions.x;
        float ny = (Mathf.PerlinNoise(0,step)* 2 - 1) * Dimensions.y;
        float nz = (Mathf.PerlinNoise(step,step)* 2 - 1) * Dimensions.z;
        Vector3 npos = new Vector3(nx,ny,nz);
        //npos.Normalize();
        //npos = npos * Dimensions;

        _goChild.transform.position = ZeroPosition + npos;

        /*
        if (_goChild.transform.position != _vec3Previous)
            _goChild.transform.position = Vector3.SmoothDamp(_goChild.transform.position, _vec3Previous, ref _vec3Velocity, 0.3f);
        else
        {
            float _ftX = Random.Range(-0.75f, 0.75f);
            float _ftY = Random.Range(6.45f, 7f);
            float _ftZ = Random.Range(2, 3.5f);
            Vector3 _vec3Current = new Vector3(_ftX, _ftY, _ftZ);
            _vec3Previous = _vec3Current;
        }
        */
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawCube(ZeroPosition,Dimensions);
        //Vector3 corner1 = ZeroPosition + Dimensions;
        //Vector3 corner2 = ZeroPosition - Dimensions;
        //Gizmos.DrawLine()    
    }
}
