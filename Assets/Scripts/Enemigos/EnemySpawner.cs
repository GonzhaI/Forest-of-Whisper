using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab; // Prefab del enemigo
    [SerializeField] private float spawnInterval = 5f; // Intervalo de generación en segundos
    [SerializeField] private string specialEnemyName = "DemonioVolador"; // Nombre del prefab especial
    [SerializeField] private Vector3 specialRotation = new Vector3(0, -182, 0); // Rotación especial para el prefab

    private void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        GameObject spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);

        if (spawnedEnemy.name.Contains(specialEnemyName))
        {
            spawnedEnemy.transform.rotation = Quaternion.Euler(specialRotation);
        }
    }
}
