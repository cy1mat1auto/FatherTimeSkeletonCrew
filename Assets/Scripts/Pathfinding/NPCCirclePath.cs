using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCirclePath : MonoBehaviour {

    //Based on one of Richard's first unity scripts.
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0,10f), ForceMode.Force);
        GetComponent<Rigidbody>().rotation *= Quaternion.Euler(new Vector3(0, 0.2f, 0));
        //transform.Rotate(new Vector3(0, 0.5f, 0), Space.World);
    }
}
