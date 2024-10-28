using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManagerLeon : MonoBehaviour
{
    public GameObject[] buildingPrefabs;//可建造的建筑预制体数组
    private int selectedBuildIndex = -1;//建筑索引
    GameObject currentPreview;
    public LayerMask unbuildingMask;//不可放置图层

    void SelectedBuild(int index)//选择建造建筑
    {
        selectedBuildIndex = index;
        StartBuildPreview();
    }

    void StartBuildPreview()//开始预览建筑
    {
        if (selectedBuildIndex < 0 || selectedBuildIndex >= buildingPrefabs.Length)
            return;

        currentPreview = Instantiate(buildingPrefabs[selectedBuildIndex]);//实例化预览建造
        currentPreview.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
    }
    private void Update()
    {
        if (currentPreview != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;
            currentPreview.transform.position = worldPosition;

            bool isPlacement = IsPlacement(worldPosition);
            currentPreview.GetComponent<Renderer>().material.color = isPlacement ? Color.green : Color.red;//可建造是绿色，不可是红色

            if (Input.GetMouseButtonDown(0) && isPlacement)
            {
                PlaceBuild();
            }
        }
    }

    private void PlaceBuild()
    {
        if (selectedBuildIndex < 0 || currentPreview == null) return;

        // 生成正式的建筑物
        Instantiate(buildingPrefabs[selectedBuildIndex], currentPreview.transform.position, Quaternion.identity);

        // 取消预览
        Destroy(currentPreview);
        currentPreview = null;
        selectedBuildIndex = -1;
    }

    private bool IsPlacement(Vector3 position)//能否放置
    {
        float buildSize = currentPreview.transform.localScale.x / 2;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, buildSize, unbuildingMask);//以buildsize为半径侦测范围内的object
        return colliders.Length == 0;
    }

    public void button1()
    {
        SelectedBuild(0);
    }
}
