using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class CreatureMove : MonoBehaviour
{
    public float xVelocity = 0f;
    public float yVelocity = 0f;
    public float maxVelocity = 0f;

    public NavMeshAgent navMeshAgent;

    private Vector3 _targetPos = Vector3.zero;
    public Vector3 targetPos
    {
        get
        {
            return _targetPos;
        }
        set
        {
            if(_targetPos != value)
            {
                //Debug.Log($"Move To ({value.x}, {value.y})");

                MoveToPosition(_targetPos);
                _targetPos = value;
            }
        }
    }
        
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _targetPos = transform.position;
    }

    void Update()
    {
        if (transform.position != _targetPos)
        {
            MoveToPosition(_targetPos);
        }
    }

    public void MoveToPosition(Vector3 _targetPos_)
    {
        _targetPos_.z = transform.position.z; // 保持Z轴不变
        navMeshAgent.SetDestination(_targetPos_);
        //transform.position = Vector3.MoveTowards(transform.position, _targetPos_, maxVelocity * Time.deltaTime);
    }
}
