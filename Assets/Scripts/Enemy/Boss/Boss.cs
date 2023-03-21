using System;
using Unity.Mathematics;
using UnityEngine;

public enum States
{
    FirstApproach,
    Approach,
    Dead
}


public class Boss : MonoBehaviour, IDamagable
{
    public static event Action OnBossDeath;

    [SerializeField] private float speed;
    [SerializeField] private float offsetX; // For Corners
    [SerializeField] private float pushForceOnY;
    [SerializeField] private Vector3 approachPos;

    [SerializeField] private BossEye leftEye;
    [SerializeField] private BossEye rightEye;
    [SerializeField] private BossProjectiles bossProjectiles;
    private bool isLeftEyeOpen;
    private bool isRightEyeOpen;
    [SerializeField] private float eyeOpenDuration;


    [SerializeField] private int bossHealth;

    [SerializeField] private ParticleScr bossParticleSystem;

    private States currentState;
    private Vector3 direction;
    private Vector3 lastDirection;

    private bool isBothEyeOpen;


    private void Start()
    {
        currentState = States.FirstApproach;
        direction = Vector3.right;
        lastDirection = direction;
        
        CloseBothEyes();
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case States.FirstApproach:
                transform.position = Vector3.Lerp(transform.position, approachPos, speed  * Time.deltaTime);

                if (Vector3.Distance(transform.position, approachPos) < .1f)
                {
                    Invoke(nameof(OpenBothEyes), eyeOpenDuration);
                    currentState = States.Approach;
                }
                break;
            case States.Approach:
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
            case States.Dead:
                transform.position += lastDirection * (speed * 3 * Time.deltaTime);
                transform.position += Vector3.up * (speed / 2 * Time.deltaTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        
    }
    
    public void TakeDamage()
    {
        Debug.Log("boss been atacked");
        CalculteHealt();
    }


    private void CheckIfBothEyesIsClosed()
    {
        if (!isBothEyeOpen) return;

        if (!leftEye.isEyeOpen && !rightEye.isEyeOpen)
        {
            transform.position = new Vector3(
                transform.position.x, 
                transform.position.y + pushForceOnY,
                transform.position.z);
            
            isBothEyeOpen = false;
            CalculteHealt();
            Invoke(nameof(OpenBothEyes), eyeOpenDuration);
        }
    }

    private void OpenBothEyes()
    {
        leftEye.SetEyeOpen();
        rightEye.SetEyeOpen();

        isBothEyeOpen = true;
    }

    public void IsLeftEyeOpen(bool leftEye)
    {
        isLeftEyeOpen = leftEye;
        CheckIfBothEyesIsClosed();
    }
    
    public void IsRightEyeOpen(bool rightEye)
    {
        isRightEyeOpen = rightEye;
        CheckIfBothEyesIsClosed();
    }

    private void CloseBothEyes()
    {
        leftEye.SetEyeClose();
        rightEye.SetEyeClose();

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

    private void CalculteHealt()
    {
        bossHealth--;
        if (bossHealth <= 0)
        {
            OnBossDeath?.Invoke();
            bossParticleSystem.PlayParticleSystem();
            lastDirection = direction;
            currentState = States.Dead;
            Destroy(gameObject, 5f);
        }
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