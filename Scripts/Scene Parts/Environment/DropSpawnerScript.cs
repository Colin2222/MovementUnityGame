using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSpawnerScript : MonoBehaviour
{
    public GameObject dropPrefab;
    public float spawnInterval = 5f;
    public float randomSpawnOffset = 1f;

    float spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnDrop();
            spawnTimer = spawnInterval + Random.Range(-randomSpawnOffset, randomSpawnOffset);
        }
    }

    void SpawnDrop()
    {
        Instantiate(dropPrefab, transform.position, Quaternion.identity);
    }
}
