using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float turnrate = 250f;
    [SerializeField] float thrustrate = 1000f;
    Rigidbody rigidBody;
    AudioSource audioSource;
    TextMesh[] textMesh;
    float energy = 50f;
    enum State {Alive, Death, Transcending };
    State state = State.Alive;
    float timeTracker;

	void Start () {

        textMesh = GetComponentsInChildren<TextMesh>();
        textMesh[0].text = energy+"";
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        timeTracker = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeTracker >= 1) // update every second
        {
            energy = energy - 1;
            if(0 >= energy)
            {
                state = State.Death;
                Invoke("LoadStartScene", 1f);

            }
            timeTracker = Time.time; // reset to current time
            textMesh[0].text = energy+""; // number posted as text

        }
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
        if(state != State.Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                energy = energy - 5;
                break;
        }
    }

    void LoadSameScene()
    {
        SceneManager.LoadScene(0);
        state = State.Alive;
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
        if (Input.GetKey(KeyCode.W))
        {
            float thrustnow = thrustrate * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.up * thrustnow);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            float thrustnow = thrustrate * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.down * thrustnow);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
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
