using System;
using System.Collections;
using UnityEngine;

public class StageSystem : MonoBehaviour
{
    [SerializeField] private Projectiles projectiles;
    
    [SerializeField] private Transform bossPrefab;
    [SerializeField] private Transform bossSpawnPos;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CoinSpawner coinSpawner;

    [SerializeField] private int stage;
    public int GetStage => stage;

    public int GetLaserFireCount => projectiles.GetLaserFiredCount;

    [SerializeField] private AnimationCurve enemySpawnByStage;

    private bool isBossActive;

    public float GetEnemySpawnRate => enemySpawnByStage.Evaluate(stage);


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
        if (projectiles.GetLaserFiredCount % 2 != 0) return;

        if (!isBossActive)
        {
            stage++;
            if (stage % 4 == 0) // her 4 stagede bir boss cagır
            {
                enemySpawner.StopSpawning();
                SpawnBoss();
            }
            else
            {
                StartCoroutine(StopSpawningForAWhileCo());
            }

            OnStageChanged?.Invoke();
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