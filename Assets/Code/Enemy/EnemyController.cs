using Assets.Code.Shooting;
using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BodyController
{
    [SerializeField] private BodyPartsDamageFactor bodyPartsDamageFactors;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private AIDestinationSetter aIDestinationSetter;
    [SerializeField] private FollowerEntity followerEntity;
    [SerializeField] private Animator animator;
    [SerializeField] private RagdollActivator ragdollActivator;
    [SerializeField] private float ragdollTimeActivation;
    [SerializeField] private float takeDameBackupCooldown;
    [SerializeField] private LayerMask raylayer;
    [SerializeField] private LayerMask onlyPlayerLayer;

    [SerializeField] private float visionDistance = 15;
    [SerializeField] private float attackCooldownTime = 3;
    [SerializeField] private float attackRadius = 2;
    [SerializeField] private float walkingSpeed = 1.95f;
    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private int damageAmount = 15;

    private Game game;
    public bool CanSeePlayer => canSeePlayer;
    public float VisionDistance => visionDistance;

    private StateMachine skeletonStateMachine;
    private bool canSeePlayer;
    private bool isInAgro;
    private Transform playerTrs;

    public event Action OnDie;
    public event Action<int, Vector3, bool> OnGetDamage;

    private BodyPartType bodyPartWasHit;

    [ShowInInspector, ReadOnly]
    private Health health;
    private float attackCooldown = 0;
    private bool isInAttackRadius;
    private bool isDead;

    private readonly Vector3 offset = new Vector3 (0, 1.5f, 0);
    private bool isInitialized;
    private bool getDamage;
    private bool isPatrolling;
    private bool isMoving;

    private bool attacking;

    protected static readonly int RunHash = Animator.StringToHash("Run");

    private void Awake()
    {
        health = new Health(maxHealth);
    }

    public void Initialize(Game game, Transform playerTransform, List<Transform> patrollingPoints)
    {
        this.game = game;
        playerTrs = playerTransform;
        health = new Health(maxHealth);
        health.OnDie += Die;

        skeletonStateMachine = new StateMachine();

        var idleState = new IdleEnemyState(this, animator);
        var followState = new FollowEnemyState(this, aIDestinationSetter, playerTransform, animator);
        var patrollingState = new PatrolEnemyState(this, aIDestinationSetter, patrollingPoints, animator);
        var attackEnemyState = new SimpleAttackEnemyState(this, playerTransform, animator, aIDestinationSetter, attackCooldownTime);
        var dieState = new DieState(this, animator, ragdollActivator, ragdollTimeActivation);
        var takeDamageState = new TakeDamageState(this, animator, takeDameBackupCooldown);

        if (patrollingPoints != null)
        {
            skeletonStateMachine.AddTransition(idleState, patrollingState, new FuncPredicate(() => !canSeePlayer));
            skeletonStateMachine.AddTransition(patrollingState, followState, new FuncPredicate(() => canSeePlayer));

            skeletonStateMachine.AddTransition(takeDamageState, patrollingState, new FuncPredicate(() => isMoving));
        }
        skeletonStateMachine.AddAnyTransition(attackEnemyState, new FuncPredicate(() => isInAttackRadius && attackCooldown <= 0));

        skeletonStateMachine.AddAnyTransition(takeDamageState, new FuncPredicate(() => getDamage));
        skeletonStateMachine.AddAnyTransition(dieState, new FuncPredicate(() => isDead));

        skeletonStateMachine.AddTransition(idleState, followState, new FuncPredicate(() => canSeePlayer));

        skeletonStateMachine.AddTransition(takeDamageState, idleState, new FuncPredicate(() => !getDamage));
        skeletonStateMachine.AddTransition(followState, idleState, new FuncPredicate(() => !canSeePlayer));
        skeletonStateMachine.AddTransition(attackEnemyState, idleState, new FuncPredicate(() => attackCooldown > 0));
        skeletonStateMachine.SetState(idleState);

        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        if (canSeePlayer)
        {
            var dist = Vector2.Distance(playerTrs.position, transform.position);

            if (dist > visionDistance && !isInAgro)
            {
                canSeePlayer = false;
            }
            else if (isInAgro && dist > visionDistance * 2)
            {
                canSeePlayer = false;
                isInAgro = false;
            }
            else if (dist < attackRadius)
            {
                isInAttackRadius = true;
            }
            else
            {
                isInAttackRadius = false;
            }
        }

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (followerEntity.velocity.magnitude > 1)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        skeletonStateMachine.Update();
    }

    public void StartRunning()
    {
        followerEntity.maxSpeed = runningSpeed;
        animator.SetBool(RunHash, true);
    }

    public void StopRunning()
    {
        followerEntity.maxSpeed = walkingSpeed;
        animator.SetBool(RunHash, false);
    }

    public void AttackAnimationFinished()
    {
        Debug.Log("Attack");
        var from = transform.position + offset;
        var to = playerTrs.position - transform.position;

        Debug.DrawRay(from, to, Color.red, 3);

        if (Physics.SphereCast(from - transform.forward, 1, transform.forward * 3, out RaycastHit hit, visionDistance, onlyPlayerLayer))
        {
            DealDamageToPlayer();
        }

        AttackProccessed();
    }

    private void DealDamageToPlayer()
    {
        Debug.Log("Deal damage to player");
        game.PlayerStatsService.GetDamage(damageAmount);
    }

    public void CheckIfCanSeePlayer()
    {
        var from = transform.position + offset;
        var to = playerTrs.position - transform.position;

        Debug.DrawRay(from, to, Color.red, 3);
        if (Physics.Raycast(from, playerTrs.position - transform.position, out RaycastHit hit, visionDistance, raylayer))
        {
            if (hit.transform == playerTrs)
            {
                canSeePlayer = true;
            }
        }
    }

    private void Die()
    {
        health.OnDie -= Die;

        OnDie?.Invoke();
        isDead = true;
    }

    public void DamageProccessed()
    {
        getDamage = false;
        bodyPartWasHit = BodyPartType.None;
    }

    public void AttackProccessed()
    {
        attackCooldown = attackCooldownTime;
    }

    public void Patrolling(bool set)
    {
        isPatrolling = set;
    }

    public override void RegisterHit(BodyPartType bodyPart, int damageAmount, Vector3 position)
    {
        float bodyFactor = bodyPartsDamageFactors.GetBodyFactor(bodyPart);
        if (health.TryDealDamage((int)(damageAmount * bodyFactor)))
        {
            OnGetDamage?.Invoke((int)(damageAmount * bodyFactor), position, bodyPart == BodyPartType.Head);
            getDamage = true;
            bodyPartWasHit = bodyPart;
            canSeePlayer = true;
            isInAgro = true;
            //Debug.Log($"Hit body part - {bodyPart}, and did damage - {(int)(damageAmount * bodyFactor)}");
        }
    }
}
