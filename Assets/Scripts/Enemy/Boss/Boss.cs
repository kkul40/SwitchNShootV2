using System;
using UnityEngine;
using UnityEngine.Serialization;

public enum BossStates
{
    FirstApproach,
    Approach,
    Dead
}

[SelectionBase]
public class Boss : MonoBehaviour, IDamagable
{
    [SerializeField] private float speed;
    [SerializeField] private float offsetX; // For Corners
    [SerializeField] private float pushForceOnY;
    [SerializeField] private Vector3 approachPos;

    [SerializeField] private BossEye leftBossEye;
    [SerializeField] private BossEye rightBossEye;
    [SerializeField] private BossProjectiles bossProjectiles;
    [SerializeField] private float eyeOpenDuration;

    [SerializeField] private CoinSpawner coinSpawner;
    [SerializeField] private ParticleScr bossParticleSystem;

    private BossStates currentBossState;
    private Vector3 direction;

    private bool isBothEyeOpen;
    private bool isLeftEyeOpen;
    private bool isRightEyeOpen;
    private Vector3 lastDirection;

    private void OnEnable()
    {
        ProjectileManager.OnLaserFired += DeathSequence;
    }

    private void OnDisable()
    {
        ProjectileManager.OnLaserFired -= DeathSequence;
    }
    
    private void Start()
    {
        currentBossState = BossStates.FirstApproach;
        direction = Vector3.right;
        lastDirection = direction;

        coinSpawner = FindObjectOfType<CoinSpawner>();

        CloseBothEyes();
    }

    private void FixedUpdate()
    {
        switch (currentBossState)
        {
            case BossStates.FirstApproach:
                transform.position = Vector3.Lerp(transform.position, approachPos, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, approachPos) < .1f)
                {
                    Invoke(nameof(OpenBothEyes), eyeOpenDuration);
                    currentBossState = BossStates.Approach;
                }

                break;
            case BossStates.Approach:
                CheckCorners();

                if (bossProjectiles.isAttacking)
                {
                    //TODO buraya birseyler ekle
                    //transform.position = Vector3.Lerp(transform.position, transform.position + direction * 0.2f, .4f);
                }
                else
                {
                    transform.position += direction * (speed * Time.deltaTime);
                    transform.position += Vector3.down * (speed / 2 * Time.deltaTime);
                }

                break;
            case BossStates.Dead:
                transform.position += lastDirection * (speed * 5 * Time.deltaTime);
                transform.position += Vector3.up * (speed * Time.deltaTime);
                break;
        }
    }

    public void TakeDamage()
    {
        // Do Nothing
    }

    public static event Action OnBossDeath;
    public static event Action OnBossLeave;


    private bool CheckIfBothEyesIsClosed(BossEye.WhichEye whichEye)
    {
        if (!isBothEyeOpen) return false;

        if (!leftBossEye.isEyeOpen && !rightBossEye.isEyeOpen)
        {
            switch (whichEye)
            {
                case BossEye.WhichEye.LeftEye:
                    SpawnCoin(leftBossEye.transform.position);
                    break;
                case BossEye.WhichEye.RightEye:
                    SpawnCoin(rightBossEye.transform.position);
                    break;
            }
            
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + pushForceOnY,
                transform.position.z);

            isBothEyeOpen = false;
            Invoke(nameof(OpenBothEyes), eyeOpenDuration);
        }

        return true;
    }

    private void SpawnCoin(Vector3 pos)
    {
        coinSpawner.Spawn(pos);
    }

    private void OpenBothEyes()
    {
        leftBossEye.SetEyeOpen();
        rightBossEye.SetEyeOpen();

        isBothEyeOpen = true;
    }

    public void IsLeftEyeOpen(bool leftEye)
    {
        isLeftEyeOpen = leftEye;
        CheckIfBothEyesIsClosed(BossEye.WhichEye.LeftEye);
    }

    public void IsRightEyeOpen(bool rightEye)
    {
        isRightEyeOpen = rightEye;
        CheckIfBothEyesIsClosed(BossEye.WhichEye.RightEye);
    }

    private void CloseBothEyes()
    {
        leftBossEye.SetEyeClose();
        rightBossEye.SetEyeClose();

        isBothEyeOpen = false;
    }

    private void CheckCorners()
    {
        var leftCorner = CameraScr.Instance.cameraLeftCornerX.x + offsetX;
        var rightCorner = CameraScr.Instance.cameraRightCornerX.x - offsetX;

        if (transform.position.x < leftCorner || transform.position.x > rightCorner)
        {
            if (direction == Vector3.left)
                direction = Vector3.right;
            else if (direction == Vector3.right) direction = Vector3.left;
        }
    }

    private void DeathSequence()
    {
        bossParticleSystem.PlayParticleSystem();
        lastDirection = direction;
        currentBossState = BossStates.Dead;
        OnBossLeave?.Invoke();
        Invoke(nameof(SelfDestroy), 3f);
    }

    private void SelfDestroy()
    {
        OnBossDeath?.Invoke();
        Destroy(gameObject);
    }


    //Attack Pattern
    /*
     * 1
     * +      +
     *   +  +
     *    +
     * 
     * 2
     * +
     *   +
     *     +
     * 
     * 3
     *     + 
     *   +
     * +
     * 
     * 4
     * +
     *  +
     *   +
     *    +
     *     +
     *     
     *5
     *     +
     *    +
     *   +
     *  +
     * +
     * 
     * 6 
     * LASER
     */
}