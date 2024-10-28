using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using JetBrains.Annotations;

public class ResourceConllectorLeon : MonoBehaviour
{
    //此脚本挂载在为工人行为逻辑
    public int carryCapactiy = 10;//工人背包大小
    public int carriedCapactiy = 0;//已携带数量
    public ResourceNodeLeon.ResourceType carriedResourceType;//背包内物品种类
    NavMeshAgent agent;

    public ResourceNodeLeon currentTarget;//现在的目标
    Transform baseLocation;//基地坐标

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        baseLocation = FindObjectOfType<BaseData>().transform;
    }

    private void Update()
    {
        if (currentTarget != null && carryCapactiy > carriedCapactiy)
        {
            agent.SetDestination(currentTarget.transform.position);
        }
        else if (carriedCapactiy > 0)
        {
            agent.SetDestination(baseLocation.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)//采集资源或给基地存储
    {
        if (other.gameObject.CompareTag("Resource") && carriedCapactiy < carryCapactiy)//采集资源
        {
            currentTarget = other.GetComponent<ResourceNodeLeon>();
            int havesedAmount = currentTarget.Harvest(carryCapactiy - carriedCapactiy);
            carriedCapactiy += havesedAmount;
            carriedResourceType = currentTarget.type;
        }
        else if (other.gameObject.CompareTag("Base") && carriedCapactiy > 0)//返回基地清空背包
        {
            ResourceManagerLeon.instance.AddResource(carriedResourceType.ToString(), carriedCapactiy);
            carriedCapactiy = 0;
        }
    }

    public void SetResourceTarget(ResourceNodeLeon target)//设置采集目标
    {
        currentTarget = target;
    }
}
