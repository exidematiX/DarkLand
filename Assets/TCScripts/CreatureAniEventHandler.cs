using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAniEventHandler : MonoBehaviour
{
    public EnemyAI enemyAI;
    public void MeleeAttackFinished()
    {
        //enemyAI.CurrentState = EnemyState.Idle;
        Debug.Log("造成伤害");
        
        if(enemyAI.targetDistance < enemyAI.creatureData.atkRange)
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
        //enemyAI.animator?.SetBool("isMeleeAttacking", false);
        //enemyAI.animator?.SetBool("isWalking", false);
    }

    void Update()
    {
        
    }
}
