using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeLeon : MonoBehaviour
{
    //此脚本为资源点逻辑
    public enum ResourceType { light};//资源类型
    public ResourceType type;
    int resourceAmount = 10;

    public int Harvest(int amount)//挖矿
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
