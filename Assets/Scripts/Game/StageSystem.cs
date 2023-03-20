using System;
using System.Collections;
using UnityEngine;

public class StageSystem : MonoBehaviour
{
    [SerializeField] private Transform bossPrefab;
    [SerializeField] private Transform bossSpawnPos;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CoinSpawner coinSpawner;

    [SerializeField] private int stage;

    private bool isBossActive;

    private void OnEnable()
    {
        Projectiles.OnLaserStopped += AddStage;
        Boss.OnBossDeath += SetBossActiveToFalse; // 1
        Boss.OnBossDeath += AddStage; // 2
    }

    private void OnDisable()
    {
        Projectiles.OnLaserStopped -= AddStage;
        Boss.OnBossDeath -= SetBossActiveToFalse;
        Boss.OnBossDeath -= AddStage;
    }

    public static event Action OnStageChanged;

    private void AddStage()
    {
        if (isBossActive)
            return;

        stage++;

        if (stage % 2 == 0)
        {
            enemySpawner.StopSpawning();
            OnStageChanged?.Invoke();
            SpawnBoss();
        }
        else
        {
            StartCoroutine(StopSpawningForAWhileCo());
        }
    }

    private IEnumerator StopSpawningForAWhileCo()
    {
        enemySpawner.StopSpawning();
        coinSpawner.StopSpawning();
        yield return new WaitForSeconds(1);
        enemySpawner.StartSpawning();
        coinSpawner.StartSpawning();
    }

    private void SpawnBoss()
    {
        if (isBossActive)
            return;

        isBossActive = true;
        Instantiate(bossPrefab, bossSpawnPos.position, Quaternion.identity);
    }

    private void SetBossActiveToFalse()
    {
        isBossActive = false;
    }
}