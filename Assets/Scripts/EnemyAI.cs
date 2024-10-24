using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : CreatureMove
{
    [Header("目标相关")]
    public Transform targetTransform;
    public Transform townCenterTransform;
    public LayerMask friendlyUnitCombieLayerMask;
    public float targetDistance = 0f;

    [Header("怪物视野相关")]
    public float viewRadius = 5f;
    public float hatredRadius = 20f;

    [Header("范围显示相关")]
    public Color viewRadiusColor = Color.blue;
    public Color atkColor = Color.red;
    public Color hatredColor = Color.green;

    [SerializeField]
    private EnemyState _currentState = EnemyState.Patrolling;




    [SerializeField] private float moveSpeed = 3f;

    [Header("Turret")]
    public static Action<Turret, float> OnTurretHit;
    public EnemyHealth EnemyHealth { get; set; }
    public EnemyState CurrentState
    {
        get => _currentState;
        set
        {
            if (value != _currentState)
            {
                if (value == EnemyState.Chasing)
                {
                    Debug.Log("切换到 Chasing");
                    animator?.SetBool("isWalking", true);
                    animator?.SetBool("isMeleeAttacking", false);
                }
                else if (value == EnemyState.Patrolling)
                {
                    Debug.Log("切换到 Patrolling");
                    animator?.SetBool("isWalking", true);
                    animator?.SetBool("isMeleeAttacking", false);
                }
                else if (value == EnemyState.MeleeAttacking)
                {
                    Debug.Log("切换到 MeleeAttacking");
                    animator?.SetBool("isWalking", false);
                    animator?.SetBool("isMeleeAttacking", true);
                }
                else if (value == EnemyState.Idle)
                {
                    Debug.Log("切换到 Idle");
                    animator?.SetBool("isWalking", false);
                    animator?.SetBool("isMeleeAttacking", false);
                }
            }
            _currentState = value;
        }
    }

    protected override void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = transform.Find("CharacterImage").gameObject.GetComponent<Animator>();
        creatureData = GetComponent<CreatureData>();
        _targetPos = transform.position;
        townCenterTransform = GameObject.FindGameObjectsWithTag("Base")[0].transform;
        targetTransform = GameObject.FindGameObjectsWithTag("Base")[0].transform; ;
        friendlyUnitCombieLayerMask = LayerMask.GetMask("Friendly", "Friendly Building");

        animator?.SetBool("isMeleeAttacking", false);
        animator?.SetBool("isWalking", false);

        EnemyHealth = GetComponent<EnemyHealth>();


        TurretHealth.OnTurretKilled += OnTurretKilled;
    }

    protected override void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                break;
            case EnemyState.Chasing:
                Chase();
                break;
            case EnemyState.MeleeAttacking:
                MeleeAttack();
                break;
            case EnemyState.RangedAttacking:
                RangedAttack();
                break;
        }

    }

    // 实现巡逻行为
    public void Patrol()
    {
        MoveToPosition(townCenterTransform.position);
        targetDistance = Vector2.Distance(transform.position, townCenterTransform.position);

        // If there is no target, search for one
        SearchTarget();
    }


    // 实现追击行为，靠近target
    public void Chase()
    {
        if (targetTransform != null)
        {
            animator.SetBool("isWalking", true);
            MoveToPosition(targetTransform.position);
            targetDistance = Vector2.Distance(transform.position, targetTransform.position);

            if (targetDistance > hatredRadius)
            {
                CurrentState = EnemyState.Patrolling;
            }
            else if (targetDistance < creatureData.atkRange)
            {
                CurrentState = EnemyState.MeleeAttacking;
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            // If target is null, switch back to Patrolling state
            CurrentState = EnemyState.Patrolling;
        }
    }


    // 实现近战攻击行为
    public void MeleeAttack()
    {
        // Check if targetTransform is not null
        if (targetTransform != null)
        {
            float targetDistance = Vector2.Distance(transform.position, targetTransform.position);

            Vector2 targetDir = targetTransform.position - transform.position;
            animator.SetFloat("targetX", targetDir.x);
            animator.SetFloat("targetY", targetDir.y);

            animator.SetBool("isMeleeAttacking", true);

            if (targetDistance > creatureData.atkRange)
            {
                CurrentState = EnemyState.Chasing;
                animator.SetBool("isMeleeAttacking", false);
            }
        }
        else
        {
            // If target is null, switch back to Patrolling state
            CurrentState = EnemyState.Patrolling;
        }
    }


    void RangedAttack()
    {
        // 实现远程攻击行为
    }

    void SearchTarget()
    {
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, viewRadius, friendlyUnitCombieLayerMask);

        if (targetsInRange.Length > 0)
        {
            GameObject closestTarget = FindClosestTarget(targetsInRange);
            if (closestTarget != null)
            {
                targetTransform = closestTarget.transform;
                CurrentState = EnemyState.Chasing;
            }
        }
    }

    GameObject FindClosestTarget(Collider2D[] targets)
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D target in targets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target.gameObject;
            }
        }
        return closest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = viewRadiusColor;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Gizmos.color = atkColor;
        Gizmos.DrawWireSphere(transform.position, creatureData.atkRange);

        //Gizmos.color = hatredColor;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hatredRadius);
    }


    public void StopMovement()
    {
        // Before stopping movement, make sure the NavMeshAgent is not null.
        if (navMeshAgent != null)
        {
            // Set the NavMeshAgent's speed to 0, effectively stopping the movement.
            navMeshAgent.speed = 0f;
        }
    }

    public void ResetMovement()
    {
        // Before resetting movement, make sure the NavMeshAgent is not null.
        if (navMeshAgent != null)
        {
            // Reset the NavMeshAgent's speed to its original value.
            navMeshAgent.speed = moveSpeed; // Assuming 'moveSpeed' is the original speed value
        }
    }



    private void OnDestroy()
    {
        // 取消监听，避免内存泄漏
        TurretHealth.OnTurretKilled -= OnTurretKilled;
    }

    // 处理炮塔死亡事件，切换状态为巡逻
    private void OnTurretKilled(Turret turret)
    {
        Debug.Log("炮塔已被摧毁，敌人切换到巡逻状态");

        // Set targetTransform to null if the destroyed turret was the current target
        if (targetTransform != null && targetTransform.GetComponent<Turret>() == turret)
        {
            targetTransform = null;
        }

        // Switch to Patrolling state
        CurrentState = EnemyState.Patrolling;
    }


}
