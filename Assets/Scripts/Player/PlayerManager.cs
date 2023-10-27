using System;
using System.Collections;
using PlayerNS;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour, IDamagable
{
    public static PlayerManager Instance;

    [SerializeField] private PlayerInputs playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCollision playerCollision;
    [SerializeField] private PlayerShield playerShield;
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private PlayerSound playerSound;
    [SerializeField] public ProjectileManager projectileManager;

    //**********************

    [Header("Particle Effect")] [SerializeField]
    private Transform enemyParticlePrefab;

    private bool isGameStarted;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        Shield.OnShieldCollected += ActivateShield;
    }

    private void ActivateShield()
    {
        playerShield.HasShield = true;
        playerAnimation.ToggleShieldAnimation(true);
    }

    private void Start()
    {
        playerInput = GetComponent<PlayerInputs>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollision = GetComponent<PlayerCollision>();
        playerShield = GetComponent<PlayerShield>();
        playerSound = GetComponent<PlayerSound>();
        projectileManager = GetComponentInChildren<ProjectileManager>();

        ////////////////////////
    }

    private void Update()
    {
        switch (GameManager.Instance.currentStage)
        {
            case Stages.Intro:
                if (playerInput.IsSwitchPressed())
                {
                    playerMovement.SetPlayerStartPos();
                    playerAnimation.PlayerTurnOn();
                    OnPlayerStarted?.Invoke();
                }
                break;
            case Stages.Game:
                if (playerInput.IsSwitchPressed()) SwitchNShoot();
                break;
            case Stages.Outro:
                //TODO Do Nothing for now
                break;
        }
    }
    // Variables For PlayerIntro Movement
    private float time = 3;
    private float timer = 0;
    private bool wait = true;

    private void FixedUpdate()
    {
        switch (GameManager.Instance.currentStage)
        {
            case Stages.Intro:
                timer += Time.deltaTime;

                if (timer > time)
                {
                    SwitchNShoot();
                    timer = 0;
                    time = Random.Range(0f, 1f);
                    wait = false;
                }
                
                if(!wait)
                    playerMovement.MoveHorizontal();
                
                break;
            case Stages.Game:
                playerCollision.CheckCollisions();
                playerMovement.MoveHorizontal();
                break;
            case Stages.Outro:
                playerMovement.MoveVertical();
                break;
        }
    }

    public void TakeDamage()
    {
        if (playerShield.HasShield)
        {
            playerShield.HasShield = false;
            playerAnimation.ToggleShieldAnimation(false);
            return;
        }
        
        playerAnimation.PlayerTurnOff();
        var particle = Instantiate(enemyParticlePrefab, transform.position, Quaternion.identity);
        particle.GetComponent<ParticleScr>().SelfDestroy(3f);
        playerSound.PlaySound();
        OnPlayerDeath?.Invoke();
    }

    public static event Action OnPlayerStarted;
    public static event Action OnPlayerDeath;

    private void SwitchNShoot()
    {
        playerAnimation.PlayerTurnOn();
        playerMovement.Switch();
        playerAnimation.FlipX(playerMovement.direction);
        projectileManager.Shoot();
    }

    public Vector3 GetFirePointPos()
    {
        return projectileManager.projectileSpawnPoint.position;
    }

    public float GetPlayerPosY()
    {
        return transform.position.y;
    }
}