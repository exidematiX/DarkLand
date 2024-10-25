using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRectRendering : MonoBehaviour
{
    private Mesh mesh;
    private MeshRenderer meshRenderer;

    public Vector3[] vertices = new Vector3[4];
    public Vector2[] uv = new Vector2[4];
    public int[] triangles = new int[6];
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "mask";

        this.gameObject.SetActive(false);
    }

    void Update()
    {
        //Vector3[] vertices = new Vector3[4];
        //Vector2[] uv = new Vector2[vertices.Length];
        //int[] triangles = new int[6];

        if (PlayerController.Instance.onDrawingRect)
        {
            float Xmin = Mathf.Min(PlayerController.Instance.startPoint.x, PlayerController.Instance.currentPoint.x);
            float Xmax = Mathf.Max(PlayerController.Instance.startPoint.x, PlayerController.Instance.currentPoint.x);
            float Ymin = Mathf.Min(PlayerController.Instance.startPoint.y, PlayerController.Instance.currentPoint.y);
            float Ymax = Mathf.Max(PlayerController.Instance.startPoint.y, PlayerController.Instance.currentPoint.y);

            //Vector3 startWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Xmin, Ymin, 0));
            //Vector3 endWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Xmax, Ymax, 0));
            Vector3 startWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Xmin, Ymin, Camera.main.nearClipPlane));
            Vector3 endWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Xmax, Ymax, Camera.main.nearClipPlane));

            vertices[0] = new Vector3(startWorldPos.x, startWorldPos.y, Camera.main.nearClipPlane); 
            vertices[1] = new Vector3(endWorldPos.x, startWorldPos.y, Camera.main.nearClipPlane);
            vertices[2] = new Vector3(startWorldPos.x, endWorldPos.y, Camera.main.nearClipPlane);
            vertices[3] = new Vector3(endWorldPos.x, endWorldPos.y, Camera.main.nearClipPlane); 

            triangles[0] = 0;
            triangles[1] = 3;
            triangles[2] = 1;

            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();

            this.gameObject.SetActive(true);
            Debug.Log($"vertices[0]：{vertices[0]}，vertices[1]：{vertices[1]}，vertices[2]：{vertices[2]}，vertices[3]：{vertices[3]}");
            
            //gameObject.GetComponent<MeshFilter>().mesh = mesh;
        }
    }

    private void OnDisable()
    {
        vertices[0] = Vector3.zero;
        vertices[1] = Vector3.zero;
        vertices[2] = Vector3.zero;
        vertices[3] = Vector3.zero;


        triangles[0] = 0;
        triangles[1] = 0;
        triangles[2] = 0;

        triangles[3] = 0;
        triangles[4] = 0;
        triangles[5] = 0;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        //mesh.RecalculateBounds();
    }
}

