using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BaseData basetower;
    bool isstop = false;
    public GameObject startUI;
    public GameObject endUI;
    float timer = 0f;
    public float gameDuration = 120f; // 游戏时长，单位为秒
    public GameObject UImanager;
    private void Start()
    {
        isstop = true;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= gameDuration)
        {
            GameOver();
        }
        if (basetower.Health == 0)
            GameOver();
        Time.timeScale = isstop ? 0 : 1;
    }


    void GameOver()
    {
        isstop = true;
        UImanager.SetActive(false);
        endUI.SetActive(true);
    }

    public void GameStart()
    {
        isstop = false;
        UImanager.SetActive(true);
        startUI.SetActive(false);
    }
}
