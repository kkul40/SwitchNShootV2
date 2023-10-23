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

    [SerializeField] private int stage;
    public int Stage => stage;

    private float enemySpawnTimer = 0;

    [SerializeField] private AnimationCurve enemySpawnByStage;

    [SerializeField] private AnimationCurve bossLaserChanceByStage;

    [SerializeField] private ParticleSystem HyperDriveScreen;

    private bool isBossActive;
    public int GetStage => stage;

    public float GetEnemySpawnRate => enemySpawnByStage.Evaluate(stage);
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
        if (isBossActive) return;

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

        EndStage();

        yield return new WaitForSeconds(1);

        if (stage % 3 == 0) // Boss Çağırma Kodu
            StartBossStage();
        else
        {
            StartNormalStage();
            stage++;    
        }

        OnStageChanged?.Invoke();
    }

    private void KillAllTheCoinsOnTheScene()
    {
        var coins = GameObject.FindObjectsOfType<Coin>();

        foreach (var coin in coins)
            coin.SelfDestroy();
    }

    private static void KillAllTheEnemiesOnTheScene()
    {
        var enemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (var enemy in enemies)
            enemy.TakeDamage();
    }

    private void StartNormalStage()
    {
        enemySpawnTimer = 0;
        enemySpawner.StartSpawning();
        coinSpawner.StartSpawning();
        
        HyperDriveScreen.Stop();
    }

    protected void EndStage()
    {
        wallSystem.ResetEverything();
        projectileManager.ChooseProjectile(0);

        enemySpawner.StopSpawning();
        coinSpawner.StopSpawning();
        
        KillAllTheEnemiesOnTheScene();
        KillAllTheCoinsOnTheScene();
        
        HyperDriveScreen.Play();
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


    private void StartBossStage()
    {
        if (isBossActive)
            return;

        isBossActive = true;
        coinSpawner.StartSpawning();
        Instantiate(bossPrefab, bossSpawnPos.position, Quaternion.identity);
        
        HyperDriveScreen.Stop();
    }


    private void BossIsDead()
    {
         isBossActive = false;
        
        StartNextStage();
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