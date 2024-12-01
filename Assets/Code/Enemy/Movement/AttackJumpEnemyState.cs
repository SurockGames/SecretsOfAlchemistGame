using Pathfinding;
using System.Collections;
using UnityEngine;

public class AttackJumpEnemyState : EnemyBaseState
{
    private Transform trs;
    private Transform attackTarget;

    public AttackJumpEnemyState(EnemyController enemyController, Transform attackTarget, Animator animator) : base(enemyController, animator)
    {
        trs = enemyController.transform;
        this.attackTarget = attackTarget;
    }

    public override void OnEnter()
    {

    }

    public override void Update()
    {

    }

    public override void OnExit()
    {

    }
}

public class SimpleAttackEnemyState : EnemyBaseState
{
    private Transform trs;
    private Transform attackTarget;
    private AIDestinationSetter aIDestinationSetter;
    private float attackBackupTime;
    private float cooldownTimer;

    public SimpleAttackEnemyState(EnemyController enemyController, Transform attackTarget, Animator animator, AIDestinationSetter aIDestinationSetter, float attackBackUp) : base(enemyController, animator)
    {
        trs = enemyController.transform;
        this.attackTarget = attackTarget;
        this.aIDestinationSetter = aIDestinationSetter;
        this.attackBackupTime = attackBackUp;
    }

    public override void OnEnter()
    {
        animator.SetTrigger(AttackHash);
        aIDestinationSetter.target = trs;
        cooldownTimer = attackBackupTime;
    }

    public override void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0)
            enemyController.DamageProccessed();
    }

    public override void OnExit()
    {

    }
}