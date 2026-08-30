using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    // The Pipe/Obstacle prefab template that will be duplicated into the scene
    [SerializeField] private GameObject obstaclePrefab;

    // The delay timer (in seconds) between each consecutive pipe spawn
    [SerializeField] private float spawnInterval = 2f;

    [Header("Randomization Buffer")]
    [SerializeField] private float minHeight = -2f;
    [SerializeField] private float maxHeight = 2f;

    void Start()
    {
        // InvokeRepeating calls a specific function by its text name over a timer loop.
        // Parameters: (Function Name, Time before the very first call, Time interval between loops)
        // 'nameof(SpawnObstacle)' passes the function name as a safe string to prevent typos.
        InvokeRepeating(nameof(SpawnObstacle), 0f, spawnInterval);
    }

    void SpawnObstacle()
    {
        // 1. Calculate a random vertical height offset
        float randomY = Random.Range(minHeight, maxHeight);

        // 2. Build a new spawning position combining the spawner's X coordinate with the random Y coordinate
        Vector3 spawnPosition = new Vector3(transform.position.x, randomY, transform.position.z);

        // 3. Instantiate the pipe at that customized offset position
        Instantiate(obstaclePrefab, spawnPosition, transform.rotation, transform);
    }
}