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

    [Header("玩家视野有关")]
    public SpriteRenderer spriteRenderer;
    public GameObject enemyUICanvas;

    [SerializeField] private bool _isVisible = false;
    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }
        set
        {
            if (value != _isVisible)
            {
                enemyUICanvas.SetActive(value);
            }
            _isVisible = value;
        }
    }

        [SerializeField]
    private EnemyState _currentState = EnemyState.Patrolling;
    public EnemyState CurrentState 
    { 
        get => _currentState;
        set 
        {
            if(value != _currentState)
            {
                if(value == EnemyState.Chasing)
                {
                    //Debug.Log("切换到 Chasing");
                    animator?.SetBool("isWalking", true);
                    animator?.SetBool("isMeleeAttacking", false);
                }
                else if (value == EnemyState.Patrolling)
                {
                    //Debug.Log("切换到 Patrolling");
                    animator?.SetBool("isWalking", true);
                    animator?.SetBool("isMeleeAttacking", false);
                }
                else if(value == EnemyState.MeleeAttacking)
                {
                    //Debug.Log("切换到 MeleeAttacking");
                    animator?.SetBool("isWalking", false);
                    animator?.SetBool("isMeleeAttacking", true);
                }
                else if (value == EnemyState.Idle)
                {
                    //Debug.Log("切换到 Idle");
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

        spriteRenderer = transform.Find("CharacterImage").gameObject.GetComponent<SpriteRenderer>();
        enemyUICanvas = transform.Find("Canvas").gameObject;

        animator?.SetBool("isMeleeAttacking", false);
        animator?.SetBool("isWalking", false);
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

        
        if (spriteRenderer.isVisible)
        {
            IsVisible = true;
        }
        else
        {
            IsVisible = false;
        }
    }

    // 实现巡逻行为
    public void Patrol()
    {
        MoveToPosition(townCenterTransform.position);
        targetDistance = Vector2.Distance(transform.position, townCenterTransform.position);
        
        //if (targetDistance < creatureData.atkRange)
        //{
        //    CurrentState = EnemyState.MeleeAttacking;
        //}

        SearchTarget();
    }

    // 实现追击行为，靠近target
    public void Chase()
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

    // 实现近战攻击行为
    public void MeleeAttack()
    {
        float targetDistance = Vector2.Distance(transform.position, targetTransform.position);

        Vector2 targetDir = targetTransform.position - transform.position;
        animator.SetFloat("targetX", targetDir.x);
        animator.SetFloat("targetY", targetDir.y);

        animator.SetBool("isMeleeAttacking", true);

        if(targetDistance > creatureData.atkRange)
        {
            CurrentState = EnemyState.Chasing;
            animator.SetBool("isMeleeAttacking", false);
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


}
