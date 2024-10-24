using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeLeon : MonoBehaviour
{
    //�˽ű�Ϊ��Դ���߼�
    public enum ResourceType { light};//��Դ����
    public ResourceType type;
    int resourceAmount = 10;

    public int Harvest(int amount)//�ڿ�
    {
        if(resourceAmount<=0)
        {
            return 0;
        }
        int SpendAmount = Mathf.Min(resourceAmount, amount);
        resourceAmount -= SpendAmount;
        if(resourceAmount<=0)
        {
            Destroy(gameObject);
        }
        return SpendAmount;
    }
}
