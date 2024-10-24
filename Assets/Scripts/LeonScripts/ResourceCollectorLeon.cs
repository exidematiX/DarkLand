using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using JetBrains.Annotations;

public class ResourceConllectorLeon : MonoBehaviour
{
    //�˽ű�������Ϊ������Ϊ�߼�
    public int carryCapactiy = 10;//���˱�����С
    public int carriedCapactiy = 0;//��Я������
    public ResourceNodeLeon.ResourceType carriedResourceType;//��������Ʒ����
    NavMeshAgent agent;

    public ResourceNodeLeon currentTarget;//���ڵ�Ŀ��
    Transform baseLocation;//��������

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        baseLocation = FindObjectOfType<Base>().transform;
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

    private void OnTriggerEnter2D(Collider2D other)//�ɼ���Դ������ش洢
    {
        if (other.gameObject.CompareTag("Resource") && carriedCapactiy < carryCapactiy)//�ɼ���Դ
        {
            currentTarget = other.GetComponent<ResourceNodeLeon>();
            int havesedAmount = currentTarget.Harvest(carryCapactiy - carriedCapactiy);
            carriedCapactiy += havesedAmount;
            carriedResourceType = currentTarget.type;
        }
        else if (other.gameObject.CompareTag("Base") && carriedCapactiy > 0)//���ػ�����ձ���
        {
            ResourceManagerLeon.instance.AddResource(carriedResourceType.ToString(), carriedCapactiy);
            carriedCapactiy = 0;
        }
    }

    public void SetResourceTarget(ResourceNodeLeon target)//���òɼ�Ŀ��
    {
        currentTarget = target;
    }
}
