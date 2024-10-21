using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int lives = 10;
    
    public int TotalLives { get; set; }

    private void Start()
    {
        TotalLives = lives;
    }

    private void ReduceLives(Enemy _enemy)
    {
        TotalLives--;

        if(TotalLives <= 0)
        {
            TotalLives = 0;
        }
    }

    private void OnEnable()
    {
        Enemy.OnEndReached += ReduceLives;
    }


    private void OnDisable()
    {
        Enemy.OnEndReached -= ReduceLives;
    }
}
