using System.Resources;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float mainThrust = 500f;
    [SerializeField] float rotatingThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip victorySound;

    AudioSource audioSource;
    Rigidbody rigidBody;
    enum State { Alive, Died, Transending };
    State state = State.Alive;

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
            // TODO Stop Sound
            case State.Alive:
                RespondToThrustInput();
                RespondToRotateInput();
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Return Means Nothing Below Will Run It Stops At This If Check
        // AKA Guard Condition
        if (state != State.Alive)
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
        Invoke("LoadNewScene", 1.5f);
    }

    private void StartDeathSequence()
    {
        state = State.Died;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        Invoke("LoadLastScene", 1.5f);
    }

    private void LoadNewScene()
    {
        // TODO load more than 2 levels
        SceneManager.LoadScene(1);
    }

    private void LoadLastScene()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
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
            // Playing The Selected Audio C
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
    {
        // Control It AKA Taking Away Physics
        rigidBody.freezeRotation = true;

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
