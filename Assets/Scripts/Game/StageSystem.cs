using System;
using System.Collections;
using PlayerNS.Bullet;
using UnityEngine;

public class StageSystem : MonoBehaviour
{
    [SerializeField] private ProjectileManager projectileManager;

    [SerializeField] private WallSystem wallSystem;
    [SerializeField] private Transform bossPrefab;
    [SerializeField] private Transform bossSpawnPos;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private DialogueManager dialogueManager;

    [SerializeField] private int stage;
    public int Stage => stage;

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

        dialogueManager = FindObjectOfType<DialogueManager>();
    }


    private void OnEnable()
    {
        PlayerManager.OnPlayerStarted += KillAllTheBulletsOnTheScene;
        PlayerManager.OnPlayerStarted += StartNextStage;
        
        ProjectileManager.OnHyperDrived += StartNextStage;
        Boss.OnBossLeave += BossIsDead;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerStarted -= KillAllTheBulletsOnTheScene;
        PlayerManager.OnPlayerStarted -= StartNextStage;

        ProjectileManager.OnHyperDrived -= StartNextStage;
        Boss.OnBossLeave -= BossIsDead;
    }

    public static event Action OnStageChanged;

    private void StartNextStage()
    {
        if (isBossActive) return;

        Debug.Log("Test");
        StartCoroutine(NextStageCo());
    }
    

    IEnumerator NextStageCo(float timeToSpawn = 1)
    {
        // Hper driveları aktif et
        // FireWallı kapat
        // ekrandaki tüm duşmanları patlat
        // Ekrana uyarı yazısı bastır
        // 1 saniye sonra yeni stage geç
            // boss stage ini daha sonra koyarsın
        stage++;

        EndStage();
        if(stage != 0)
            HyperDriveScreen.Play();

        yield return new WaitForSeconds(timeToSpawn);
        
        HyperDriveScreen.Stop();
        
        // Dialoge Handler
        dialogueManager.StartDialogue(Stage);

        if (dialogueManager.dialgoueActive)
        {
            yield return new WaitUntil(() => dialogueManager.dialgoueActive == false);
            yield return new WaitForSeconds(1f);
        }

        if (stage % 3 == 0 && stage != 0) // Boss Çağırma Kodu
            StartBossStage();
        else
            StartNormalStage();
        

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
    
    private static void KillAllTheBulletsOnTheScene()
    {
        var bullets = GameObject.FindObjectsOfType<BulletBase>();

        foreach (var bullet in bullets)
            bullet.SelfDestroy();
    }

    private void StartNormalStage()
    {
        enemySpawner.StartSpawning();
        coinSpawner.StartSpawning();
    }

    protected void EndStage()
    {
        wallSystem.ResetEverything();
        projectileManager.ChooseProjectile(0);

        enemySpawner.StopSpawning();
        coinSpawner.StopSpawning();
        
        KillAllTheEnemiesOnTheScene();
        KillAllTheCoinsOnTheScene();
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