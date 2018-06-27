using UnityEngine;
using System.Collections;

public class AudioVizualiser : MonoBehaviour {

    public GameObject _SampleCubePrefab;
    GameObject[] _sampleCube = new GameObject[Audio._barLenght];
    public float _height, _minimumHeight;
    public Vector3 _direction;
    public float _stepDistance;
    public float _scale;
    Vector3 _position;
    Material _mt;
    public Gradient _colorGradient;


	void Start ()
    {
        _direction = _direction.normalized;
        _position = this.transform.position;

	    for(int i =0; i<_sampleCube.Length; i++) {
            GameObject _instanceSampleCube = (GameObject)Instantiate(_SampleCubePrefab);
            _instanceSampleCube.transform.position = this.transform.position;
            _instanceSampleCube.transform.parent = this.transform;
            _instanceSampleCube.name = "SampleCune" + i;
            _instanceSampleCube.transform.position = _position;
            _position += _direction * _stepDistance;
            _sampleCube[i] = _instanceSampleCube;

        }
	}
	
	
	void Update ()
    {
	    for(int i = 0; i < _sampleCube.Length; i++)
        {
            if (_sampleCube != null)
            {
                _sampleCube[i].transform.localScale = new Vector3(_scale, (Audio._bandBuffer[i] * _height) + _minimumHeight, _scale);
                Color _color = _colorGradient.Evaluate(Audio._bandBuffer[i]);
                _mt = _sampleCube[i].GetComponentInChildren<MeshRenderer>().materials[0];
                _mt.SetColor("_EmissionColor", _color);
            }
        }
	}
}
