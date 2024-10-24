using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAniEventHandler : MonoBehaviour
{
    public EnemyAI enemyAI;
    public TurretHealth targetTurretHealth;  // 目标 TurretHealth
    public float damageAmount = 1f;  // 每次攻击造成的伤害

    public void MeleeAttackFinished()
    {
        Debug.Log("造成伤害");

        // 检查目标是否在攻击范围内
        if (targetTurretHealth != null && enemyAI.targetDistance < enemyAI.creatureData.atkRange)
        {
            targetTurretHealth.DealDamage(damageAmount);  // 对 TurretHealth 调用 DealDamage
        }

        // 根据敌人的远近调整敌人状态
        if (enemyAI.targetDistance < enemyAI.creatureData.atkRange)
        {
            enemyAI.CurrentState = EnemyState.MeleeAttacking;
        }
        else if (enemyAI.targetDistance > enemyAI.creatureData.atkRange && enemyAI.targetDistance < enemyAI.viewRadius)
        {
            enemyAI.CurrentState = EnemyState.Chasing;
        }
        else if (enemyAI.targetDistance > enemyAI.viewRadius)
        {
            enemyAI.CurrentState = EnemyState.Patrolling;
        }
    }

    void Start()
    {
        enemyAI = GetComponentInParent<EnemyAI>();

        // 假设目标是场景中的某一特定炮塔:
        targetTurretHealth = FindObjectOfType<TurretHealth>();  // 这只是一个简单示例
    }
}
