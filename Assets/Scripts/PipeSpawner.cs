using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipePrefab; // Reference to the pipe prefab
    public float spawnInterval = 2f; // Time interval between pipe spawns

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnPipe", 0f, spawnInterval); // Start spawning pipes at regular intervals
    }

    // Spawn Pipes
    void SpawnPipe()
    {
        Instantiate(pipePrefab, transform.position + Vector3.up * Random.Range(-1f, 1f), Quaternion.identity);
    }
}
