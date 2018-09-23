using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aircam : MonoBehaviour {
    GameObject ac;
	// Use this for initialization
	void Start () {
        ac = GameObject.Find("aircraft");
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(ac.transform.position.x, ac.transform.position.y, ac.transform.position.z-0.5f);
	}
}
