using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEnemySpawner : MonoBehaviour
{
    [SerializeField] private Game game;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private EnemySO enemy;
    [SerializeField] private float spawnCooldown;

    public bool IsPatrolling;

    [ShowIf(nameof(IsPatrolling))]
    [SerializeField] private List<Transform> patrollingPoints;

    private EnemyController spawnedEnemy;

    private void Start()
    {
        SpawnEnemy();
    }

    private void StartCooldownForNextSpawn()
    {
        if (spawnedEnemy != null)
        {
            spawnedEnemy.OnDie -= StartCooldownForNextSpawn;
            game.EnemyTargetService.UnregisterEnemy(spawnedEnemy);

            spawnedEnemy = null;
        }

        StartCoroutine(SpawnEnemyCooldown());
    }

    private IEnumerator SpawnEnemyCooldown()
    {
        yield return new WaitForSeconds(spawnCooldown);

        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        spawnedEnemy = Instantiate(enemy.Enemy, spawnPoint.position, spawnPoint.rotation);
        game.EnemyTargetService.RegisterEnemy(spawnedEnemy);

        if (IsPatrolling && patrollingPoints.Count > 1)
        {
            spawnedEnemy.Initialize(game, playerTransform, patrollingPoints);
        }
        else
        {
            spawnedEnemy.Initialize(game, playerTransform, null);
        }

        spawnedEnemy.OnDie += StartCooldownForNextSpawn;
    }
}
