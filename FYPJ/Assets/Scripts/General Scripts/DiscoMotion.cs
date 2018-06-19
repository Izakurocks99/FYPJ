using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoMotion : MonoBehaviour {

    public GameObject _goAudio;
    Transform _transform;
    Vector3 _vec3Velocity;
    Vector3 _vec3Previous;
    float _floatTime;
    float _floatWait;
    bool _blFlip;

    void Start () {
        _vec3Previous = new Vector3(0, 6.45f, 2.75f);
        _floatTime = 0.0f;
        _floatWait = 2.0f;
        _blFlip = false;
        _transform = this.transform;
    }
	
	void Update () {
        if (_goAudio.GetComponent<AudioSource>().clip != null &&
            _goAudio.GetComponent<AudioSource>().isPlaying == true) {
                
            if ((_floatTime += 1 * Time.deltaTime) > _floatWait)
            {
                _blFlip = !_blFlip;
                _floatTime = 0;
            }
            if (_blFlip == true)
            _transform.Rotate(new Vector3(5, 0, 5));
            else
            _transform.Rotate(new Vector3(0, 5, 5));
        
            TransitDiscoBall();
        }
    }

    void TransitDiscoBall()
    {
        if (_transform.position != _vec3Previous)
        {
            _transform.position = Vector3.SmoothDamp(_transform.position, _vec3Previous, ref _vec3Velocity, 0.5f);
        }
        else
        {
            float _ftX = Random.Range(-4, 4);
            float _ftY = Random.Range(6.45f, 7f);
            float _ftZ = Random.Range(2, 3.5f);
            Vector3 _vec3Current = new Vector3(_ftX, _ftY, _ftZ);
            _vec3Previous = _vec3Current;
        }
    }
}
