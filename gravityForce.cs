using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityForce : MonoBehaviour {
    
    [SerializeField]
    float gravityf;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
       // Debug.Log(transform.position.z);	
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Rocket")
        { 
            /*
            if ((other.gameObject.transform.position.y - transform.position.y) > 0)
                other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * gravityf);
            if ((other.gameObject.transform.position.y - transform.position.y) < 0)
                other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * gravityf);
            */
            if ((other.gameObject.transform.position.z- transform.position.z) >0)
                other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back*gravityf);
            if ((other.gameObject.transform.position.z - transform.position.z) < 0)
                other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * gravityf);
            if ((other.gameObject.transform.position.x - transform.position.x) > 0)
                other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * gravityf);
            if ((other.gameObject.transform.position.x - transform.position.x) < 0)
                other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * gravityf);
            Vector3 distance = new Vector3(other.gameObject.transform.position.x - transform.position.x, other.gameObject.transform.position.y - transform.position.y, other.gameObject.transform.position.z - transform.position.z);
            Debug.Log(distance.magnitude);
            if (distance.magnitude < transform.localScale.z/2+0.1f)
            {
                other.gameObject.transform.position = new Vector3(1000, 1000, 1000);
                Debug.Log("아아아ㅏㄱ");
            }
            }
    }
}
