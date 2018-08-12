using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        ProcessUpdate();
    }

    void ProcessUpdate()
    {
        handleShipControl();
    }

    void handleShipControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigidBody.AddRelativeForce(Vector3.up);
            enableBoosterAudio();
        }
        else
        {
            audioSource.Stop();
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward);
        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward);
        }
    }

    void enableBoosterAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
