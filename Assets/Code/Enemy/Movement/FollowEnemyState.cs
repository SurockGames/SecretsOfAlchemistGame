using Pathfinding;
using UnityEngine;

public class FollowEnemyState : EnemyBaseState
{
    private Transform trs;
    private Transform followTarget;
    private AIDestinationSetter aIDestinationSetter;

    public FollowEnemyState(EnemyController enemyController, AIDestinationSetter aIDestinationSetter, Transform followTarget, Animator animator) : base(enemyController, animator)
    {
        trs = enemyController.transform;
        this.followTarget = followTarget;
        this.aIDestinationSetter = aIDestinationSetter;
    }

    public override void OnEnter()
    {
        aIDestinationSetter.target = followTarget;
        animator.SetBool(WalkHash, true);

        enemyController.StartRunning();
    }

    public override void Update()
    {

    }

    public override void OnExit()
    {
        aIDestinationSetter.target = trs;
        animator.SetBool(WalkHash, false);

        enemyController.StopRunning();
    }
}
