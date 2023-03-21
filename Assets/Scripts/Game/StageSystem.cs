using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class StageSystem : MonoBehaviour
{
    [SerializeField] private Projectiles projectiles;

    [SerializeField] private Transform bossPrefab;
    [SerializeField] private Transform bossSpawnPos;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CoinSpawner coinSpawner;

    [SerializeField] private int stage = 1;

    [SerializeField] private AnimationCurve enemySpawnByStage;
    [FormerlySerializedAs("bossLaserChangeByStage")] [SerializeField] private AnimationCurve bossLaserChanceByStage;

    private bool isBossActive;
    public int GetStage => stage;

    public int GetLaserFireCount => projectiles.GetLaserFiredCount;

    public float GetEnemySpawnRate => enemySpawnByStage.Evaluate(stage);
    public float GetBossLaserChance => bossLaserChanceByStage.Evaluate(stage);


    private void OnEnable()
    {
        Projectiles.OnLaserStopped += AddStage;
        Boss.OnBossDeath += BossIsDead;
    }

    private void OnDisable()
    {
        Projectiles.OnLaserStopped -= AddStage;
        Boss.OnBossDeath -= BossIsDead;
    }

    public static event Action OnStageChanged;


    private void AddStage()
    {
        if (projectiles.GetLaserFiredCount % 2 != 0) return;

        if (!isBossActive)
        {
            stage++;
            if (stage % 2 == 0) // her 4 stagede bir boss cagır
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

    private void RestartSpawnings()
    {
        stage++;
        OnStageChanged?.Invoke();
        StartCoroutine(StopSpawningForAWhileCo());
    }

    private IEnumerator StopSpawningForAWhileCo()
    {
        enemySpawner.StopSpawning();
        coinSpawner.StopSpawning();
        // 5 saniye stage molası 4 saaniye burda 1 saniye spawn fonksiyonu içinde
        yield return new WaitForSeconds(4);
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

    private void BossIsDead()
    {
        isBossActive = false;
        Invoke(nameof(AddStage),0);
        // Her stage arası 5 saniye beklenecek
        // 1 saniye spawn süresinden geliyor
        // 4 saniey coroutine de harcanıyor
        // 0 saniye burada bekletiliyor
    }
}