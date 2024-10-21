using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float attackRangfe = 3f;

    public Enemy CurrentEnemyTarget { get; set; }

    private bool _gameStarted;
    private List<Enemy> _enemies;

    private void Start()
    {
        
        _enemies = new List<Enemy>();
        _gameStarted = true;
    }

    private void Update()
    {

        GetCurrentEnemyTarget();
        RotateTowardsTarget();
    }

    private void GetCurrentEnemyTarget()
    {
        if(_enemies.Count <= 0) 
        {
            CurrentEnemyTarget = null;
            return;
        }

        CurrentEnemyTarget = _enemies[0];

    }

    private void RotateTowardsTarget()
    {
        if(CurrentEnemyTarget == null)
        {
            return;
        }

        Vector3 targetPos = CurrentEnemyTarget.transform.position - transform.position;
        float angle = Vector3.SignedAngle(transform.up,targetPos,transform.forward);
        transform.Rotate(0f,0f,angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy newEnemy = other.GetComponent<Enemy>();
            _enemies.Add(newEnemy);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy newEnemy = other.GetComponent<Enemy>();
            if (_enemies.Contains(newEnemy))
            {
                _enemies.Remove(newEnemy);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!_gameStarted)
        {
            GetComponent<CircleCollider2D>().radius = attackRangfe;
        }

        Gizmos.DrawWireSphere(transform.position, attackRangfe);
    }
}
