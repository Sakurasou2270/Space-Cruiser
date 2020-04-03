using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField]
    float mainThrust = 500f;

    [SerializeField]
    float rotatingThrust = 100f;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Hit Friendly");
                break;
            case "Fuel":
                print("Hit Fuel");
                break;
            case "Finish":
                print("Finished Level");
                SceneManager.LoadScene(1);
                break;
            default:
                print("Died");
                SceneManager.LoadScene(0);
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

            if (!audioSource.isPlaying)
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
