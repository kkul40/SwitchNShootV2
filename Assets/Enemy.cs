using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Enemy,
    BossEnemy
}

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float speed;

    public EnemyState currentState;

    private void Start()
    {
        currentState = EnemyState.Enemy;
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(float t)
    {
        Destroy(gameObject, t);
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Enemy:
                CheckCorner();
                transform.position += Vector3.down * speed * Time.deltaTime;
                break;
            case EnemyState.BossEnemy:
                CheckCorner();
                transform.position += Vector3.down * (speed/2) * Time.deltaTime;
                break;
            default:
                break;
        }
        
    }

    private void CheckCorner()
    {
        if (transform.position.y <= Player.Instance.GetPlayerPosY())
        {
            //Start Death Sequence
            TakeDamage(1);
            speed = 0;
        }

        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }

    public void SwitchState(EnemyState newState)
    {
        currentState = newState;
    }
}
