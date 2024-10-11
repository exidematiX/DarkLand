using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class unit : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    LayerMask ground;
    public GameObject target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));  // 2D游戏通常z为0
            agent.SetDestination(worldPos);
        }
       // agent.SetDestination(target.transform.position);
    }
}
