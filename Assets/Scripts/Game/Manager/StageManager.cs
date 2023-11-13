using System;
using System.Collections;
using PlayerNS.Bullet;
using UnityEngine;
using Game.Manager;
using UnityEngine.Serialization;

public class StageManager : MonoBehaviour
{
    [SerializeField] private ProjectileManager projectileManager;

    [SerializeField] private WallSystem wallSystem;
    [SerializeField] private Transform bossPrefab;
    [SerializeField] private Transform bossSpawnPos;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private DialogueManager dialogueManager;

    private int stage;

    public int Stage
    {
        get { return stage;}
        private set
        {
            stage = value; 
            EnemySpawnRateManager.instance.SetSpawnRate(stage);
        }
    }

    [SerializeField] private AnimationCurve bossLaserChanceByStage;

    [SerializeField] private ParticleSystem HyperDriveScreen;

    private bool isBossActive;
    public float GetBossLaserChance => bossLaserChanceByStage.Evaluate(Stage);


    public static StageManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Start()
    {
        Stage = -1;
    }

    private void OnEnable()
    {
        PlayerManager.OnPlayerStarted += KillAllTheBulletsOnTheScene;
        PlayerManager.OnPlayerStarted += StartNextStage;
        
        ProjectileManager.OnHyperDrived += StartNextStage;
        Boss.OnBossDeath += BossIsDead;
        
        PlayerManager.OnPlayerDeath += StopCoinSpawner;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerStarted -= KillAllTheBulletsOnTheScene;
        PlayerManager.OnPlayerStarted -= StartNextStage;

        ProjectileManager.OnHyperDrived -= StartNextStage;
        Boss.OnBossDeath -= BossIsDead;
        
        PlayerManager.OnPlayerDeath -= StopCoinSpawner;
    }

    public static event Action OnStageChanged;

    private void StartNextStage()
    {
        if (isBossActive) return;
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
        Stage++;

        EndStage();
        if(Stage != 0)
            HyperDriveScreen.Play();

        yield return new WaitForSeconds(timeToSpawn);
        
        
        // Dialogue Handler
        dialogueManager.StartDialogue(Stage);

        if (dialogueManager.dialgoueActive)
        {
            yield return new WaitUntil(() => dialogueManager.dialgoueActive == false);
            yield return new WaitForSeconds(1f);
        }
        

        if (Stage % 3 == 0 && Stage != 0) // Boss Çağırma Kodu
        {
            SoundManager.Instance.ChangeBackgroundMusicToBossFight();
            StartBossStage();
        }
        else
        {
            SoundManager.Instance.ChangeBackgroundMusicToDefault();
            StartNormalStage();
        }
        
        HyperDriveScreen.Stop();

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

    private void StopCoinSpawner()
    {
        coinSpawner.StopSpawning();
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