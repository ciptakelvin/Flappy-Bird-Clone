using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    // The constant speed at which the obstacle (pipe) slides to the left
    [SerializeField] private float speed = 3f;

    // The boundary X-coordinate where the obstacle is considered completely off-screen
    [SerializeField] private float deadZone = -10f;

    // Update is called once per frame and handles frame-rate independent movement
    void Update()
    {
        // Moves the obstacle to the left by 'speed' units per second.
        // Vector3.left is a shorthand for (-1, 0, 0).
        // Time.deltaTime ensures the movement remains smooth and identical whether running at 30 FPS or 300 FPS.
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Cleanup Check: Once the object moves past the left edge of the screen...
        if (transform.position.x < deadZone)
        {
            // Destroy this GameObject to free up system memory and prevent an infinite accumulation of hidden pipes.
            Destroy(gameObject);
        }
    }
}