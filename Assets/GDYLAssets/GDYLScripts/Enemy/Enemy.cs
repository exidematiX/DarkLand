using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public static Action OnEndReached;

    [SerializeField] private float moveSped = 3f;
    //[SerializeField] private Waypoint waypoint;

    public Waypoint waypoint{ get; set; }


    /// <summary>
    /// 返回敌人需要去的现在的位置
    /// </summary>
    public Vector3 CurrentPointPosition => waypoint.GetWaypointPosition(_currentWaypointIndex);

    private int _currentWaypointIndex;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        _currentWaypointIndex = 0;//1
        _enemyHealth = GetComponent<EnemyHealth>();
    }


    private void Update()
    {
        Move();
        if (CurrentPointPositionReached())
        {
           UpdateCurrentPointIndex();
        }


    }

    private void Move()
    {
       
        transform.position = Vector3.MoveTowards(transform.position,
        CurrentPointPosition,moveSped * Time.deltaTime);
    }

    private bool CurrentPointPositionReached()
    {
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;
        if(distanceToNextPointPosition < 0.1f)
        {
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
        
        OnEndReached?.Invoke();
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject);
    }



    public void ResetEnemy()
    {
        _currentWaypointIndex = 0;
    }
}
