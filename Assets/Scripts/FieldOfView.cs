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
    [SerializeField] private float viewDistance;
    public Dictionary<GameObject, Vector3> originsDic;
    //public Vector3 origin;
    public float startingAngle;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "mask";
        fov = 360f;
        viewDistance = 10f;
        originsDic = new Dictionary<GameObject, Vector3>();
        //origin = Vector3.zero;
    }

    //private void LateUpdate()
    //{
    //    int rayCount = 360;
    //    float angle = 0;
    //    float angleIncrease = fov / rayCount;

    //    Vector3[] vertices = new Vector3[originsDic.Count * (rayCount + 1 + 1)];
    //    Vector2[] uv = new Vector2[vertices.Length];
    //    int[] triangles = new int[originsDic.Count * rayCount * 3];

    //    //Vector3[] vertices = new Vector3[rayCount + 1 + 1];
    //    //Vector2[] uv = new Vector2[vertices.Length];
    //    //int[] triangles = new int[rayCount * 3];

    //    int count = 0;
    //    foreach (var origin in originsDic)
    //    {
    //        vertices[0 + count * (rayCount + 1 + 1)] = origin.Value;

    //        int vertexIndex = 1;
    //        int triangleIndex = 0;

    //        angle = 0;
    //        for (int i = 0; i <= rayCount; i++)
    //        {
    //            Vector3 vertex;
    //            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin.Value, GetVectorFromAngle(angle), viewDistance, layerMask);
    //            if (raycastHit2D.collider == null)
    //            {
    //                // No hit
    //                //Debug.Log("No hit");
    //                vertex = origin.Value + GetVectorFromAngle(angle) * viewDistance;
    //            }
    //            else
    //            {
    //                //Debug.Log("Hit object");
    //                vertex = raycastHit2D.point;
    //            }
    //            vertices[vertexIndex + count * (rayCount + 1 + 1)] = vertex;

    //            if (i > 0)
    //            {
    //                triangles[count * rayCount * 3 + triangleIndex + 0] = count * rayCount * 3 + 0;
    //                triangles[count * rayCount * 3 + triangleIndex + 1] = count * rayCount * 3 + vertexIndex - 1;
    //                triangles[count * rayCount * 3 + triangleIndex + 2] = count * rayCount * 3 + vertexIndex;

    //                triangleIndex += 3;
    //            }

    //            vertexIndex++;
    //            angle -= angleIncrease;
    //        }
    //        count++;
    //    }
    //    mesh.vertices = vertices;
    //    mesh.uv = uv;
    //    mesh.triangles = triangles;
    //    //mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    //    //vertices[0] = origin;

    //    //int vertexIndex = 1;
    //    //int triangleIndex = 0;
    //    //for (int i = 0; i <= rayCount; i++)
    //    //{
    //    //    Vector3 vertex;
    //    //    RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance, layerMask);
    //    //    if (raycastHit2D.collider == null)
    //    //    {
    //    //        // No hit
    //    //        Debug.Log("No hit");
    //    //        vertex = origin + GetVectorFromAngle(angle) * viewDistance;
    //    //    }
    //    //    else
    //    //    {
    //    //        Debug.Log("Hit object");
    //    //        vertex = raycastHit2D.point;
    //    //    }
    //    //    vertices[vertexIndex] = vertex;

    //    //    if (i > 0)
    //    //    {
    //    //        triangles[triangleIndex + 0] = 0;
    //    //        triangles[triangleIndex + 1] = vertexIndex - 1;
    //    //        triangles[triangleIndex + 2] = vertexIndex;

    //    //        triangleIndex += 3;
    //    //    }

    //    //    vertexIndex++;
    //    //    angle -= angleIncrease;
    //    //    mesh.vertices = vertices;
    //    //}
    //    //mesh.vertices = vertices;
    //    //mesh.uv = uv;
    //    //mesh.triangles = triangles;
    //    //mesh.bounds = new Bounds(origin, Vector3.one * 1000f);
    //}

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
                RaycastHit2D raycastHit2D = Physics2D.Raycast(origin.Value, GetVectorFromAngle(angle), viewDistance, layerMask);
                if (raycastHit2D.collider == null)
                {
                    vertex = origin.Value + GetVectorFromAngle(angle) * viewDistance;
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
                Debug.Log($"成功修改:{objOriginPair.Key.name}, {objOriginPair.Value}");
            }
        }
        else
        {
            originsDic.Add(objOriginPair.Key, objOriginPair.Value);
            Debug.Log($"成功添加:{objOriginPair.Key.name}, {objOriginPair.Value}");
        }

    }
}
