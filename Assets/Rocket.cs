using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float turnrate = 250f;
    [SerializeField] float thrustrate = 1000f;
    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {Alive, Death, Transcending };
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
        Thrust();
        Rotate();
        }
        else
        {
            audioSource.Stop();
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Death;
                Invoke("LoadStartScene", 1f);
                break;
        }

    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
        state = State.Alive;
    }

    void LoadStartScene()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
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
