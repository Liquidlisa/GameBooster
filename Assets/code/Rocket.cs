using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float turnrate = 250f;
    [SerializeField] float thrustrate = 1000f;
    [SerializeField] float levelDelay = 2f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip jingle;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem jingleParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;
    TextMesh[] textMesh;
    float energy = 50f;
    enum State {Alive, Death, Transcending };
    State state = State.Alive;
    float timeTracker;

	void Start ()
    {
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
            timeTracker = Time.time; // reset to current time
        }

        if (0 >= energy) // out of energy
        {
            if (state != State.Death)
            {
                state = State.Death;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                mainEngineParticles.Stop();
                deathParticles.Play();
                Invoke("LoadSameScene", levelDelay);
            }
        }

        if (state == State.Alive)
        {
            Thrust();
            Rotate();
            textMesh[0].text = energy + ""; // number posted as text
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Fuel":
                audioSource.Stop();
                audioSource.PlayOneShot(jingle);
                energy = energy + 20;
                collision.gameObject.SetActive(false);
                break;
            case "Finish":
                audioSource.Stop();
                audioSource.PlayOneShot(jingle);
                jingleParticles.Play();
                mainEngineParticles.Stop();
                state = State.Transcending;
                Invoke("LoadNextScene", levelDelay);
                break;
            default:
                energy = energy - 5;
                break;
        }
    }

    void LoadSameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        state = State.Alive;
    }

    void LoadNextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(index);
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
                audioSource.PlayOneShot(mainEngine);
                mainEngineParticles.Play();
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            float thrustnow = thrustrate * Time.deltaTime;
            rigidBody.AddRelativeForce(Vector3.down * thrustnow);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
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
