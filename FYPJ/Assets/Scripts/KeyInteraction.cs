using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInteraction : MonoBehaviour {

    public GameObject _goContainer;
    public Material[] _materials;

	void Update () {

        if (Input.GetMouseButtonDown(0))
		{
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            List<GameObject> listGO = new List<GameObject>();
            foreach (Transform child in _goContainer.transform)
                listGO.Add(child.gameObject);

            if (Physics.Raycast(ray, out hit, 2.25f))
            {
                for (int i = 0; i < listGO.Count; i++)
                    if (listGO[i].name == hit.transform.gameObject.name)
                    {
                        if (listGO[i].transform.GetComponent<Renderer>().material.name.Contains(_materials[1].name))
                            listGO[i].transform.GetComponent<Renderer>().material = _materials[0];
                        break;
                    }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            List<GameObject> listGO = new List<GameObject>();
            foreach (Transform child in _goContainer.transform)
                listGO.Add(child.gameObject);

            if (Physics.Raycast(ray, out hit, 2.25f))
            {
                for (int i = 0; i < listGO.Count; i++)
                    if (listGO[i].name == hit.transform.gameObject.name)
                    {
                        if (listGO[i].transform.GetComponent<Renderer>().material.name.Contains(_materials[2].name))
                            listGO[i].transform.GetComponent<Renderer>().material = _materials[0];
                        break;
                    }
            }
        }

        if (Input.GetKey(KeyCode.A))
            Camera.main.transform.eulerAngles -= new Vector3(0.0f, 80.0f, 0.0f) * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            Camera.main.transform.eulerAngles += new Vector3(0.0f, 80.0f, 0.0f) * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            Camera.main.transform.eulerAngles -= new Vector3(80.0f, 0.0f, 0.0f) * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            Camera.main.transform.eulerAngles += new Vector3(80.0f, 0.0f, 0.0f) * Time.deltaTime;
    }
}
