using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour {
    GameObject earth;
	// Use this for initialization
	void Start () {
        earth = GameObject.Find("earth");
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        //other.transform.localEulerAngles = Vector3.zero;
        other.transform.position = new Vector3(earth.transform.position.x, earth.transform.position.y + 0.5f, earth.transform.position.z);
    }
}
