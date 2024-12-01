using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyState : EnemyBaseState
{
    private Transform trs;
    private List<Transform> patrollingPoints;
    private AIDestinationSetter aIDestinationSetter;

    private int nextPoint;

    public PatrolEnemyState(EnemyController enemyController, AIDestinationSetter aIDestinationSetter, List<Transform> patrollingPoints, Animator animator) : base (enemyController, animator) 
    {
        trs = enemyController.transform;
        this.patrollingPoints = patrollingPoints;
        this.aIDestinationSetter = aIDestinationSetter;
    }

    public override void OnEnter()
    {
        enemyController.Patrolling(true);
        animator.SetBool(WalkHash, true);

        var minDist = 10000;
        int toPoint = 0;

        for (int i = 0; i < patrollingPoints.Count; i++)
        {
            Transform t = patrollingPoints[i];
            if (Vector3.Distance(trs.position, t.position) < minDist)
            {
                toPoint = i; 
            }    
        }

        nextPoint = toPoint;
        aIDestinationSetter.target = patrollingPoints[nextPoint];
        //CalculateNextPoint();
    }

    public override void Update()
    {
        var dist = Vector3.Distance(trs.position, patrollingPoints[nextPoint].position);
        if (dist < 1f)
        {
            CalculateNextPoint();

            aIDestinationSetter.target = patrollingPoints[nextPoint];
        }
    }

    private void CalculateNextPoint()
    {
        if (nextPoint + 1 == patrollingPoints.Count)
        {
            nextPoint = 0;
        }
        else
        {
            nextPoint++;
        }
    }

    public override void OnExit()
    {
        enemyController.Patrolling(false);
        animator.SetBool(WalkHash, false);
        aIDestinationSetter.target = trs;
    }
}
