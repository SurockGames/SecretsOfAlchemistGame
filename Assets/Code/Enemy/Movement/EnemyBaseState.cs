using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : IState
{
    protected readonly EnemyController enemyController;
    protected readonly Animator animator;

    protected static readonly int IdleHash = Animator.StringToHash("Idle");
    protected static readonly int TakeDamageHash = Animator.StringToHash("TakeDamage");
    protected static readonly int DieHash = Animator.StringToHash("Die");

    protected static readonly int AttackHash = Animator.StringToHash("Attack");
    protected static readonly int RunHash = Animator.StringToHash("Run");
    protected static readonly int WalkHash = Animator.StringToHash("Walk");


    public EnemyBaseState(EnemyController enemyController, Animator animator)
    {
        this.enemyController = enemyController;
        this.animator = animator;
    }

    public virtual void OnEnter()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void OnExit()
    {

    }
}
