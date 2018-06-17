using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
    [SerializeField] float turnrate = 250f;
    [SerializeField] float thrustrate = 1000f;
    Rigidbody rigidBody;
    AudioSource audioSource;
	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

        Thrust();
        Rotate();
	}

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            default:
                print("not friendly");
                break;
        }

    }
    void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustnow = thrustrate * Time.deltaTime;
                rigidBody.AddRelativeForce(Vector3.up * thrustnow);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }
    }
    void Rotate()
    {

        rigidBody.freezeRotation = true; // only turns with key press, not from collision.
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(turnrate * Vector3.forward * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(turnrate  * - Vector3.forward * Time.deltaTime);
        }
        rigidBody.freezeRotation = false;
    }
}
