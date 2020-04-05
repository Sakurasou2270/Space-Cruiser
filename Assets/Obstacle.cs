using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Allowing Only One Script Per Component
[DisallowMultipleComponent]
public class Obstacle : MonoBehaviour
{
    [SerializeField]
    Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField]
    float period = 2f;

    // 0 Not Moved 1 Fully Moved
    float movementFactor;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }

        // Grows Continually From 0
        float cylces = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cylces * tau);

        movementFactor = rawSinWave / 2f + .5f;

        // Setting The Scale MovementFactor towards the position
        Vector3 offset = movementVector * movementFactor;
        // Setting the starting pos to the current offset
        transform.position = startingPos + offset;
    }
}
