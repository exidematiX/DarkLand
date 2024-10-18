using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]private Vector3[] points;


    public Vector3[] Points => points; //使用属性来实现只读访问
    public Vector3 CurrentPositions => _currentPositions;

    private Vector3 _currentPositions;
    private bool _gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _gameStarted = true;
        _currentPositions = transform.position;
    }

    public Vector3 GetWaypointPosition(int index)
    {
        return CurrentPositions + points[index];
    }


    //绘制方便开发者查看的视觉信息
    private void OnDrawGizmos()
    {
        if(!_gameStarted && transform.hasChanged)
        {
            _currentPositions = transform.position;
        }


        for(int i = 0; i < points.Length; i++) 
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(points[i]+ _currentPositions, 0.5f);


            if(i < points.Length - 1)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(points[i] + _currentPositions, points[i + 1] + _currentPositions);
            }
        }
    }
}
