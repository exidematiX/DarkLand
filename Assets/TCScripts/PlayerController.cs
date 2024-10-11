using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
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

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        RectChoose();
        IssueOrder();
    }

    public void RectChoose()
    {
        //玩家按下鼠标左键，此时进入画框状态，并确定框的起始点
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            onDrawingRect = true;
            startPoint = Input.mousePosition;
            Debug.LogFormat("开始画框，起点:{0}", startPoint);
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
            Debug.LogFormat("画框结束，终点:{0}", endPoint);

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
                chosenObjs.Add(obj);

                obj.transform.Find("Canvas/ChosenMark")?.gameObject.SetActive(true);

                Debug.Log($"{obj.name}被选中了啊！！！");
            }
        }
    }
    
    void IssueOrder()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Debug.Log($"MouseWorldPos({mouseWorldPos.x}, {mouseWorldPos.y})");

            foreach (GameObject obj in chosenObjs)
            {
                obj.GetComponent<CreatureMove>().targetPos = mouseWorldPos;
            }
        }
    }
}
