using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public static event Action OnEnemyDeath;

    [SerializeField] private Animator animator;


    [SerializeField] private float speed;
    [SerializeField] private bool bossEnemy;

    private bool isDead;

    [SerializeField] private AudioClip hit;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        CheckCorner();

        Move();
    }


    public void TakeDamage()
    {
        isDead = true;
        
        var duration = 0.1f;
        var magnitude = 0.2f;
        CameraScr.Instance.CameraShake(duration, magnitude);
        OnEnemyDeath?.Invoke();
        SoundManager.Instance.PlayOneShot(hit);
        Destroy(gameObject);
    }

    private void Move()
    {
        if(!isDead) transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void TakeDamage(float t)
    {
        isDead = true;
        animator.SetBool("isDead", true);
        Destroy(gameObject, t);
    }

    private void CheckCorner()
    {
        if (isDead) return;
        
        if (transform.position.y < Player.Instance.GetPlayerPosY())
        {
            // TODO Start Death Sequence
            TakeDamage(1);
            Vector2 tempPos = new Vector2(transform.position.x, Player.Instance.GetPlayerPosY());
            transform.position = tempPos;
        }

        if (transform.position.y <= -10) Destroy(gameObject);
    }
}