using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] float MAIN_THRUST = 50f;
    [SerializeField] float RCS_THRUST = 100f;
    
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                //do nothing
                print("Friend");
                break;
            case "Fuel":
                print("Fuel added!");
                break;
            case "Goal":
                print("Stage complete!");
                break;
            default:
                print("Dead");
                //kill player
                break;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        ShipControl();
    }

    void ShipControl()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //static thrust
            rigidBody.AddRelativeForce(Vector3.up * MAIN_THRUST);

            if (!audioSource.isPlaying)//avoids audio layering
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        //Freeze rigid frame rotation
        rigidBody.freezeRotation = true;

        //Calculate dynamic rotation per frame
        float rotationThisFrame = RCS_THRUST * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        //Unfreeze rigid frame rotation
        rigidBody.freezeRotation = false;
    }
}
