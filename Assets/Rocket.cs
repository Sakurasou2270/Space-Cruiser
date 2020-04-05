using System.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float mainThrust = 500f;
    [SerializeField] float rotatingThrust = 100f;
    [SerializeField] float levelLoadDelay = 1.5f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip victorySound;

    AudioSource audioSource;
    Rigidbody rigidBody;
    // Another Way Instead Of A Enum
    // bool isTransitioning = false;
    enum State { Alive, Died, Transending };
    State state = State.Alive;
    bool collisionEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Alive:
                RespondToThrustInput();
                RespondToRotateInput();
                break;
            default:
                break;
        }

        // Will Only Run In Debug Mode
        if (Debug.isDebugBuild)
        {
        RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Invoke("LoadNewScene", levelLoadDelay);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionEnabled = !collisionEnabled;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Return Means Nothing Below Will Run It Stops At This If Check
        // AKA Guard Condition
        if (state != State.Alive || !collisionEnabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Fuel":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transending;
        audioSource.Stop();
        audioSource.PlayOneShot(victorySound);
        // Loading Scene For 1 Second
        Invoke("LoadNewScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Died;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        Invoke("ReloadScene", levelLoadDelay);
    }

    private void LoadNewScene()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevel == SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 0;
        }
        SceneManager.LoadScene(nextLevel);
    }

    private void ReloadScene()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
            // mainEngineParticles.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            // Playing The Selected Audio
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
    {
        // Set Any Rotation It Had With Physics To Zero
        rigidBody.angularVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * rotatingThrust * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward * rotatingThrust * Time.deltaTime);
        }
        // Stop Controlling It AKA Physics Re Applied
        rigidBody.freezeRotation = false;
    }
}
