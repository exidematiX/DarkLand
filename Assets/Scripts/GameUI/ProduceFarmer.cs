using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class ProduceFarmer : MonoBehaviour
{
    public Transform baseTransform;

    [SerializeField,Min(0.0f)]
    public float spawnRange = 0.5f;
    void Start()
    {
        baseTransform = GameObject.FindGameObjectsWithTag("Base")[0].transform;
    }

    public void OnClickToProduceFarmer()
    {
        if (!ResourceManagerLeon.instance.SpendResource("light", 5))
            return;
        
        var targetPosition = baseTransform.position;
        targetPosition.x += Random.Range(-spawnRange,spawnRange);
        targetPosition.y += Random.Range(-spawnRange,spawnRange);
        
        Instantiate(Resources.Load("Prefabs/CreaturePrefabs/FarmerObj"), targetPosition, quaternion.identity);
        //Instantiate(Resources.Load("Prefabs/CreaturePrefabs/FarmerObj"));
    }
}
