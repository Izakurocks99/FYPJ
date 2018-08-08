using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SpeakerBeatMotion : MonoBehaviour {

	Transform _tfCamera = null;
	//Transform _tfParent = null;
	Transform _tfThis;
    public float _intShiftRate;
    public float _ftTimeToTravel;
    float _ftTime;
	int _intNumber;
	Vector3 _vec3Shift;
	Vector3 _vec3Area;
    ControlCalibrationScript calibration;

    //void Start() {
    //	_tfThis = this.transform;
    //	_tfParent = this.transform.parent.transform;
    //	_tfCamera = Camera.main.transform;
    //	_vec3Area = (_tfCamera.position + _tfCamera.forward * 0.5f);
    //	_vec3Shift = new Vector3(Random.Range(-0.25f, 0.25f),
    //							 Random.Range(-0.25f, 0.25f),
    //							 Random.Range(-0.25f, 0.25f));
    //   }

    bool _die = false;
    List<GameObject> _home = null;
    List<Material> _materials = null;
    [SerializeField]
    [Range(0.1f, 1.5f)]
    float dissolveTime = 1f;
    Material myDissolveMaterial = null;
    Material myDefaultMaterial = null;
    float dissolveTimer = 0;

    SpeakerBeatCollisionScript collisionScript;

    public void PoolInit(List<GameObject> home, List<Material> mat)
    {
        if (!collisionScript)
            collisionScript = gameObject.GetComponent<SpeakerBeatCollisionScript>();

        dissolveTimer = 0;
        _home = home;
        _materials = mat;
        _die = false;

        _tfThis = this.transform;
        //_tfParent = this.transform.parent.transform;
        _tfCamera = Camera.main.transform;
        _vec3Shift = new Vector3(Random.Range(-0.125f, 0.125f),
                                 Random.Range(-0.125f, 0.125f),
                                 0.0f);
        calibration = FindObjectOfType<ControlCalibrationScript>();

        _vec3Area = (_tfCamera.position + (_tfCamera.forward * 0.5f)) + _vec3Shift;
    }

    void Update () {
		// _tfThis.Rotate(_tfThis.forward, 7.0f);
		
		BeatMotion();
        if (!_die)
        {
            // _vec3Area = new Vector3(0, 0, -7.0f);
            TransitBeat();
        }
        else
        {
            Dissolve();
        }
    }

	void BeatMotion() {
		_tfThis.position += new Vector3(0, Mathf.Sin(Time.time * _intShiftRate) * Time.deltaTime, 0);
	}

	void TransitBeat() {
        _ftTime += Time.deltaTime / _ftTimeToTravel;
        Vector3 _vec3Heading = _vec3Area - _tfThis.position;
        if (!(_vec3Heading.sqrMagnitude < 0.1f * 0.1f))
            _tfThis.position = Vector3.Lerp(_tfThis.transform.position, _vec3Area, _ftTime);
        else {
            _ftTime = 0.0f;
            collisionScript.Decay();
            OnReturn();
        }
    }

    public void Die()
    {
        if (_die) return;
        _die = true;
        Material temp = _materials.PopBack();
        Material current = this.GetComponent<Renderer>().material;
        myDefaultMaterial = current;
        string main = "_MainTex";
        string emis = "_Emissive";

        temp.SetTexture(main, current.GetTexture(main));
        temp.SetTexture(emis, current.GetTexture("_EmissionMap"));
        temp.SetFloat("_TreshHold", 0f);
        this.GetComponent<Renderer>().material = temp;
        myDissolveMaterial = temp;
    }

    void Dissolve()
    {
        dissolveTimer += Time.deltaTime;
        float currentTime = dissolveTimer / dissolveTime;
        if (currentTime > 1)
        {
            OnReturn();
        }
        else
        {
            myDissolveMaterial.SetFloat("_TreshHold", currentTime);
        }
    }

    void OnReturn()
    {
        this.transform.parent = null;
        this.gameObject.SetActive(false);

        if (myDefaultMaterial)
        {
            this.GetComponent<Renderer>().material = myDefaultMaterial;
            myDefaultMaterial = null;
            _materials.Add(myDissolveMaterial);
        }
        _home.Add(this.gameObject);
    }
}
