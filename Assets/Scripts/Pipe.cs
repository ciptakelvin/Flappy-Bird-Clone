using UnityEngine;

public class Pipe : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 15f); // Destroy the pipe after 10 seconds to prevent memory leaks
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * 2f); // Move the pipe to the left at a speed of 2 units per second
    }
}
