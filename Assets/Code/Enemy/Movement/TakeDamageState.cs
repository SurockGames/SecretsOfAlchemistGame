using UnityEngine;

public class TakeDamageState : EnemyBaseState
{
    private float damagedBackupTime;

    private float cooldownTimer;

    public TakeDamageState(EnemyController enemyController, Animator animator, float damagedBackupTime) : base(enemyController, animator)
    {
        this.damagedBackupTime = damagedBackupTime;
    }

    public override void OnEnter()
    {
        animator.SetTrigger(TakeDamageHash);
        cooldownTimer = damagedBackupTime;
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
