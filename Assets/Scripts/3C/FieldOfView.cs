using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.UI.Image;

public class FieldOfView : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private float fov;
    [SerializeField] private float viewDefaultDistance;
    public Dictionary<GameObject, Vector3> originsDic;
    public Dictionary<GameObject, float> rangesDic;
    //public Vector3 origin;
    public float startingAngle;
    public static FieldOfView Instance;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "mask";
        fov = 360f;
        viewDefaultDistance = 10f;
        
        //origin = Vector3.zero;
    }

    private void Awake()
    {
        originsDic = new Dictionary<GameObject, Vector3>();
        rangesDic = new Dictionary<GameObject, float>();
        Instance = this;
    }

    private void LateUpdate()
    {
        int rayCount = 360;
        float angle = 0;
        float angleIncrease = fov / rayCount;

        // 顶点数组和三角形数组的大小需要足够容纳所有对象的顶点和三角形
        Vector3[] vertices = new Vector3[originsDic.Count * (rayCount + 1 + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[originsDic.Count * rayCount * 3];

        int vertexOffset = 0;  // 用于计算全局的顶点偏移
        int triangleOffset = 0;  // 用于计算全局的三角形偏移

        foreach (var origin in originsDic)
        {
            // 设置每个对象的中心顶点
            vertices[vertexOffset] = origin.Value;

            int vertexIndex = vertexOffset + 1;  // 从中心顶点的下一个开始
            angle = 0;

            for (int i = 0; i <= rayCount; i++)
            {
                Vector3 vertex;
                RaycastHit2D raycastHit2D = Physics2D.Raycast(origin.Value, GetVectorFromAngle(angle), rangesDic[origin.Key], layerMask);
                if (raycastHit2D.collider == null)
                {
                    vertex = origin.Value + GetVectorFromAngle(angle) * viewDefaultDistance;
                }
                else
                {
                    vertex = raycastHit2D.point;
                }
                vertices[vertexIndex] = vertex;

                // 计算三角形索引
                if (i > 0)
                {
                    triangles[triangleOffset + 0] = vertexOffset;  // 中心顶点
                    triangles[triangleOffset + 1] = vertexIndex - 1;
                    triangles[triangleOffset + 2] = vertexIndex;

                    triangleOffset += 3;
                }

                vertexIndex++;
                angle -= angleIncrease;
            }

            vertexOffset += rayCount + 1 + 1;  // 更新顶点偏移
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
    }

    public static Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(origin, viewDistance);
    //}

    //public void SetOrigin(Vector3 origin)
    //{
    //    this.origin = origin;
    //}
    public void SetOrigin(KeyValuePair<GameObject, Vector3> objOriginPair)
    {
        if (originsDic.ContainsKey(objOriginPair.Key))
        {
            if (!originsDic[objOriginPair.Key].Equals(objOriginPair.Value))
            {
                this.originsDic[objOriginPair.Key] = objOriginPair.Value;
                //Debug.Log($"成功修改:{objOriginPair.Key.name}, {objOriginPair.Value}");
            }
        }
        else
        {
            originsDic.Add(objOriginPair.Key, objOriginPair.Value);
            rangesDic.Add(objOriginPair.Key, viewDefaultDistance);
            //Debug.Log($"成功添加:{objOriginPair.Key.name}, {objOriginPair.Value}");
        }

    }

    public void SetOrigin(KeyValuePair<GameObject, Vector3> objOriginPair, float viewRange)
    {
        if (originsDic.ContainsKey(objOriginPair.Key))
        {
            //Debug.Log($"why");
            if (!originsDic[objOriginPair.Key].Equals(objOriginPair.Value))
            {
                this.originsDic[objOriginPair.Key] = objOriginPair.Value;
            }
        }
        else
        {
            Debug.Log($"成功添加:{objOriginPair.Key.name}, {objOriginPair.Value}, {viewRange}");
            originsDic.Add(objOriginPair.Key, objOriginPair.Value);
            rangesDic.Add(objOriginPair.Key, viewRange);
        }

    }
}
