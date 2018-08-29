using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float MAIN_THRUST = 50f;
    [SerializeField] float RCS_THRUST = 100f;

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
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f);//parameterise time
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);
                //kill player
                break;
        }
    }

    private void LoadFirstLevel()
    {
        level = 0;
        SceneManager.LoadScene(level);
    }

    void LoadNextLevel()
    {
        if (SceneManager.sceneCount > level)
        {
            level++;
            SceneManager.LoadScene(level);
            state = State.Alive;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (state == State.Alive)
        {
            ShipControl();
        }
        else if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
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
