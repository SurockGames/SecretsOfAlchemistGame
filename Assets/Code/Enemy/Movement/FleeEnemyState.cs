using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class FleeEnemyState : EnemyBaseState
{
    private Transform trs;
    private AIDestinationSetter aIDestinationSetter;

    private int nextPoint;

    public FleeEnemyState(EnemyController enemyController, AIDestinationSetter aIDestinationSetter, Animator animator) : base(enemyController, animator)
    {
        trs = enemyController.transform;
        this.aIDestinationSetter = aIDestinationSetter;
    }

    public override void OnEnter()
    {
        animator.SetBool(WalkHash, true);
        
        //aIDestinationSetter.target = patrollingPoints[nextPoint];
        //CalculateNextPoint();
    }

    public override void Update()
    {
        
    }

    public override void OnExit()
    {
        enemyController.Patrolling(false);
        animator.SetBool(WalkHash, false);
        aIDestinationSetter.target = trs;
    }
}
