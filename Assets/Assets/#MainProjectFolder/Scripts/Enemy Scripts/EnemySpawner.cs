using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private HexCell cell;
    [SerializeField] private Vector2 spawnPositions;

    private void Awake()
    {
        // Ensure only one instance of the class exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnEnemy(Transform spawnTransform)
    {
        if (enemyPrefab != null && spawnTransform != null)
        {
            Instantiate(enemyPrefab, spawnTransform.position, spawnTransform.rotation);
        }
        else
        {
            Debug.LogError("EnemyPrefab or spawnTransform is null. Make sure to assign them in the Inspector.");
        }
    }

}
