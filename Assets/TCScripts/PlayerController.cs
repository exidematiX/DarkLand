using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/*
 * 唐氏操作系统---by tang_cong
 * 
 * 按住鼠标左键画框，框中的人物被选中
 * 点按鼠标右键发号施令，默认是非跟随状态，鼠标右键点击要清空编队
 * 
 * 鼠标放在屏幕边缘就可移动摄像机
 * 按住R摄像机跟随编队，此时处于跟随状态，鼠标右键点击不清空编队，松开R键取消跟随状态
 * 点按R摄像机立即移动到编队
 * 点按H摄像机立即移动回主基地
 * 
 * 2024/10/15
 */
public enum CameraStatus
{
    Holding,
    Free
}

public class Selector
{
    public float Xmin;
    public float Xmax;
    public float Ymin;
    public float Ymax;

    //构造函数，在创建选定框时自动计算Xmin/Xmax/Ymin/Ymax
    public Selector(Vector3 start, Vector3 end)
    {
        Xmin = Mathf.Min(start.x, end.x);
        Xmax = Mathf.Max(start.x, end.x);
        Ymin = Mathf.Min(start.y, end.y);
        Ymax = Mathf.Max(start.y, end.y);
    }
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public bool onDrawingRect;//是否正在画框(即鼠标左键处于按住的状态)

    public Vector3 startPoint;//框的起始点，即按下鼠标左键时指针的位置
    public Vector3 currentPoint;//在拖移过程中，玩家鼠标指针所在的实时位置
    public Vector3 endPoint;//框的终止点，即放开鼠标左键时指针的位置

    public List<GameObject> chosenObjs = new List<GameObject>();

    public Material GLRectMat;//绘图的材质，在Inspector中设置
    public Color GLRectColor;//矩形的内部颜色，在Inspector中设置
    public Color GLRectEdgeColor;//矩形的边框颜色，在Inspector中设置

    [Header("设置单位之间的间隔")]
    public float unitSpacing = 1.0f;

    [Header("摄像机移动参数")]
    public GameObject mainCamera;
    public float panSpeed = 20f; // 摄像机平移速度
    public float edgeSize = 10f; // 边缘检测的阈值（例如 10 像素）
    public Vector2 minBounds; // 摄像机的移动范围最小值
    public Vector2 maxBounds; // 摄像机的移动范围最大值
    public CameraStatus cameraStatus = CameraStatus.Free;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        RectChoose();
        IssueOrder();
        CameraEdgeMov();
        CameraReset();
    }

    void Start()
    {
        mainCamera = Camera.main.gameObject;
    }

    public void RectChoose()
    {
        //玩家按下鼠标左键，此时进入画框状态，并确定框的起始点
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            onDrawingRect = true;
            startPoint = Input.mousePosition;
            //Debug.LogFormat("开始画框，起点:{0}", startPoint);
        }

        //在鼠标左键未放开时，实时记录鼠标指针的位置
        if (onDrawingRect)
        {
            currentPoint = Input.mousePosition;
        }

        //玩家放开鼠标左键，说明框画完，确定框的终止点，退出画框状态
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            endPoint = Input.mousePosition;
            onDrawingRect = false;
            //Debug.LogFormat("画框结束，终点:{0}", endPoint);

            Selector selector = new Selector(startPoint, endPoint);
            CheckWhetherInRect(selector, "Friendly Creature", ref chosenObjs);
        }
    }
    
    void CheckWhetherInRect(Selector _selector_, string _tag_, ref List<GameObject> chosenObjs)
    {
        List<GameObject> allObjWithTag = GameObject.FindGameObjectsWithTag(_tag_).ToList();

        foreach (GameObject obj in allObjWithTag)
        {
            Vector3 cameraPos = Camera.main.WorldToScreenPoint(obj.transform.position);
            if (cameraPos.x > _selector_.Xmin && cameraPos.x < _selector_.Xmax && cameraPos.y > _selector_.Ymin && cameraPos.y < _selector_.Ymax)
            {
                if(obj.GetComponent<CreatureData>()?.IsChosen != true)
                {
                    chosenObjs.Add(obj);
                    if(obj.GetComponent<CreatureData>() != null)
                    {
                        obj.GetComponent<CreatureData>().IsChosen = true;
                    }
                }

                //obj.transform.Find("Canvas/ChosenMark")?.gameObject.SetActive(true);

                //Debug.Log($"{obj.name}被选中了啊！！！");
            }
        }
    }
    
    void IssueOrder()
    {
        if (Input.GetMouseButtonDown(1) )
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Debug.Log($"MouseWorldPos({mouseWorldPos.x}, {mouseWorldPos.y})");

            //foreach (GameObject obj in chosenObjs)
            //{
            //    obj.GetComponent<CreatureMove>().targetPos = mouseWorldPos;
            //}
            MoveUnitsInFormation(mouseWorldPos);

            if (cameraStatus == CameraStatus.Free)
            {
                ClearChosenList();
            }
        }
    }

    // 将单位排列成一个方阵并移动
    void MoveUnitsInFormation(Vector3 destination)
    {
        int unitCount = chosenObjs.Count;
        int sideCount = Mathf.CeilToInt(Mathf.Sqrt(unitCount)); // 计算方阵的边长

        //float unitSpacing = 1.0f; 
        Vector3 formationStart = 
            destination - new Vector3((sideCount - 1) * unitSpacing / 2, (sideCount - 1) * unitSpacing / 2, 0); // 从一个左下角开始排列

        int i = 0;
        for (int x = 0; x < sideCount; x++)
        {
            for (int y = 0; y < sideCount; y++)
            {
                if (i >= unitCount) break; // 如果所有单位都已分配位置，停止循环

                Vector3 offsetPosition = formationStart + new Vector3(x * unitSpacing, y * unitSpacing, 0);
                //chosenObjs[i].GetComponent<NavMeshAgent>().SetDestination(offsetPosition); // 移动单位
                if(chosenObjs[i].GetComponent<CreatureMove>() != null)
                {
                    chosenObjs[i].GetComponent<CreatureMove>().targetPos = offsetPosition;
                    i++;
                }
            }
        }
    }

    void ClearChosenList()
    {
        foreach (GameObject obj in chosenObjs)
        {
            if(obj.GetComponent<CreatureData>() != null)
            {
                obj.GetComponent<CreatureData>().IsChosen = false;
            }
        }
        chosenObjs.Clear();
    }

    public void CameraEdgeMov()
    {
        Vector3 cameraMovement = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;

        // 获取屏幕宽高
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 检测鼠标是否靠近屏幕的边缘
        if (mousePos.x > screenWidth - edgeSize)
        {
            cameraMovement.x += 1; 
            Debug.Log("右移");
        }
        else if (mousePos.x < edgeSize)
        {
            cameraMovement.x -= 1; 
            Debug.Log("左移");
        }

        if (mousePos.y > screenHeight - edgeSize)
        {
            cameraMovement.y += 1; 
            Debug.Log("上移");
        }
        else if (mousePos.y < edgeSize)
        {
            cameraMovement.y -= 1; 
            Debug.Log("下移");
        }

        // 移动摄像机
        mainCamera.transform.position += cameraMovement * panSpeed * Time.deltaTime;

        // 限制摄像机在指定范围内移动
        //Vector3 pos = mainCamera.transform.position;
        //pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        //pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        //mainCamera.transform.position = pos;
    }

    public void CameraReset()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        { 
            mainCamera.transform.position = FindChosenObjMiddiumPos();
        }
        else if (Input.GetKey(KeyCode.R))
        {
            mainCamera.transform.position = FindChosenObjMiddiumPos();
            cameraStatus = CameraStatus.Holding;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            cameraStatus = CameraStatus.Free;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            mainCamera.transform.position = new Vector3(0, 0, mainCamera.transform.position.z);
        }
    }

    public Vector3 FindChosenObjMiddiumPos()
    {
        Vector3 middiumPos = Vector3.zero;
        middiumPos.z = mainCamera.transform.position.z;

        if (chosenObjs.Count > 0)
        {
            foreach (var obj in chosenObjs)
            {
                middiumPos.x += obj.transform.position.x;
                middiumPos.y += obj.transform.position.y;
            }
            middiumPos.x /= chosenObjs.Count;
            middiumPos.y /= chosenObjs.Count;
        }
        else
        {
            middiumPos = new Vector3(0, 0, mainCamera.transform.position.z);
        }

        return middiumPos;
    }
    
}
