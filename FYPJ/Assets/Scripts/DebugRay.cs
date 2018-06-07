using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugRay : MonoBehaviour {

    public int _intColor;
    public int _intDirection;

	void Update () {
        Color _color = Color.red;

        switch(_intColor)
        {
            case 1:
                {
                    _color = Color.grey;
                    break;
                }
            case 2:
                {
                    _color = Color.black;
                    break;
                }
            default:
                {
                    _color = Color.red;
                    break;
                }
        }

        switch(_intDirection)
        {
            case 1:
            {
                Debug.DrawRay(transform.position, transform.up * 50f, _color);		
                break;                
            }
            default:
            {
                Debug.DrawRay(transform.position, transform.forward * 50f, _color);		
                break;
            }
        }
	}
}
