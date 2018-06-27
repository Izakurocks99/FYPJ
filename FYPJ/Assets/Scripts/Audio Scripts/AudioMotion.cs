#define BEAT_POOL
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AudioMotion : MonoBehaviour {

	Transform _tfCamera;
	Transform _tfParent;
	Transform _tfThis;
    public float _intShiftRate;
	float _ftX, _ftY, _ftZ;
	int _intNumber;
	Vector3 _vec3Area;
    ControlCalibrationScript calibration;
    PlayerStats PlayerStats;

    public Transform endPoint;

#if (BEAT_POOL)
    List<GameObject> _home = null;
    List<Material> _materials = null; 
    public void PoolInit(List<GameObject> home)
        {
        _home = home;
#else
    void Start() 
        {
#endif
		_tfThis = this.transform;
		_tfParent = this.transform.parent.transform;
		_tfCamera = Camera.main.transform;

		_intNumber = int.Parse(Regex.Replace(_tfThis.name, "[^0-9]", ""));

        calibration = FindObjectOfType<ControlCalibrationScript>();
        _ftX = Random.Range(-calibration.horizontalSize, calibration.horizontalSize);
        _ftY = Random.Range(-calibration.verticleSize, calibration.verticleSize);
        _ftZ = -calibration.distFromPlayer;

        PlayerStats = FindObjectOfType<PlayerStats>();
        _vec3Area = calibration.calibrationObject.transform.position + new Vector3(_ftX,
                                                     _ftY,
                                                     _ftZ);
    }

	void Update () {
		_tfThis.Rotate(_tfThis.up, 7.0f);

        endPoint.position = _vec3Area;

		BeatMotion();
		TransitBeat();
	}

	void BeatMotion() {
		_tfThis.position += new Vector3(0, Mathf.Sin(Time.time * _intShiftRate) * Time.deltaTime, 0);
	}

	void TransitBeat() {
		Vector3 _vec3Heading = _vec3Area - _tfThis.position;
        if (!(_vec3Heading.sqrMagnitude < 0.1f * 0.1f))
            _tfThis.position = Vector3.MoveTowards(_tfThis.position, _vec3Area, 0.1f);
        else
        {
#if (BEAT_POOL)
            OnReturn();
#else
            Destroy(_tfThis.gameObject);
#endif
            PlayerStats.ModifyScore(-1);
        }
	}

#if (BEAT_POOL)
    void OnReturn()
    {
            this.gameObject.SetActive(false);
            _home.Add(this.gameObject);
#else
    void OnDestroy()
    {
#endif

    }
}
