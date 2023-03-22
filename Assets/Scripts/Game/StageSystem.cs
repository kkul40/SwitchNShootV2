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

    [SerializeField] private int stage = 1;

    [SerializeField] private AnimationCurve enemySpawnByStage;

    [SerializeField] private AnimationCurve bossLaserChanceByStage;

    private bool isBossActive;
    private bool isStagePreparing;
    public int GetStage => stage;

    public int GetLaserFireCount => projectiles.GetLaserFiredCount;

    public float GetEnemySpawnRate => enemySpawnByStage.Evaluate(stage);
    public float GetBossLaserChance => bossLaserChanceByStage.Evaluate(stage);


    private void OnEnable()
    {
        Projectiles.OnLaserStopped += AddStage;
        Boss.OnBossLeave += StageReadyAfterBoss;
        Boss.OnBossDeath += BossIsDead;
    }

    private void OnDisable()
    {
        Projectiles.OnLaserStopped -= AddStage;
        Boss.OnBossLeave -= StageReadyAfterBoss;
        Boss.OnBossDeath -= BossIsDead;
    }

    public static event Action OnStageChanged;


    private void AddStage()
    {
        //if (projectiles.GetLaserFiredCount % 2 != 0) return;

        if (isBossActive || isStagePreparing) return;

        stage++;
        if (stage % 3 == 0) // her 4 stagede bir boss cagır
        {
            enemySpawner.StopSpawning();
            coinSpawner.StopSpawning();
            Invoke(nameof(SpawnBoss), 3f);
        }
        else
        {
            StartCoroutine(StopSpawningForAWhileCo());
        }

        isStagePreparing = true;
        OnStageChanged?.Invoke();
    }

    private IEnumerator StopSpawningForAWhileCo()
    {
        enemySpawner.StopSpawning();
        coinSpawner.StopSpawning();
        // 3 saniye stage molası 2 saaniye burda 1 saniye spawn fonksiyonu içinde
        yield return new WaitForSeconds(2);
        enemySpawner.StartSpawning();
        coinSpawner.StartSpawning();

        isStagePreparing = false;
    }

    private void SpawnBoss()
    {
        if (isBossActive)
            return;

        isBossActive = true;
        coinSpawner.StartSpawning();
        Instantiate(bossPrefab, bossSpawnPos.position, Quaternion.identity);
    }

    private void StageReadyAfterBoss()
    {
        isStagePreparing = false;
    }

    private void BossIsDead()
    {
        isBossActive = false;
        AddStage();

        // Her stage arası 3 saniye beklenecek
        // 1 saniye spawn süresinden geliyor
        // 2 saniey coroutine de harcanıyor
        // 0 saniye burada bekletiliyor
    }


    /* TODO
     1 - Laser Atışı yaşandığında stage atlanıcak
     2 - boss öldüğünde stage atlanıcak
     
     
     
     
     */
}