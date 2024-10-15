using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManagerLeon : MonoBehaviour
{
    public GameObject[] buildingPrefabs;//�ɽ���Ľ���Ԥ��������
    private int selectedBuildIndex = -1;//��������
    GameObject currentPreview;
    public LayerMask unbuildingMask;//���ɷ���ͼ��

    void SelectedBuild(int index)//ѡ���콨��
    {
        selectedBuildIndex = index;
        StartBuildPreview();
    }

    void StartBuildPreview()//��ʼԤ������
    {
        if (selectedBuildIndex < 0 || selectedBuildIndex >= buildingPrefabs.Length)
            return;

        currentPreview = Instantiate(buildingPrefabs[selectedBuildIndex]);
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
            currentPreview.GetComponent<Renderer>().material.color = isPlacement ? Color.green : Color.red;

            if (Input.GetMouseButtonDown(0) && isPlacement)
            {
                PlaceBuild();
            }
        }
    }

    private void PlaceBuild()
    {
        if (selectedBuildIndex < 0 || currentPreview == null) return;

        // ������ʽ�Ľ�����
        Instantiate(buildingPrefabs[selectedBuildIndex], currentPreview.transform.position, Quaternion.identity);

        // ȡ��Ԥ��
        Destroy(currentPreview);
        currentPreview = null;
        selectedBuildIndex = -1;
    }

    private bool IsPlacement(Vector3 position)//�ܷ����
    {
        float buildSize = currentPreview.transform.localScale.x / 2;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, buildSize, unbuildingMask);//��buildsizeΪ�뾶��ⷶΧ�ڵ�object
        return colliders.Length == 0;
    }

    public void button1()
    {
        SelectedBuild(0);
    }
}
