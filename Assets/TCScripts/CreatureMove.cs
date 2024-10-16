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

    private Vector2 lastMoveDirection;
    private Animator animator;
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
        animator = transform.Find("CharacterImage").gameObject.GetComponent<Animator>();
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
        PlayWalkAnim();
    }

    public void PlayWalkAnim()
    {
        // 获取移动方向的向量
        Vector3 velocity = navMeshAgent.velocity;

        // 将世界坐标转换为本地坐标（根据角色朝向）
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);


        // 设置Animator参数
        animator.SetFloat("moveX", localVelocity.x);
        animator.SetFloat("moveY", localVelocity.y);

        // 如果角色向左移动，翻转Sprite
        if (localVelocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (localVelocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // 控制行走动画是否播放
        if (velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
            lastMoveDirection = new Vector2(localVelocity.x, localVelocity.y);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("moveX", lastMoveDirection.x);
            animator.SetFloat("moveY", lastMoveDirection.y);
            // 当角色停止时，根据最后移动方向播放对应站立动画
            //if (lastMoveDirection.y > 0.1f)
            //{
            //    animator.Play("Idle_Up");
            //}
            //else if (lastMoveDirection.y < -0.1f)
            //{
            //    animator.Play("Idle_Down");
            //}
            //else if (Mathf.Abs(lastMoveDirection.x) > 0.1f)
            //{
            //    animator.Play("Idle_LeftRight");

            //    // 翻转角色朝向左或右
            //    if (lastMoveDirection.x < 0)
            //    {
            //        transform.localScale = new Vector3(-1, 1, 1); // 朝左
            //    }
            //    else
            //    {
            //        transform.localScale = new Vector3(1, 1, 1); // 朝右
            //    }
            //}
        }
    }
}
