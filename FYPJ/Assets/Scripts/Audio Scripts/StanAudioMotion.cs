﻿#define BEAT_POOL
//#define STAN
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class StanAudioMotion : MonoBehaviour
{
    Transform _tfCamera;
    Transform _tfParent;
    Transform _tfThis;
    public float _intShiftRate;
    public float _ftTimeToTravel;
    float _ftX, _ftY, _ftZ;
    float _ftTime;
    int _intNumber;
    Vector3 _vec3Area;
    ControlCalibrationScript calibration;
    PlayerStats PlayerStats;

    public float speed = 10;
    public Transform endPoint;
	public int id;
	float count = 0;

    //[SerializeField]
#if (STAN)
    private Transform _childGlow = null;
    //[SerializeField]
    private Transform _childTwinkle = null;
    Vector3 _childGlowScale = new Vector3(0,0,0);
    Vector3 _childTwinkleScale  = new Vector3(0,0,0);
#endif


#if (BEAT_POOL)

    bool _die = false;
    List<GameObject> _home = null;
    List<Material> _materials = null;
    [SerializeField]
    [Range(0.1f, 1.5f)]
    float dissolveTime = 1f;
    Material myDissolveMaterial = null;
    Material myDefaultMaterial = null;
    float dissolveTimer = 0;
	public void PoolInit(List<GameObject> home, List<Material> mat)
	{

		dissolveTimer = 0;
		_home = home;
		_materials = mat;
	}
#else
    void Start() 
        {
#endif
//_childGlowScale = _childGlow.localScale;
//Debug.Assert(_childGlow);


#if (STAN)
        _childGlow = this.transform.GetChild(2);
         _childGlowScale = _childGlow.localScale;
        _childTwinkle = transform.GetChild(3);
        _childTwinkleScale = _childTwinkle.localScale;
#endif

	void Start()
	{
		_ftTime = 0.0f;
        _tfThis = this.transform;
        _tfParent = this.transform.parent.transform;
        _tfCamera = Camera.main.transform;

        _die = false;
        //_intNumber = int.Parse(Regex.Replace(_tfThis.name, "[^0-9]", ""));

       /* calibration = FindObjectOfType<ControlCalibrationScript>();
        _ftX = Random.Range(-calibration.horizontalSize, calibration.horizontalSize);
        _ftY = Random.Range(-calibration.verticleSize, calibration.verticleSize);
        _ftZ = -Mathf.Abs(calibration.distFromPlayer);*/

        PlayerStats = FindObjectOfType<PlayerStats>();
        //_vec3Area = calibration.calibrationObject.transform.position + new Vector3(_ftX, _ftY, _ftZ);
    }

    void Update()
    {
        _tfThis.Rotate(_tfThis.up, 4.0f);

        // _vec3Area = new Vector3(0, 0, -7.0f);
        endPoint= GameObject.Find("target "+id).transform;

        BeatMotion();
        if (!_die)
            TransitBeat();
        else
            dissolve();
    }

    void BeatMotion()
    {
       // _tfThis.position += new Vector3(0, Mathf.Sin(Time.time * _intShiftRate) * Time.deltaTime, 0);
    }

    void TransitBeat()
    {
        _ftTime += Time.deltaTime / _ftTimeToTravel   ;
		count += Time.deltaTime;
        Vector3 _vec3Heading = endPoint.position - _tfThis.position;
        if (!(_vec3Heading.sqrMagnitude < 0.1f * 0.1f))
            // _tfThis.position = Vector3.MoveTowards(_tfThis.position, endpoint.transform.position, speed * Time.deltaTime);
            _tfThis.position = Vector3.Lerp(_tfThis.transform.parent.transform.position, endPoint.transform.position, count);
        else
        {

			Die();
#if (BEAT_POOL)
            OnReturn();
#else
            Destroy(_tfThis.gameObject);
#endif
            //PlayerStats.ModifyScore(-1);
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

    void dissolve()
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

#if (STAN)
            _childGlow.localScale = Vector3.Lerp(_childGlowScale, Vector3.zero, currentTime);
            _childTwinkle.localScale = Vector3.Lerp(_childTwinkleScale, Vector3.zero, currentTime);
#endif
        }
    }

#if (BEAT_POOL)
    void OnReturn()
    {
        this.transform.parent = null;
        this.gameObject.SetActive(false);
        //if(_die)
        //this.GetComponent<Renderer>().material = myDefaultMaterial;

        if (myDefaultMaterial)
        {
            this.GetComponent<Renderer>().material = myDefaultMaterial;
            myDefaultMaterial = null;
            _materials.Add(myDissolveMaterial);
        }
        _home.Add(this.gameObject);
#else
    void OnDestroy()
    {
#endif
    }
}