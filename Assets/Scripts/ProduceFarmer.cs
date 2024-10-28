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
        var targetTransform = baseTransform;
        var targetPosition = targetTransform.position;
        targetPosition.x += Random.Range(0.0f,5.0f);
        targetPosition.y += Random.Range(0.0f,5.0f);
        targetTransform.position = targetPosition;
        Instantiate(Resources.Load("Prefabs/CreaturePrefabs/FarmerObj"), targetTransform);
        //Instantiate(Resources.Load("Prefabs/CreaturePrefabs/FarmerObj"));
    }
}
