using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class StageSystem : MonoBehaviour
{
    
    
    [FormerlySerializedAs("projectiles")] [SerializeField]
    private ProjectileManager projectileManager;

    [SerializeField] private WallSystem wallSystem;
    [SerializeField] private Transform bossPrefab;
    [SerializeField] private Transform bossSpawnPos;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CoinSpawner coinSpawner;

    [SerializeField] private int stage = 1;
    public int Stage => stage;

    private float enemySpawnTimer = 0;

    [SerializeField] private AnimationCurve enemySpawnByStage;

    [SerializeField] private AnimationCurve bossLaserChanceByStage;

    [SerializeField] private ParticleSystem HyperDriveScreen;

    private bool isBossActive;
    public int GetStage => stage;

    public float GetEnemySpawnRate => enemySpawnByStage.Evaluate(enemySpawnTimer);
    public float GetBossLaserChance => bossLaserChanceByStage.Evaluate(stage);


    public static StageSystem Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }


    private void OnEnable()
    {
        ProjectileManager.OnHyperDrived += StartNextStage;
        Boss.OnBossLeave += BossIsDead;
    }

    private void OnDisable()
    {
        ProjectileManager.OnHyperDrived -= StartNextStage;
        Boss.OnBossLeave -= BossIsDead;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.currentStage == Stages.Game)
        {
            enemySpawnTimer += Time.deltaTime * stage/30;
            Debug.Log(enemySpawnTimer);
        }
    }

    public static event Action OnStageChanged;

    private void StartNextStage()
    {
        StartCoroutine(NextStageCo());
    }

    IEnumerator NextStageCo()
    {
        // Hper driveları aktif et
        // FireWallı kapat
        // ekrandaki tüm duşmanları patlat
        // Ekrana uyarı yazısı bastır
        // 1 saniye sonra yeni stage geç
            // boss stage ini daha sonra koyarsın

        wallSystem.StopFireWalls();
        projectileManager.ChooseProjectile(0);
        
        enemySpawner.StopSpawning();
        coinSpawner.StopSpawning();
        
        stage++;
        
        HyperDriveScreen.Play();
        var enemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (var enemy in enemies)
            enemy.TakeDamage();

        yield return new WaitForSeconds(1);
        
        HyperDriveScreen.Stop();

        enemySpawnTimer = 0;
        enemySpawner.StartSpawning();
        coinSpawner.StartSpawning();
        
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
        NextStageCo();

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