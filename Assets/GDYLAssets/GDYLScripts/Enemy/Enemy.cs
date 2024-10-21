using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public static Action<Enemy> OnEndReached;

    [SerializeField] private float moveSpeed = 3f;
    //[SerializeField] private Waypoint waypoint;

    public Waypoint waypoint{ get; set; }
    public float MoveSpeed { get; set; }


    public EnemyHealth EnemyHealth { get; set; }

    /// <summary>
    /// 返回敌人需要去的现在的位置
    /// </summary>
    public Vector3 CurrentPointPosition => waypoint.GetWaypointPosition(_currentWaypointIndex);

    private int _currentWaypointIndex;

    private Vector3 _lastPointPosition;
    private EnemyHealth _enemyHealth;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _currentWaypointIndex = 0;//1

        _spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyHealth = GetComponent<EnemyHealth>();

        MoveSpeed = moveSpeed;

        _lastPointPosition = transform.position;
        _enemyHealth = GetComponent<EnemyHealth>();
    }


    private void Update()
    {

        Rotate();
        Move();
        if (CurrentPointPositionReached())
        {
           UpdateCurrentPointIndex();
        }


    }

    private void Move()
    {
       
        transform.position = Vector3.MoveTowards(transform.position,
        CurrentPointPosition,MoveSpeed * Time.deltaTime);
    }


    public void StopMovement()
    {
        MoveSpeed = 0f;
    }

    public void ResetMovement()
    {
        MoveSpeed = moveSpeed;
    }


    private void Rotate()
    {
        if (CurrentPointPosition.x > _lastPointPosition.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    private bool CurrentPointPositionReached()
    {
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;
        if(distanceToNextPointPosition < 0.1f)
        {
            _lastPointPosition = transform.position;
            return true;
        }
        return false;
    }

    private void UpdateCurrentPointIndex()
    {
        int lastWaypointIndex = waypoint.Points.Length - 1;
        if (_currentWaypointIndex < lastWaypointIndex)
        {
            _currentWaypointIndex++;
        }
        else
        {
            EndPointReached();
        }
    }


    private void EndPointReached()
    {
        
        OnEndReached?.Invoke(this);
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject);
    }



    public void ResetEnemy()
    {
        _currentWaypointIndex = 0;
    }
}
