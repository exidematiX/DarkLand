using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProduceFarmer : MonoBehaviour
{
    public Transform baseTransform;
    void Start()
    {
        baseTransform = GameObject.FindGameObjectsWithTag("Base")[0].transform;
    }

    public void OnClickToProduceFarmer()
    {
        Debug.Log("扣除资源");
        Instantiate(Resources.Load("Prefabs/CreaturePrefabs/FarmerObj"), baseTransform);
        //Instantiate(Resources.Load("Prefabs/CreaturePrefabs/FarmerObj"));
    }
}
