using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class univ : MonoBehaviour {

    [SerializeField]
    float rotation = 1;
    float ts = 1;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        Mathf.Clamp(ts, 1, 1000);
        if (Input.GetKey(KeyCode.Alpha1))
            Time.timeScale = 1;
        if (Input.GetKey(KeyCode.Alpha2))
            Time.timeScale = 10;
        if (Input.GetKey(KeyCode.Alpha3))
            Time.timeScale = 100;
        transform.Rotate(Vector3.up * rotation * Time.deltaTime * Time.timeScale);//배속일때 더 빨라지게
    }
    
}
