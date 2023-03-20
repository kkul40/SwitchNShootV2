using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public static event Action OnEnemyDeath;


    [SerializeField] private float speed;
    [SerializeField] private bool bossEnemy;


    private void FixedUpdate()
    {
        CheckCorner();

        Move();
    }


    public void TakeDamage()
    {
        var duration = 0.1f;
        var magnitude = 0.2f;
        CameraScr.Instance.CameraShake(duration, magnitude);
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }

    private void Move()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void TakeDamage(float t)
    {
        Destroy(gameObject, t);
    }

    private void CheckCorner()
    {
        if (transform.position.y <= Player.Instance.GetPlayerPosY())
        {
            //Start Death Sequence
            TakeDamage(1);
            speed = 0;
        }

        if (transform.position.y <= -10) Destroy(gameObject);
    }
}