using UnityEngine;

public class DieState : EnemyBaseState
{
    private RagdollActivator ragdollActivator;
    private float ragdollTimeActivation;

    private float cooldownTimer;

    public DieState(EnemyController enemyController, Animator animator, RagdollActivator ragdollActivator, float ragdollTimeActivation) : base(enemyController, animator)
    {
        this.ragdollActivator = ragdollActivator;
        this.ragdollTimeActivation = ragdollTimeActivation;
    }

    public override void OnEnter()
    {
        animator.SetTrigger(DieHash);
        cooldownTimer = ragdollTimeActivation;
    }

    public override void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0)
        {
            animator.enabled = false;
            ragdollActivator.TurnOnRagdoll();
        }
    }

    public override void OnExit()
    {

    }
}