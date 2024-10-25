using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawOnCamera : MonoBehaviour
{
    private CommandBuffer commandBuffer;
    private Mesh rectMesh;

    private void Start()
    {
        CreateRectMesh();  // 创建矩形的Mesh
    }

    private void OnEnable()
    {
        // 创建CommandBuffer
        commandBuffer = new CommandBuffer { name = "Draw Rect Command Buffer" };
        // 注册到渲染管线
        RenderPipelineManager.endCameraRendering += RenderRect;
    }

    private void OnDisable()
    {
        // 移除渲染事件
        RenderPipelineManager.endCameraRendering -= RenderRect;
        if (commandBuffer != null)
        {
            commandBuffer.Release();
        }
    }

    // 创建一个矩形的Mesh
    private void CreateRectMesh()
    {
        rectMesh = new Mesh();
        rectMesh.vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),   // 左下角
            new Vector3(1, 0, 0),   // 右下角
            new Vector3(1, 1, 0),   // 右上角
            new Vector3(0, 1, 0)    // 左上角
        };
        rectMesh.triangles = new int[]
        {
            0, 1, 2,  // 第一个三角形
            2, 3, 0   // 第二个三角形
        };
    }

    // 在渲染结束时绘制矩形框
    private void RenderRect(ScriptableRenderContext context, Camera camera)
    {
        if (camera != Camera.main || !PlayerController.Instance.onDrawingRect)
        {
            return;
        }

        // 清空之前的命令
        commandBuffer.Clear();

        // 获取矩形框的位置和尺寸
        float Xmin = Mathf.Min(PlayerController.Instance.startPoint.x, PlayerController.Instance.currentPoint.x);
        float Xmax = Mathf.Max(PlayerController.Instance.startPoint.x, PlayerController.Instance.currentPoint.x);
        float Ymin = Mathf.Min(PlayerController.Instance.startPoint.y, PlayerController.Instance.currentPoint.y);
        float Ymax = Mathf.Max(PlayerController.Instance.startPoint.y, PlayerController.Instance.currentPoint.y);

        // 确定矩形的中心位置和缩放比例
        Vector3 position = new Vector3((Xmin + Xmax) / 2, (Ymin + Ymax) / 2, 0);
        Vector3 scale = new Vector3(Xmax - Xmin, Ymax - Ymin, 1);

        // 创建矩阵
        Matrix4x4 matrix = Matrix4x4.TRS(position, Quaternion.identity, scale);

        // 绘制矩形
        commandBuffer.DrawMesh(rectMesh, matrix, PlayerController.Instance.GLRectMat, 0);

        // 执行命令
        context.ExecuteCommandBuffer(commandBuffer);
    }


    //private void OnPostRender()
    //{
    //    //Debug.Log("开始画框");
    //    DrawRect();
    //}


    //private void DrawRect()
    //{
    //    if (PlayerController.Instance.onDrawingRect)
    //    {
    //        Debug.Log("开始画框");
    //        //准备工作:获取确定矩形框各角坐标所需的各个数值
    //        float Xmin = Mathf.Min(PlayerController.Instance.startPoint.x, PlayerController.Instance.currentPoint.x);
    //        float Xmax = Mathf.Max(PlayerController.Instance.startPoint.x, PlayerController.Instance.currentPoint.x);
    //        float Ymin = Mathf.Min(PlayerController.Instance.startPoint.y, PlayerController.Instance.currentPoint.y);
    //        float Ymax = Mathf.Max(PlayerController.Instance.startPoint.y, PlayerController.Instance.currentPoint.y);

    //        GL.PushMatrix();//GL入栈
    //                        //在这里，只需要知道GL.PushMatrix()和GL.PopMatrix()分别是画图的开始和结束信号,画图指令写在它们中间
    //        if (!PlayerController.Instance.GLRectMat)
    //            return;

    //        PlayerController.Instance.GLRectMat.SetPass(0);//启用线框材质rectMat


    //        GL.LoadPixelMatrix();//设置用屏幕坐标绘图

    //        /*------第一步，绘制矩形------*/
    //        GL.Begin(GL.QUADS);//开始绘制矩形,这一段画的是框中间的半透明部分，不包括边界线

    //        GL.Color(PlayerController.Instance.GLRectColor);//设置矩形的颜色，注意GLRectColor务必设置为半透明

    //        //陈述矩形的四个顶点
    //        GL.Vertex3(Xmin, Ymin, 0);//陈述第一个点，即框的左下角点，记为点1
    //        GL.Vertex3(Xmin, Ymax, 0);//陈述第二个点，即框的左上角点，记为点2
    //        GL.Vertex3(Xmax, Ymax, 0);//陈述第三个点，即框的右上角点，记为点3
    //        GL.Vertex3(Xmax, Ymin, 0);//陈述第四个点，即框的右下角点，记为点4

    //        GL.End();//告一段落，此时画好了一个无边框的矩形


    //        /*------第二步，绘制矩形的边框------*/
    //        GL.Begin(GL.LINES);//开始绘制线，用来描出矩形的边框

    //        GL.Color(PlayerController.Instance.GLRectEdgeColor);//设置方框的边框颜色，建议设置为不透明的

    //        //描第一条边
    //        GL.Vertex3(Xmin, Ymin, 0);//起始于点1
    //        GL.Vertex3(Xmin, Ymax, 0);//终止于点2

    //        //描第二条边
    //        GL.Vertex3(Xmin, Ymax, 0);//起始于点2
    //        GL.Vertex3(Xmax, Ymax, 0);//终止于点3

    //        //描第三条边
    //        GL.Vertex3(Xmax, Ymax, 0);//起始于点3
    //        GL.Vertex3(Xmax, Ymin, 0);//终止于点4

    //        //描第四条边
    //        GL.Vertex3(Xmax, Ymin, 0);//起始于点4
    //        GL.Vertex3(Xmin, Ymin, 0);//返回到点1

    //        GL.End();//画好啦！
    //        GL.PopMatrix();//GL出栈
    //    }
    //}


}
