using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : TeamPlayer
{



    [Header("State")]
    PlayerStateMachine playerStateMachine;
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerWalkState playerWalkState { get; private set; }
    public PlayerAttackState playerAttackState { get; private set; }

    public PlayerInjureState PlayerInjureState { get; private set; }

    public PlayerKnockoutState PlayerKnockoutState { get; private set; }

    public PlayerJumpState PlayerJumpState { get; private set; }

    public PlayerVictoryState PlayerVictoryState { get; private set; }

    [Header("Required Components")]
    public FloatingJoystick joystick;

    public Vector3 moveDirection;

    public BoxCollider boxCollider;
    public CapsuleCollider capsuleCollider;



    [Header("Combat Settings")]
    public float attackRange = 1.5f;
    public float attackRadius = 0.5f;
    public bool isAttackTarget = false;





    public bool hasAttack = false;
    public bool isInjured = false;
    public bool isJump = false;
    public override void Awake()
    {
        base.Awake();

        joystick = GameManager.Instance.joystick;
        ClickJump.OnSwipe += SetBoolJump;
        playerStateMachine = new PlayerStateMachine();
        playerIdleState = new PlayerIdleState(this, playerStateMachine, "Idle");
        playerWalkState = new PlayerWalkState(this, playerStateMachine, "Walk");
        playerAttackState = new PlayerAttackState(this, playerStateMachine, "Attack");
        PlayerInjureState = new PlayerInjureState(this, playerStateMachine, "Hit");
        PlayerKnockoutState = new PlayerKnockoutState(this, playerStateMachine, "Knock");
        PlayerJumpState = new PlayerJumpState(this, playerStateMachine, "Jump");
        PlayerVictoryState = new PlayerVictoryState(this, playerStateMachine, "Victory");

    }
    private void OnEnable()
    {
        OpponentManager.Instance.RegisterTargetPlayer(transform);
    }

    public override void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        boxCollider.enabled = false;
        playerStateMachine.Initialized(playerIdleState);
    }

    public override void Update()
    {
        base.Update();
        if (!GameManager.Instance.IsGameplayActive())
        {
            return;
        }
        playerStateMachine.currentState.Update();

        attackCooldownTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        HandleFourDirectionalMovement();
    }

    private void SetBoolJump()
    {
        Debug.Log("Jump Triggered");
        isJump = true;
    }

    private void HandleFourDirectionalMovement()
    {
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        moveDirection = Vector3.zero;

        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                moveDirection.x = Mathf.Sign(horizontalInput);
            }
            else
            {
                moveDirection.z = Mathf.Sign(verticalInput);
            }
            moveDirection.y = 0;
        }
    }
    public bool IsMoving()
    {
        return moveDirection.magnitude > 0;
    }
    public void CheckForHit()
    {
        Vector3 castOrigin = transform.position + transform.up * 0.6f;
        RaycastHit[] hits = Physics.SphereCastAll(
                                castOrigin,
                                attackRadius,
                                transform.forward,
                                attackRange,
                                CONST.enemyLayer,
                                QueryTriggerInteraction.Ignore);

        if (hits.Length > 0)
        {
            hasAttack = true;
            foreach (RaycastHit hit in hits)
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.attackCount = attackCount;
                    int attackDamage = TakeDamageCount();

                    enemy.TakeDamage(attackDamage, attackCount);
                }
            }
        }
    }
    public void TakeDamage(int attackDamage, float attackCountEnemy)
    {
        attackCount = attackCountEnemy;
        currentHealth -= attackDamage;
        GameView.Instance.UpdateHpPlayer(maxHealth, currentHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OpponentManager.Instance.UnregisterTargetPlayer(transform);
            playerStateMachine.ChangeState(PlayerKnockoutState);


        }
        else
        {
            playerStateMachine.ChangeState(PlayerInjureState);

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
                                CONST.enemyLayer,
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
    }
    public void AnimationTrigger_InjuredFinished()
    {
        isInjured = true;

    }
    public void AnimationTrigger_KnockoutFinished()
    {
        ObjectPool.Instance.ReturnObject(CONST.POOL_PLAYER, gameObject);

        capsuleCollider.enabled = true;
        boxCollider.enabled = false;

    }
    public void AnimationTrigger_JumpFinished()
    {
        isJump = false;
       
    }

    public override void ResetStatEntity()
    {
        //ResetStatsToOriginal();

        isInjured = false;
        hasAttack = false;
        attackCooldownTimer = 0;
        isAttackTarget = false;
        isJump = false;
        playerStateMachine.Initialized(playerIdleState);
    }
}
