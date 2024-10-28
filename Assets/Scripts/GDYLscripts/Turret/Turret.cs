using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float attackRange = 3f;  // 修正拼写错误

    [SerializeField] private Transform Rotationtransform;

    public GameObject chooseUIPrefab;

    public EnemyAI CurrentEnemyTarget { get; set; }

    private bool _gameStarted;
    private List<EnemyAI> _enemies;


    public TurretHealth TurretHealth { get; set; }
    private void Start()
    {
        _enemies = new List<EnemyAI>();
        _gameStarted = true;

        chooseUIPrefab = Resources.Load<GameObject>("Prefabs/UIPrefabs/TurretChooseObj");
        FieldOfView.Instance.SetOrigin(new KeyValuePair<GameObject, Vector3>(gameObject, transform.position), attackRange);
    }

    //private void OnDestroy()
    //{
    //    FieldOfView.Instance.SetOrigin(new KeyValuePair<GameObject, Vector3>(gameObject, transform.position), attackRange);
    //}

    private void Update()
    {
        GetCurrentEnemyTarget();
        RotateTowardsTarget();
    }

    
    private void GetCurrentEnemyTarget()
    {
        if (_enemies.Count <= 0)
        {
            CurrentEnemyTarget = null;
            return;
        }

        CurrentEnemyTarget = _enemies[0]; // 获取列表中的第一个敌人作为当前目标
    }

    private void RotateTowardsTarget()
    {
        if (CurrentEnemyTarget == null)
        {
            return;
        }

        Vector3 targetPos = CurrentEnemyTarget.transform.position - transform.position;
        float angle = Vector3.SignedAngle(Rotationtransform.up, targetPos, Rotationtransform.forward);
        Rotationtransform.Rotate(0f, 0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy Creature"))  // 确保EnemyAI的对象也使用这个标签
        {
            EnemyAI newEnemy = other.GetComponent<EnemyAI>();
            if (newEnemy != null)        // 确保组件存在
            {
                _enemies.Add(newEnemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy Creature"))  // 确保EnemyAI的对象也使用这个标签
        {
            EnemyAI newEnemy = other.GetComponent<EnemyAI>();
            if (newEnemy != null && _enemies.Contains(newEnemy))
            {
                _enemies.Remove(newEnemy);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!_gameStarted)
        {
            GetComponent<CircleCollider2D>().radius = attackRange;  // 修正拼写错误
        }

        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
  
}
