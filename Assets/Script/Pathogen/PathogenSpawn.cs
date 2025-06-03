using UnityEngine;


public class PathogenSpawn : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject pathogenPrefab;     // Prefab of the pathogen (must have Health + PathogenRandomMovement + Collider2D + Rigidbody2D)
    public int numberOfPathogens = 10;    // Total number of pathogens to spawn
    public float spawnInterval = 1f;      // Time between spawns

    [Header("Spawn Area Settings")]
    public Vector2 spawnAreaMin = new Vector2(-5f, -5f);
    public Vector2 spawnAreaMax = new Vector2(5f, 5f);
    public Vector2 sizeRange = new Vector2(0.5f, 1.5f);  // Random localScale range

    private int pathogensSpawned = 0;

    void Start()
    {
        // Every 'spawnInterval' seconds, call SpawnPathogen(). 
        InvokeRepeating(nameof(SpawnPathogen), 0f, spawnInterval);
        GameManager.Instance?.RegisterPathogen(gameObject);
    }

    void SpawnPathogen()
    {
        if (pathogensSpawned >= numberOfPathogens)
        {
            CancelInvoke(nameof(SpawnPathogen));
            return;
        }

        // Pick a random position within the designated bounds:
        Vector2 spawnPos = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Instantiate the pathogen prefab:
        GameObject pathogen = Instantiate(pathogenPrefab, spawnPos, Quaternion.identity);

        // Randomly scale it:
        float randomScale = Random.Range(sizeRange.x, sizeRange.y);
        pathogen.transform.localScale = new Vector3(randomScale, randomScale, 1f);

        // If it has PathogenRandomMovement, let it pick a random direction now:
        PathogenRandomMovement randomMovement = pathogen.GetComponent<PathogenRandomMovement>();
        if (randomMovement != null)
        {
            randomMovement.SetRandomDirection();
        }

        pathogensSpawned++;
    }
}