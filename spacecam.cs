using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spacecam : MonoBehaviour {
    GameObject sc;
    GameObject mars;
	// Use this for initialization
	void Start () {
        sc = GameObject.Find("SpaceshipFighter");
        mars = GameObject.Find("mars");

	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(sc.transform.position.x, sc.transform.position.y + 28.44f, sc.transform.position.z +13f);
        //Debug.Log(sc.transform.position);
        //Debug.Log(mars.transform.position);
	}
}
