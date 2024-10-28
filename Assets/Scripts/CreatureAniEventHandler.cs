// CreatureAniEventHandler.cs
using UnityEngine;

public class CreatureAniEventHandler : MonoBehaviour
{
    public EnemyAI enemyAI;
    private TurretHealth targetTurretHealth;
    public float damageAmount = 1f;

    public void SetTarget(TurretHealth target)
    {
        targetTurretHealth = target;
    }

    public void MeleeAttackFinished()
    {
        Debug.Log("造成伤害");

        // 检查目标是否在攻击范围内
        if (targetTurretHealth != null && enemyAI.targetDistance < enemyAI.creatureData.atkRange)
        {
            targetTurretHealth.DealDamage(damageAmount);
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
    }
}
