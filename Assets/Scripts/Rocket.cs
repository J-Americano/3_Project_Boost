﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float MAIN_THRUST = 50f;
    [SerializeField] float RCS_THRUST = 100f;

    [SerializeField] AudioClip engineClip;
    [SerializeField] AudioClip successClip;
    [SerializeField] AudioClip deathClip;

    [SerializeField] ParticleSystem enginePrtcl;
    [SerializeField] ParticleSystem successPrtcl;
    [SerializeField] ParticleSystem deathPrtcl;

    int level = 0;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State
    {
        Alive,
        Dying,
        Transcending
    }

    State state = State.Alive;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            ShipControl();
        }
    }

    void ShipControl()
    {
        RespondToThrustInput();
        RespondToRotationInput();
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            enginePrtcl.Stop();
        }
    }

    void ApplyThrust()
    {
        //static thrust
        rigidBody.AddRelativeForce(Vector3.up * MAIN_THRUST);

        if (!audioSource.isPlaying)//avoids audio layering
        {
            audioSource.PlayOneShot(engineClip);
        }
        enginePrtcl.Play();
    }

    void RespondToRotationInput()
    {
        //Freeze rigid frame rotation
        rigidBody.freezeRotation = true;

        ApplyRotation();

        //Unfreeze rigid frame rotation
        rigidBody.freezeRotation = false;
    }

    void ApplyRotation()
    {
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }

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
                HandleSuccess();
                break;
            default:
                HandleDeath();
                break;
        }
    }

    void HandleSuccess()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successClip);
        successPrtcl.Play();
        Invoke("LoadNextLevel", 1f);//parameterise time
    }

    void HandleDeath()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathClip);
        deathPrtcl.Play();
        Invoke("LoadFirstLevel", 1f);
        //kill player
    }

    void LoadFirstLevel()
    {
        level = 0;
        deathPrtcl.Stop();
        SceneManager.LoadScene(level);
    }

    void LoadNextLevel()
    {
        if (SceneManager.sceneCount > level)
        {
            level++;
            successPrtcl.Stop();
            SceneManager.LoadScene(level);
            state = State.Alive;
        }
    }

    
}
