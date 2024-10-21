using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定义生成模式的枚举
public enum SpawnModes
{
    Fixed, // 固定模式
    Random // 随机模式
}

public class Spawner : MonoBehaviour
{
    // 序列化字段，以便在Unity编辑器中设置

    [Header("Settings")]
    [SerializeField] private SpawnModes spawnModes = SpawnModes.Fixed; // 选择生成模式，默认为固定生成
    [SerializeField] private int enemyCount = 10; // 要生成的敌人总数量
    [SerializeField] private float delayBtwWaves = 1f; 

    //[SerializeField] private GameObject testGO; // 要生成的敌人对象


    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns; // 两次生成之间的固定延迟时间


    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay; // 随机延迟的最小值
    [SerializeField] private float maxRandomDelay; // 随机延迟的最大值



    private float _spawnTimer; // 用于记录生成计时器的时间
    private float _enemiesSpawned; // 已生成的敌人数目
    private float _enemiesRamaining; 



    private ObjectPooler _pooler;
    private Waypoint _waypoint;

    private void Start()
    {
        _pooler = GetComponent<ObjectPooler>();
        _waypoint = GetComponent<Waypoint>();

        _enemiesRamaining = enemyCount;

    }

    // 每帧调用一次
    void Update()
    {
        _spawnTimer -= Time.deltaTime; // 减去经过的时间，使计时器倒计时
        if (_spawnTimer < 0) // 如果计时器时间到了
        {
            _spawnTimer = GetSpawnDelay(); // 重置计时器为一个新生成的随机时间
            if (_enemiesSpawned < enemyCount) // 如果已生成敌人数少于目标数量
            {
                _enemiesSpawned++; // 增加生成敌人数计数
                SpawnEnemy(); // 调用生成敌人函数
            }
        }
    }

    // 生成敌人对象的方法
    private void SpawnEnemy()
    {

        GameObject newInstance = _pooler.GetInstanceFromPool();

        Enemy enemy = newInstance.GetComponent<Enemy>();
        enemy.waypoint = _waypoint;
        enemy.ResetEnemy();

        enemy.transform.localPosition = transform.position;

        newInstance.SetActive(true);

        //Instantiate(testGO, transform.position, Quaternion.identity); // 在当前对象位置生成敌人对象
    }


    private float GetSpawnDelay()
    {
        float delay = 0f;
        if(spawnModes == SpawnModes.Fixed)
        {
            delay = delayBtwSpawns;
        }
        else
        {
            delay = GetRandomDelay();
        }
        return delay;
    }

    // 获取一个随机的延迟时间
    private float GetRandomDelay()
    {
        float randomTimer = Random.Range(minRandomDelay, maxRandomDelay); // 生成一个 min 和 max 范围内的随机浮点数
        return randomTimer; // 返回随机延迟时间
    }


    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBtwSpawns);
        _enemiesRamaining = enemyCount;
        _spawnTimer = 0f;
        _enemiesSpawned = 0;
    }

    private void RecordEnemy(Enemy _enemy)
    {
        _enemiesRamaining--;
        if(_enemiesRamaining <= 0)
        {
            StartCoroutine(NextWave());
        }
    }
    private void OnEnable()
    {
        Enemy.OnEndReached += RecordEnemy;
        EnemyHealth.OnEnemyKilled += RecordEnemy;
    }

    private void OnDisable()
    {
        Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyKilled -= RecordEnemy;
        
    }
}
