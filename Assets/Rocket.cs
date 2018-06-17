using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    float turnrate = 70;
    Rigidbody rigidBody;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(turnrate * Vector3.forward * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(turnrate  * - Vector3.forward * Time.deltaTime);
        }
    }
}
