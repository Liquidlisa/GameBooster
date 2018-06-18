using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float turnrate = 250f;
    [SerializeField] float thrustrate = 1000f;
    Rigidbody rigidBody;
    AudioSource audioSource;
    TextMesh textMesh;
    int currentScene = 0;
    float energy = 999f;
    enum State {Alive, Death, Transcending };
    State state = State.Alive;
    float startTime;
	// Use this for initialization
	void Start () {
        textMesh = GetComponent<TextMesh>();
        textMesh.text = "9";
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        print(Time.time - startTime);
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
                state = State.Death;
                Invoke("LoadSameScene", 1f);
                break;
        }
    }

    void LoadSameScene()
    {
        SceneManager.LoadScene(currentScene);
        state = State.Alive;
    }

    void LoadNextScene()
    {
        currentScene = currentScene + 1;
        if (currentScene >= SceneManager.sceneCount) currentScene = SceneManager.sceneCount;
        if (currentScene == SceneManager.sceneCount)
        {
            SceneManager.LoadScene(SceneManager.sceneCount);
        }
        else
        {
        SceneManager.LoadScene(currentScene);
        }
        state = State.Alive;

    }

    void LoadStartScene()
    {
        currentScene = 0;
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
