#define BEAT_POOL
// #define STAN
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AudioMotion : MonoBehaviour
{
#if (STAN)
    Transform _tfCamera;
    Transform _tfParent;
#endif
    public Transform _tfPar;
    Transform _tfThis;
    GameObject _goPlayer;
    public float _ftShiftRate;
    public float _ftTimeToTravel;
    float _ftX, _ftY, _ftZ;
    float _ftTime;
    Vector3 _vec3Area;
    ControlCalibrationScript calibration;
    public BoundCalculator bounds;
    //PlayerStats PlayerStats;

    public float speed = 10;
    public Transform endPoint;

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
    int _home = 0; // index of the pool where object came from
    //List<GameObject> _home = null;
    List<Material> _materials = null;
    [SerializeField]
    [Range(0.1f, 1.5f)]
    float dissolveTime = 1f;
    Material myDissolveMaterial = null;
    Material myDefaultMaterial = null;
    float dissolveTimer = 0;
    ObjectPool _myPool = null;
    //public void PoolInit(List<GameObject> home, List<Material> mat)
    
#if (SPAWNDEBUG)
    float timer = 0;
#endif
    public void PoolInit(int home, List<Material> mat,ObjectPool Pool)
    {
        dissolveTimer = 0;
        _home = home;
        
        _materials = mat;
	_myPool = Pool;
#else
    void Start() 
    {
#endif
//_childGlowScale = _childGlow.localScale;
//Debug.Assert(_childGlow);


#if (STAN)
        _childGlow = this.transform.GetChild(1);
         _childGlowScale = _childGlow.localScale;
        _childTwinkle = transform.GetChild(2); // out of bounds
        _childTwinkleScale = _childTwinkle.localScale;
#endif
        _ftTime = 0.0f;
        _tfThis = this.transform;

#if (STAN)
        if (_goPlayer.GetComponent<PlayerStats>()._bl4x != true)
            _tfParent = this.transform.parent.transform;
        _tfCamera = Camera.main.transform;
#endif

        _die = false;

        calibration = FindObjectOfType<ControlCalibrationScript>();
        _ftX = Random.Range(-calibration.horizontalSize, calibration.horizontalSize);
        _ftY = Random.Range(-calibration.verticleSize, calibration.verticleSize);
        _ftZ = -Mathf.Abs(calibration.distFromPlayer);

        if (calibration.calibrationObject.transform.position.z != 0) {
            string _strParName = Regex.Replace(GetChildZero().name, "^[A-z]+", "");
            _strParName = Regex.Replace(_strParName, " ", "");
            int _intDespawner = int.Parse(_strParName);
            Vector3 _vec3Parse = Vector3.zero;

            if (_goPlayer.GetComponent<PlayerStats>()._intSpawnMode == 0) { // 4x
                if (bounds != null && bounds.gameObject.activeSelf == true) {
                    switch (_intDespawner) {
                        case 0: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[0];} break;
                        case 1: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[1];} break;
                        case 2: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[2];} break;
                        case 3: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[3];} break;
                        default: break;
                    }

                    GetChildZero().transform.position = new Vector3(_vec3Parse.x, _vec3Parse.y, calibration.calibrationObject.transform.position.z);
                }
            }

            else { //8x
                if (bounds != null && bounds.gameObject.activeSelf == true) {
                    switch (_intDespawner) {
                        case 0: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[0] + new Vector3(+1.0f, +0.0f, 0.0f);} break;
                        case 5: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[0] + new Vector3(+0.0f, -1.0f, 0.0f);} break;
                        case 1: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[1] + new Vector3(+0.0f, -1.0f, 0.0f);} break;
                        case 4: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[1] + new Vector3(-1.0f, +0.0f, 0.0f);} break;
                        case 2: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[2] + new Vector3(+0.0f, +1.0f, 0.0f);} break;
                        case 7: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[2] + new Vector3(+1.0f, +0.0f, 0.0f);} break;
                        case 3: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[3] + new Vector3(-1.0f, +0.0f, 0.0f);} break;
                        case 6: {_vec3Parse = bounds.GetComponent<BoundCalculator>()._vec3Points[3] + new Vector3(+0.0f, +1.0f, 0.0f);} break;
                        default: break;
                    }

                    GetChildZero().transform.position = new Vector3(_vec3Parse.x, _vec3Parse.y, calibration.calibrationObject.transform.position.z);
                }
            }
        }

// If Calibrating
        if (calibration != null)
            _vec3Area = GetChildZero().transform.position;
// If Defaulting -> Skip
        else
        //PlayerStats = FindObjectOfType<PlayerStats>();
        _vec3Area = calibration.calibrationObject.transform.position + new Vector3(_ftX,
                                                                                   _ftY,
                                                                                   _ftZ);

#if (SPAWNDEBUG)
	timer = 0;
#endif
    }

    void Update()
    {
        _tfThis.Rotate(_tfThis.up, 4.0f);

// If Calibrating
        if (calibration != null)
            _vec3Area = GetChildZero().transform.position;
// If Defaulting -> Skip

        endPoint.position = _vec3Area;

	
#if (SPAWNDEBUG)
	timer += Time.deltaTime;
#endif
        BeatMotion();
#if (SPAWNDEBUG)
	if(timer < 1.f)
#else
        if (!_die)
#endif
            TransitBeat();
        else
            dissolve();
    }

    void BeatMotion()
    {
        _tfThis.position += new Vector3(0, Mathf.Sin(Time.time * _ftShiftRate) * Time.deltaTime, 0);
    }

    void TransitBeat()
    {
        _ftTime += Time.deltaTime / _ftTimeToTravel;
        Vector3 _vec3Heading = _vec3Area - _tfThis.position;
        if (!(_vec3Heading.sqrMagnitude < 0.1f * 0.1f)) {
            _tfThis.position = Vector3.Lerp(_tfPar.transform.position, _vec3Area, _ftTime);
            // Debug.Log("T: " + GetChildZero().transform.position);
        }
        else
        {
#if (BEAT_POOL)
            OnReturn();
#else
            Destroy(_tfThis.gameObject);
#endif
            _goPlayer.GetComponent<PlayerStats>().ModifyCombo(false);
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
        //_home.Add(this.gameObject);
	_myPool.ReturnObjectToPool(this.gameObject,_home);
	_home = 0;
#else
    void OnDestroy()
    {
#endif
    }

    public void SetPlayer(GameObject goPlayer_) {
        _goPlayer = goPlayer_;
    }

    private GameObject GetChildZero() {
        return _tfPar.transform.GetChild(0).gameObject;
    }
}
