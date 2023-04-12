using System;
using PlayerNS;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour, IDamagable
{
    public static PlayerManager Instance;

    [SerializeField] private PlayerInputs playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerCollision playerCollision;
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

    private void Start()
    {
        playerInput = GetComponent<PlayerInputs>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollision = GetComponent<PlayerCollision>();
        playerSound = GetComponent<PlayerSound>();
        projectileManager = GetComponent<ProjectileManager>();
        
        ////////////////////////
    }

    private void Update()
    {
        switch (GameManager.Instance.currentStage)
        {
            case Stages.Intro:
                if (playerInput.IsSwitchPressed())
                {
                    playerAnimation.PlayerTurnOn();
                    OnPlayerStarted?.Invoke();
                }
                break;
            case Stages.Game:
                SwitchNShoot();
                break;
            case Stages.Outro:
                //TODO Do Nothing for now
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (GameManager.Instance.currentStage)
        {
            case Stages.Intro:
                // Do Nothing
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
        if (playerInput.IsSwitchPressed())
        {
            playerAnimation.PlayerTurnOn();
            playerMovement.Switch();
            playerAnimation.FlipX(playerMovement.direction);
            projectileManager.Shoot();  
        }
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