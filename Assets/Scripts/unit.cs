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
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = Vector2.zero;
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, direction, ground);
            if (hit.collider != null)
            {
                agent.SetDestination(hit.point);
            }
            else
            {
                Debug.Log("no ground");
            }
        }
    }
}
