using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    // The rate (in units per second) at which the object slides to the left
    [SerializeField] float scrollSpeed = 2f;

    // The off-screen threshold on the left. Once the object goes past this X coordinate, it wraps around.
    [SerializeField] float resetPositionX = -19.2f;

    // The on-screen destination on the right where the object jumps back to when resetting.
    [SerializeField] float startPositionX = 19.2f;

    void Update()
    {
        // 1. Move the object continuously to the left, scaled by frame rate via Time.deltaTime
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;

        // 2. Wrap-around Check: Has the object completely scrolled out of the camera's view?
        if (transform.position.x <= resetPositionX)
        {
            // Find out exactly how far past the line it went
            float overflow = transform.position.x - resetPositionX;

            // 3. Teleport the object back to the right side (startPositionX) 
            // while preserving its current Y and Z positions to maintain its height.
            transform.position = new Vector3(startPositionX + overflow, transform.position.y, transform.position.z);
        }
    }
}