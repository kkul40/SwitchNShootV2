using System.Collections;
using System.Collections.Generic;
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
    private Vector3 direction;

    [SerializeField] private BossEye leftEye;
    [SerializeField] private BossEye rightEye;

    [SerializeField] private BossProjectiles bossProjectiles;

    [SerializeField] private float eyeOpenDuration;
    private bool isBothEyeOpen;

    private States currentState;
    

    void Start()
    {
        currentState = States.FirstApproach;
        direction = Vector3.right;

        CloseBothEyes();
        Invoke("OpenBothEyes", eyeOpenDuration);
    }

    void Update()
    {
        // surekli update ile kontrol etmek yerine eye classýndan burya sinyal gönderebilirsin
        // daha sonra halledersin
        CheckIfBothEyesIsClosed();
    }

    private void FixedUpdate()
    {
        CheckCorners();
        transform.position += direction * speed * Time.deltaTime;
        transform.position += Vector3.down * speed/2 * Time.deltaTime;
    }

    private void CheckIfBothEyesIsClosed()
    {
        if (!isBothEyeOpen)
        {
            return;
        }

        if (!leftEye.isEyeOpen && !rightEye.isEyeOpen)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + pushForceOnY, transform.position.z);

            isBothEyeOpen = false;
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
        float leftCorner = CameraScr.Instance.cameraLeftCornerX.x + offsetX;
        float rightCorner = CameraScr.Instance.cameraRightCornerX.x - offsetX;

        if (transform.position.x < leftCorner || transform.position.x > rightCorner)
        {
            if (direction == Vector3.left)
            {
                direction = Vector3.right;
            }
            else if (direction == Vector3.right)
            {
                direction = Vector3.left;
            }
        }
    }

    public void TakeDamage()
    {
        Debug.Log("boss been atacked");
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
