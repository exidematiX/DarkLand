using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
public class Projectile : MonoBehaviour
{

    public static Action<EnemyAI, float> OnEnemyHit;

    [SerializeField] protected float moveSpeed = 10f;
    [SerializeField] protected float damage = 2f;
    [SerializeField] private float minDistanceToDealDamage = 0.1f;

    public TurretProjectile TurretOwner { get; set; }

    protected EnemyAI _enemyTarget;

    protected virtual void Update()
    {
        if (_enemyTarget != null)
        {
            MoveProjectile();
            RotateProjectile();
        }
    }


    public void SetEnemy(EnemyAI enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("Attempting to set a null enemy", this);
            return;
        }
        _enemyTarget = enemy;
    }
    protected virtual void MoveProjectile()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            _enemyTarget.transform.position, moveSpeed * Time.deltaTime);


        float diatanceToTarget = (_enemyTarget.transform.position - transform.position).magnitude;
        if (diatanceToTarget < minDistanceToDealDamage)
        {
            if (_enemyTarget == null)
            {
                Debug.LogError("Enemy target is not set", this);
            }

            OnEnemyHit?.Invoke(_enemyTarget, damage);
            _enemyTarget.EnemyHealth.DealDamage(damage);
            TurretOwner.ResetTurretProjectile();
            Pooler.ReturnToPool(gameObject);
        }
    }

    private void RotateProjectile()
    {
        Vector3 enemyPos = _enemyTarget.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.up, enemyPos, transform.forward);
        transform.Rotate(0f, 0f, angle);
    }




    public void ResetProjectile()
    {
        _enemyTarget = null;
        transform.rotation = Quaternion.identity;
    }
}
