using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    public static Player Instance;

    private Inputs inputs;
    
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private Transform projectileSpawnPoint;
    
    [Header("Particle Effect")] 
    [SerializeField] private Transform enemyParticlePrefab;


    [SerializeField] private Vector2 playerStartPos;
    [SerializeField] private float speed;
    private Vector3 _direction;

    private float cornerOffsetX;

    private bool isGameStarted;
    private Projectiles projectiles;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        inputs = new Inputs();
    }

    private void Start()
    {
        _direction = Vector2.right;
        _boxCollider = GetComponent<BoxCollider2D>();
        projectiles = GetComponent<Projectiles>();
        
        cornerOffsetX = _boxCollider.bounds.extents.x;

        transform.position = playerStartPos;
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void Update()
    {
        switch (GameManager.Instance.currentStage)
        {
            case Stages.Intro:
                if (inputs.Player.Switch.WasPressedThisFrame())
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
                CheckCorner();
                CheckCollisions();
                transform.position += _direction * speed * Time.deltaTime;
                break;
            case Stages.Outro:
                transform.position += Vector3.down * speed / 2 * Time.deltaTime;
                break;
        }
    }

    public void TakeDamage()
    {
        playerAnimation.PlayerTurnOff();
        var particle = Instantiate(enemyParticlePrefab, transform.position, Quaternion.identity);
        particle.GetComponent<ParticleScr>().SelfDestroy(3f);
        OnPlayerDeath?.Invoke();
    }

    public static event Action OnPlayerStarted;
    public static event Action OnPlayerDeath;


    private void SwitchNShoot()
    {
        if (inputs.Player.Switch.WasPressedThisFrame())
        {
            playerAnimation.PlayerTurnOn();
            Switch();
            Shoot();
        }
    }

    public static event Action OnShoot;

    private void Switch()
    {
        if (_direction == Vector3.right)
        {
            _direction = Vector3.left;
            playerAnimation.FlipLeft();
        }
        else if (_direction == Vector3.left)
        {
            _direction = Vector3.right;
            playerAnimation.FlipRight();
        }
    }


    private void Shoot()
    {
        var projectile = projectiles.choosenProjectile;

        if (projectile == null)
            return;

        Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
        OnShoot?.Invoke();
    }

    private void ShootLaser()
    {
    }

    private void CheckCorner()
    {
        // TODO Daha sonra left right border yerine sadece 1 de�i�ken kullanacak �ekilde de�i�tir
        if (transform.position.x + cornerOffsetX <= CameraScr.Instance.cameraLeftCornerX.x)
            transform.position = new Vector3(CameraScr.Instance.cameraRightCornerX.x + cornerOffsetX,
                transform.position.y, 0f);
        else if (transform.position.x - cornerOffsetX >= CameraScr.Instance.cameraRightCornerX.x)
            transform.position = new Vector3(CameraScr.Instance.cameraLeftCornerX.x - cornerOffsetX,
                transform.position.y, 0f);
    }

    private void CheckCollisions()
    {
        var colliderResults =
            Physics2D.BoxCastAll(_boxCollider.bounds.center, _boxCollider.bounds.extents * 2, 0, Vector2.zero);

        foreach (var item in colliderResults)
            if (item.transform.TryGetComponent(out IDamagable damagable) && !item.transform.CompareTag("Player"))
            {
                damagable.TakeDamage();
                TakeDamage();
            }
            else if (item.transform.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect();
                projectiles.LevelUp();
            }
            else if (item.transform.TryGetComponent(out EnemyBubble enemyBubble))
            {
                TakeDamage();
            }
    }

    public Vector3 GetFirePointPos()
    {
        return projectileSpawnPoint.position;
    }

    public float GetPlayerPosY()
    {
        return transform.position.y;
    }


    /*public static PlayerScr Instance;

    public static event Action onPlayerDeath;

    [SerializeField] private CharacterInput characterInput;

    public static bool isPlayerDeath { get; private set; }

    private PowerUpSystem _powerUpSystem;
    private SpriteRenderer _sprite;
    private Animator _anim;
    private CircleCollider2D _collider;
    [SerializeField] private GameObject fireSprite;
    [SerializeField] private GameObject deadParticuls;

    public AudioClip hit;

    [SerializeField] private float speed;
    [SerializeField] private float fallSpeed;
    private float holdTime = 0;
    private float passedTime = 0;
    private Vector3 _direction;
    [SerializeField] private Vector2 startPos;
    public Vector2 PlayerStartPos => startPos;
    [SerializeField] private float cornerX;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask hitLayers;
    public Transform FirePoint => firePoint;

    public PlayerStates state;

    private bool isPlayingIntro = true;
    private bool canMoveInIntro = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void OnEnable()
    {
        onPlayerDeath += PlayerIsDeath;
    }

    private void PlayerIsDeath()
    {
        isPlayerDeath = true;
    }

    void Start()
    {
        isPlayerDeath = false;

        _direction = Vector3.right;
        transform.position = startPos;

        characterInput = GetComponent<CharacterInput>();
        _powerUpSystem = GetComponent<PowerUpSystem>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();

        fireSprite.SetActive(false);
        state = PlayerStates.PlayIntro;

        _anim = GetComponentInChildren<Animator>();

        RandomHoldTime();
        StartCoroutine(StartIntro());

    }

    private void Update()
    {
        switch (state)
        {
            case PlayerStates.PlayIntro:
                if (canMoveInIntro)
                {
                    passedTime += Time.deltaTime;

                    if (passedTime >= holdTime)
                    {
                        Switch();
                        Shoot();
                        passedTime = 0;
                        RandomHoldTime();
                    }
                    IntroCheckCornersNMove();
                }

                // e�er oyun start ekran�ndaysa ko�ulunu ekle
                if (characterInput.GetFireButton())
                {
                    GameManager.Instance.SwitchGMState(new GMStartScreen());
                    StopFlyAnimation();
                    transform.position = startPos;
                    _direction = Vector2.right;
                    isPlayingIntro = false;
                    state = PlayerStates.PressToStart;
                }
                break;
            case PlayerStates.PressToStart:
                if (characterInput.GetFireButton())
                {
                    GameManager.Instance.SwitchGMState(new GMGameScreen());
                    StartFlyAnimation();
                    state = PlayerStates.Play;
                }

                break;
            case PlayerStates.Play:
                if (characterInput.GetFireButton())
                {
                    Switch();
                    Shoot();
                }

                CheckCornersNMove();
                CheckCollisions();

                break;
            case PlayerStates.Dead:
                transform.position += Vector3.down * fallSpeed * Time.deltaTime;

                break;
            case PlayerStates.DoNothing:
                break;
            default:
                break;
        }

    }


    private void RandomHoldTime()
    {
        holdTime = UnityEngine.Random.Range(0.2f, 1f);
    }

    private void Switch()
    {
        if (_direction == Vector3.right)
        {
            _sprite.flipX = true;
            _direction = Vector3.left;
        }
        else if (_direction == Vector3.left)
        {
            _sprite.flipX = false;
            _direction = Vector3.right;
        }
    }

    private void Shoot()
    {
        if (!_powerUpSystem.IsLaserFired)
            StartCoroutine(SetFireSprite());

        _powerUpSystem.Shoot(firePoint);
    }

    private void CheckCornersNMove()
    {
        Vector3 nextMovePos = transform.position + _direction * speed * Time.deltaTime;

        if (nextMovePos.x >= cornerX)
        {
            transform.position = new Vector2(-cornerX + 0.25f, startPos.y);
        }
        else if (nextMovePos.x <= -cornerX)
        {
            transform.position = new Vector2(cornerX - 0.25f, startPos.y);
        }

        transform.position += _direction * speed * Time.deltaTime;
    }

    private void IntroCheckCornersNMove()
    {
        Vector3 nextMovePos = transform.position + _direction * speed * Time.deltaTime;

        if (nextMovePos.x < -cornerX + 2 || nextMovePos.x > cornerX - 2)
        {
            Switch();
            Shoot();
            RandomHoldTime();
        }

        transform.position += _direction * speed * Time.deltaTime;
    }

    public void TakeDamage()
    {
        if (state == PlayerStates.Dead)
            return;

        onPlayerDeath?.Invoke();
        DeadSequence();
    }

    public void DeadSequence()
    {
        _collider.enabled = false;
        Instantiate(deadParticuls, transform.position, Quaternion.identity);
        SoundManager.Instance.PlayThisSound(hit);
        state = PlayerStates.Dead;
        GameManager.Instance.SwitchGMState(new GMDeadScreen());
        onPlayerDeath?.Invoke();
    }

    IEnumerator SetFireSprite()
    {
        fireSprite.SetActive(true);
        yield return new WaitForSeconds(0.033f);
        fireSprite.SetActive(false);
    }

    IEnumerator StartIntro()
    {
        yield return new WaitForSeconds(1f);

        if (isPlayingIntro)
        {
            canMoveInIntro = true;
            isPlayingIntro = true;
            StartFlyAnimation();
        }

    }

    public void StartFlyAnimation()
    {
        _anim.SetBool("fly", true);
    }
    public void StopFlyAnimation()
    {
        _anim.SetBool("fly", false);
    }


    private void CheckCollisions()
    {
        RaycastHit2D[] colliderResults = Physics2D.BoxCastAll(_collider.bounds.center, _collider.bounds.extents * 2, 0, Vector2.zero);

        foreach (var item in colliderResults)
        {
            if (item.transform.TryGetComponent(out IDamagable damagable) && !item.transform.CompareTag("Player"))
            {
                if (damagable != null)
                {
                    Debug.Log("player �arpt�");
                    damagable.TakeDamage();
                    TakeDamage();
                }
            }
        }
    }*/
}