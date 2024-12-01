using UnityEngine;

public class IdleEnemyState : EnemyBaseState
{
    private readonly Transform trs;

    private int nextPoint;

    public IdleEnemyState(EnemyController enemyController, Animator animator) : base(enemyController, animator)
    {
        trs = enemyController.transform;
    }

    public override void OnEnter()
    {
        animator.SetBool(IdleHash, true);
    }

    public override void OnExit()
    {
        nextPoint = 0;
        animator.SetBool(IdleHash, true);
    }
}