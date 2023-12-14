using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject[] pipePrefabs; // Array contendo os prefabs dos canos
    public float spawnRate = 2f; // Taxa de geração dos canos
    public float minY = -8.35f; // Altura mínima de spawn em Y
    public float maxY = -3.96f; // Altura máxima de spawn em Y

    public float timeSinceLastSpawn = 0f;

    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= spawnRate)
        {
            SpawnPipe();
            timeSinceLastSpawn = 0f;
        }
    }
    private void Start() {
        SpawnPipe();
    }

    public void SpawnPipe()
    {
        GameObject pipePrefab = pipePrefabs[Random.Range(0, pipePrefabs.Length)]; // Seleciona aleatoriamente um prefab de cano da lista
        float randomY = Random.Range(minY, maxY); // Gera uma altura Y aleatória

        Vector2 spawnPosition = new Vector2(transform.position.x, randomY);
        Instantiate(pipePrefab, spawnPosition, Quaternion.identity);
    }
}
