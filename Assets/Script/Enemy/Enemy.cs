using DG.Tweening;

using UnityEngine;

public class Enemy : Entity
{


    [Header("State")]
    EnemyStateMachine EnemyStateMachine;
    public EnemyIdleState EnemyIdleState { get; private set; }
    public EnemyWalkState EnemyWalkState { get; private set; }
    public EnemyAttackState EnemyAttackState { get; private set; }

    public EnemyInjuredState EnemyInjuredState { get; private set; }

    public EnemyKnockoutState EnemyKnockoutState { get; private set; }

    public EnemyVictoryState EnemyVictoryState { get; private set; }

    public Vector3 moveDirection;

    public BoxCollider boxCollider;
    public CapsuleCollider capsuleCollider;


    public Transform currentTarget = null;

    public float distanceToTarget = 0f;

    [Header("Combat Settings")]
    public float attackRange = 1.5f;
    public float attackRadius = 0.5f;
    public int attackDamage = 10;
    public bool isAttackTarget = false;



    public bool isInjured = false;

    public bool hasAttack = false;

    public float detectionRadius = 5f;
    public float rotationSpeed = 1f;

    [Header("AI Settings")]
    public float rotationDuration = 0.3f;
    private Tween lookTween;





    private void OnEnable()
    {
        OpponentManager.Instance.RegisterTargetEnemy(transform);

    }

    public override void Awake()
    {
        base.Awake();


        EnemyStateMachine = new EnemyStateMachine();
        EnemyIdleState = new EnemyIdleState(this, EnemyStateMachine, "Idle");
        EnemyWalkState = new EnemyWalkState(this, EnemyStateMachine, "Walk");
        EnemyAttackState = new EnemyAttackState(this, EnemyStateMachine, "Attack");
        EnemyInjuredState = new EnemyInjuredState(this, EnemyStateMachine, "Hit");
        EnemyKnockoutState = new EnemyKnockoutState(this, EnemyStateMachine, "Knock");
        EnemyVictoryState = new EnemyVictoryState(this, EnemyStateMachine, "Victory");
    }

    public override void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        boxCollider.enabled = false;
        EnemyStateMachine.Initialized(EnemyIdleState);
        
    }

    public override void Update()
    {
        base.Update();
        if (!GameManager.Instance.IsGameplayActive())
        {
            return;
        }
        EnemyStateMachine.currentState.Update();
        if(healthBar != null)
        {
            if (Mathf.Clamp01(currentHealth / maxHealth) == 1)
            {
                healthBar.gameObject.SetActive(false);
            }
            else 
            {
                healthBar.gameObject.SetActive(true);
                healthBar.SetupHealthBar(this.transform, maxHealth, currentHealth);
            }

        }

            attackCooldownTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
    }

    public bool IsMoving()
    {
        return moveDirection.magnitude > 0;
    }
    public void TakeDamage(int attackDamage, float attackCountEnemy)
    {
        attackCount = attackCountEnemy;
        currentHealth -= attackDamage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EnemyStateMachine.ChangeState(EnemyKnockoutState);
            OpponentManager.Instance.UnregisterTargetEnemy(transform);


        }
        else
        {
            EnemyStateMachine.ChangeState(EnemyInjuredState);

        }
    }

    public void CheckForHit()
    {
        Vector3 castOrigin = transform.position + transform.up * 0.6f;
        RaycastHit[] hits = Physics.SphereCastAll(
                                castOrigin,
                                attackRadius,
                                transform.forward,
                                attackRange,
                                CONST.playerTeamLayer,
                                QueryTriggerInteraction.Ignore);

        if (hits.Length > 0)
        {
            hasAttack = true;
            foreach (RaycastHit hit in hits)
            {
                TeamPlayer player = hit.collider.GetComponent<TeamPlayer>();
                if (player == null) continue;
                player.attackCount = attackCount;
                int attackDamage = TakeDamageCount();
                if(player is Player)
                {
                    player.GetComponent<Player>().TakeDamage(attackDamage, attackCount);
                }
                else
                {
                    player.GetComponent<Ally>().TakeDamage(attackDamage, attackCount);
                }
            }
        }

    }
    public bool CheckAttacking()
    {

        Vector3 castOrigin = transform.position + transform.up * 0.6f;

        RaycastHit hitInfo;
        bool hasHit = Physics.SphereCast(
                                castOrigin,
                                attackRadius,
                                transform.forward,
                                out hitInfo,
                                attackRange,
                                CONST.playerTeamLayer,
                                QueryTriggerInteraction.Ignore);
        return hasHit;
    }


    public bool IsPlayerNearby()
    {
        Vector3 castOrigin = transform.position + transform.up * 0.6f;
        bool hasHit = Physics.CheckSphere(
                           castOrigin, 
                           detectionRadius,
                           CONST.playerTeamLayer,
                           QueryTriggerInteraction.Ignore);
        return hasHit;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector3 castOrigin = transform.position + transform.up * 0.6f;
        Vector3 castDestination = castOrigin + transform.forward * attackRange;

        Gizmos.DrawLine(castOrigin, castDestination);

        Gizmos.DrawWireSphere(castOrigin, attackRadius);

        Gizmos.DrawWireSphere(castDestination, attackRadius);

        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireSphere(castOrigin, detectionRadius);

    }
    public void AnimationTrigger_CheckForHit()
    {
        hasAttack = false;
        CheckForHit();
    }
    public void AnimationTrigger_AttackFinished()
    {

        isAttackTarget = false;
        if (hasAttack)
        {
            attackCooldownTimer = attackCooldown;

        }
        Debug.Log("Attack Finished, Cooldown Timer Reset: " + attackCooldownTimer);

    }
    public void AnimationTrigger_InjuredFinished()
    {
        isInjured = true;

    }

    public void LookAtTarget(Transform target)
    {
        if (target == null) return;

        Vector3 lookDirection = (target.position - transform.position).normalized;
        lookDirection.y = 0; 

        if (lookDirection != Vector3.zero)
        {
            if(lookTween != null && lookTween.IsActive() )
            {
                lookTween.Kill();
            }
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            lookTween = transform.DORotateQuaternion(targetRotation, rotationDuration)
                .SetEase(Ease.OutQuad);
        }
    }

    public void AnimationTrigger_KnockoutFinished()
    {

        ObjectPool.Instance.ReturnObject(CONST.POOL_ENEMY, gameObject);
        capsuleCollider.enabled = true;
        boxCollider.enabled = false;
        

    }

    public override void ResetStatEntity()
    {
        isInjured = false;
        hasAttack = false;
        attackCooldownTimer = 0;
        isAttackTarget = false;
        //ResetStatsToOriginal();
        EnemyStateMachine.Initialized(EnemyIdleState);
        if (healthBar != null)
        {
            healthBar = null;
        }
    }
}
