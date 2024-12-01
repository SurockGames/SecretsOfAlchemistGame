using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexEnemySpawner : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private List<EnemySO> enemiesList;
    [SerializeField] private int difficultyTockens;
    [SerializeField] private float spawnRadius;

    private List<EnemyController> spawnedEnemies;

    [Button]
    private void CreateEnemyPool()
    {
        int tockensForWave = difficultyTockens;

        StartCoroutine(EnemySpawn(tockensForWave));
    }

    private IEnumerator EnemySpawn(int tockens)
    {
        List<EnemySO> enemyToSpawnPool = new List<EnemySO>();

        foreach (var enemy in enemiesList)
        {
            enemyToSpawnPool.Add(enemy);
        }

        while (tockens > 0 && enemyToSpawnPool.Count > 0)
        {
            int rand = Random.Range(0, enemyToSpawnPool.Count);

            if (tockens - enemyToSpawnPool[rand].TockenValue >= 0)
            {
                if (enemyToSpawnPool[rand].Enemy == null)
                {
                    Debug.LogError($"No ENEMY Prefab on enemy scriptable object {enemyToSpawnPool[rand]}");
                    enemyToSpawnPool.Remove(enemyToSpawnPool[rand]);

                    continue;
                }

                var enemy = InstantiateEnemy(enemyToSpawnPool[rand].Enemy);
                enemy.Initialize(game, playerTransform, null);

                spawnedEnemies.Add(enemy);

                tockens -= enemyToSpawnPool[rand].TockenValue;

                yield return new WaitForSeconds(1);
            }
            else
            {
                enemyToSpawnPool.Remove(enemyToSpawnPool[rand]);
            }
        }

        yield return null;
    }

    private EnemyController InstantiateEnemy(EnemyController enemy)
    {
        float randX = Random.Range(-1f, 1f);
        float randZ = Random.Range(-1f, 1f);

        Vector2 randVector = new Vector3(randX, randZ);
        randVector.Normalize();

        Vector3 spawnPosition = new Vector3(spawnRadius * randVector.x, 0, spawnRadius * randVector.y);

        spawnPosition = new Vector3(transform.position.x + spawnPosition.x, transform.position.y + spawnPosition.y, transform.position.z + spawnPosition.z);

        return Instantiate(enemy, spawnPosition, Quaternion.identity);
    }
}
