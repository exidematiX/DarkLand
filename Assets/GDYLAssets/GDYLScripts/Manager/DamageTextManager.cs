using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : Singleton<DamageTextManager>
{
    public ObjectPooler Pooler { get; set; }

    //public static DamageTextManager Instance;


    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    else
    //    {
    //        Debug.LogError("Multiple DamageTextManager instances detected!", this);
    //        Destroy(gameObject); // 或其他处理方式
    //    }
    //}

    private void Start()
    {
        Pooler = GetComponent<ObjectPooler>();
        if (Pooler == null)
        {
            Debug.LogError("ObjectPooler not found on DamageTextManager!", this);
        }
    }

}
