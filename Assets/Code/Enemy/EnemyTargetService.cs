using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetService : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private List<EnemyController> enemyControllers = new List<EnemyController>();

    public void RegisterEnemy(EnemyController enemyController)
    {
        enemyControllers.Add(enemyController);
    }

    public void UnregisterEnemy(EnemyController enemyController)
    {
        enemyControllers.Remove(enemyController);
    }

    public void Update()
    {
        if ((int)(Time.time % 5) == 0)
            CheckVisionOfEnemies();
    }

    private void CheckVisionOfEnemies()
    {
        foreach (var enemyController in enemyControllers)
        {
            if (enemyController.CanSeePlayer) continue;

            if (Vector3.Distance(enemyController.transform.position, playerTransform.position) <= enemyController.VisionDistance)
            {
                enemyController.CheckIfCanSeePlayer();
            }
        }
    }
}
