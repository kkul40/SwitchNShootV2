using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class StageSystem : MonoBehaviour
{
    [FormerlySerializedAs("projectiles")] [SerializeField]
    private ProjectileManager projectileManager;

    [SerializeField] private Transform bossPrefab;
    [SerializeField] private Transform bossSpawnPos;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CoinSpawner coinSpawner;

    [SerializeField] private int stage = 1;

    [SerializeField] private AnimationCurve enemySpawnByStage;

    [SerializeField] private AnimationCurve bossLaserChanceByStage;

    private bool isBossActive;
    public int GetStage => stage;

    public int GetLaserFireCount => projectileManager.GetLaserFiredCount;

    public float GetEnemySpawnRate => enemySpawnByStage.Evaluate(stage);
    public float GetBossLaserChance => bossLaserChanceByStage.Evaluate(stage);


    private void OnEnable()
    {
        ProjectileManager.OnLaserStopped += AddStage;
        Boss.OnBossLeave += BossIsDead;
    }

    private void OnDisable()
    {
        ProjectileManager.OnLaserStopped -= AddStage;
        Boss.OnBossLeave -= BossIsDead;
    }

    public static event Action OnStageChanged;

    private void AddStage()
    {
        //if (projectiles.GetLaserFiredCount % 2 != 0) return;
        if (isBossActive) return;

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

        OnStageChanged?.Invoke();
    }

    private IEnumerator StopSpawningForAWhileCo()
    {
        enemySpawner.StopSpawning();
        coinSpawner.StopSpawning();
        // 3 saniye stage molası 2 saaniye burda 1 saniye spawn fonksiyonu içinde
        yield return new WaitForSeconds(3);
        enemySpawner.StartSpawning();
        coinSpawner.StartSpawning();
    }

    private void SpawnBoss()
    {
        if (isBossActive)
            return;

        isBossActive = true;
        coinSpawner.StartSpawning();
        Instantiate(bossPrefab, bossSpawnPos.position, Quaternion.identity);
    }


    private void BossIsDead()
    {
         isBossActive = false;
        // if (stage < 1)
        // {
        //     projectileManager.SetProjectileIndex(0);
        // }
        // else
        // {
        //     projectileManager.SetProjectileIndex(stage - 1);
        // }
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