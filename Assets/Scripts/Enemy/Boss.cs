using System;
using UnityEngine;

public enum States
{
    FirstApproach,
    Approach,
    Dead
}


public class Boss : MonoBehaviour, IDamagable
{
    [SerializeField] private float speed;
    [SerializeField] private float offsetX;
    [SerializeField] private float pushForceOnY;

    [SerializeField] private BossEye leftEye;
    [SerializeField] private BossEye rightEye;
    [SerializeField] private float eyeOpenDuration;

    [SerializeField] private BossProjectiles bossProjectiles;

    [SerializeField] private int bossHealth;

    private States currentState;
    private Vector3 direction;

    private bool isBothEyeOpen;


    private void Start()
    {
        Debug.Log("boss doğdu");
        currentState = States.FirstApproach;
        direction = Vector3.right;

        CloseBothEyes();
        Invoke("OpenBothEyes", eyeOpenDuration);
    }

    private void Update()
    {
        // surekli update ile kontrol etmek yerine eye classından burya sinyal gönderebilirsin
        // daha sonra halledersin
        CheckIfBothEyesIsClosed();
    }

    private void FixedUpdate()
    {
        CheckCorners();
        transform.position += direction * speed * Time.deltaTime;
        transform.position += Vector3.down * speed / 2 * Time.deltaTime;
    }

    public void TakeDamage()
    {
        Debug.Log("boss been atacked");
        CalculteHealt();
    }

    public static event Action OnBossDeath;

    private void CheckIfBothEyesIsClosed()
    {
        if (!isBothEyeOpen) return;

        if (!leftEye.isEyeOpen && !rightEye.isEyeOpen)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + pushForceOnY,
                transform.position.z);

            isBothEyeOpen = false;
            CalculteHealt();
            Invoke("OpenBothEyes", eyeOpenDuration);
        }
    }

    private void OpenBothEyes()
    {
        leftEye.SetEyeOpen();
        rightEye.SetEyeOpen();

        isBothEyeOpen = true;
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
            Destroy(gameObject);
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