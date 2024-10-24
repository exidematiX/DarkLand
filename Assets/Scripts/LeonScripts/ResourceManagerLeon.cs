using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagerLeon : MonoBehaviour
{
    public static ResourceManagerLeon instance;
    public event Action OnResourceChanged;
    Dictionary<string, int> resource = new Dictionary<string, int>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        resource.Add("light", 0);
    }

    public bool AddResource(string key, int value)
    {
        if(!resource.ContainsKey(key))
        {
            return false;
        }
        else
        {
            resource[key] += value;
            return true;
        }
    }

    public bool SpendResource(string key,int value)
    {
        if (!resource.ContainsKey(key))
        {
            return false;
        }
        else
        {
            resource[key] -= value;
            return true;
        }
    }

    public int GetResourceAmount(string key)
    {
        return resource[key];
    }
}
