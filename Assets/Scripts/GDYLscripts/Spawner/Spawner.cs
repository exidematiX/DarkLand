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
    [Header("Settings")]
    [SerializeField] private SpawnModes spawnModes = SpawnModes.Fixed; // 选择生成模式，默认为固定生成
    [SerializeField] private int enemyCount = 10; // 要生成的敌人总数量
    [SerializeField] private float delayBtwWaves = 1f;

    [Header("Fixed Delay")]
    [SerializeField] private float delayBtwSpawns; // 两次生成之间的固定延迟时间

    [Header("Random Delay")]
    [SerializeField] private float minRandomDelay; // 随机延迟的最小值
    [SerializeField] private float maxRandomDelay; // 随机延迟的最大值

    private float _spawnTimer; // 用于记录生成计时器的时间
    private float _enemiesSpawned; // 已生成的敌人数目
    private float _enemiesRamaining;

    private Pooler _pooler; // 假设你正在使用对象池以提高性能

    private void Start()
    {
        _pooler = GetComponent<Pooler>();

        _enemiesRamaining = enemyCount;
    }

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

    private void SpawnEnemy()
    {
        GameObject newInstance = _pooler.GetInstanceFromPool();

        EnemyAI enemyAI = newInstance.GetComponent<EnemyAI>();
        if (enemyAI != null)
        {
            // 设置敌人的目标和小镇中心
            enemyAI.targetTransform = FindTarget();
            enemyAI.townCenterTransform = FindTownCenter();

            // 将敌人状态初始化为巡逻
            enemyAI.CurrentState = EnemyState.Patrolling;

            // 初始化敌人的位置
            enemyAI.transform.localPosition = transform.position;
        }

        // 激活实例
        newInstance.SetActive(true);
    }

    private Transform FindTarget()
    {
        GameObject potentialTarget = GameObject.FindWithTag("Player");
        return potentialTarget != null ? potentialTarget.transform : null;
    }

    private Transform FindTownCenter()
    {
        GameObject townCenter = GameObject.FindWithTag("Base");
        return townCenter != null ? townCenter.transform : null;
    }

    private float GetSpawnDelay()
    {
        return spawnModes == SpawnModes.Fixed ? delayBtwSpawns : GetRandomDelay();
    }

    private float GetRandomDelay()
    {
        return Random.Range(minRandomDelay, maxRandomDelay);
    }

    private IEnumerator NextWave()
    {
        yield return new WaitForSeconds(delayBtwWaves);
        _enemiesRamaining = enemyCount;
        _spawnTimer = 0f;
        _enemiesSpawned = 0;
    }

    private void RecordEnemy(EnemyAI enemyAI)
    {
        _enemiesRamaining--;
        if (_enemiesRamaining <= 0)
        {
            StartCoroutine(NextWave());
        }
    }

    private void OnEnable()
    {
        //EnemyAI.OnEndReached += RecordEnemy;
        EnemyHealth.OnEnemyKilled += RecordEnemy;
    }

    private void OnDisable()
    {
        //Enemy.OnEndReached -= RecordEnemy;
        EnemyHealth.OnEnemyKilled -= RecordEnemy;

    }

}