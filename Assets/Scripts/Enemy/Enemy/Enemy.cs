using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private bool bossEnemy;


    [FormerlySerializedAs("enemyBublePrefab")] [Header("BubbleSettings")] [SerializeField]
    private Transform enemyBubble;

    [SerializeField] private float bubbleLifeTime;
    [SerializeField] private float deathDelayOnLine;

    [Header("Particle Effect")] [SerializeField]
    private Transform enemyParticlePrefab;

    [SerializeField] private AudioClip hit;

    private bool isDead;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        CheckCorner();

        Move();
    }

    private void OnEnable()
    {
        isDead = false;
        StopBubble();
    }

    private void OnDisable()
    {
        isDead = true;
    }

    public void TakeDamage()
    {
        isDead = true;

        var duration = 0.15f;
        var magnitude = 0.25f;
        CameraScr.Instance.CameraShake(duration, magnitude);

        OnEnemyDeath?.Invoke();

        SoundManager.Instance.PlayOneShot(hit);
        
        SpawnParticle();
        
        SpawnBuble();
        StopBubble();
        
        this.gameObject.SetActive(false);
        //Destroy(gameObject);
    }


    public static event Action OnEnemyDeath;

    private void Move()
    {
        if (!isDead) transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private IEnumerator DeadSequenceCo()
    {
        isDead = true;
        animator.SetBool("isDead", true);


        var bubbleSpawnTimeCalculate = deathDelayOnLine - bubbleLifeTime;

        yield return new WaitForSeconds(bubbleSpawnTimeCalculate);
        SpawnBuble();
        yield return new WaitForSeconds(bubbleLifeTime);
        StopBubble();

        var particle = SpawnParticle();
        
        particle.SelfDestroy(2f);
        SoundManager.Instance.PlayOneShot(hit);
        
        this.gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    private void SpawnBuble()
    {
        enemyBubble.gameObject.SetActive(true);
    }
    private void StopBubble()
    {
        enemyBubble.gameObject.SetActive(false);
    }

    private ParticleScr SpawnParticle()
    {
        var particle = Instantiate(enemyParticlePrefab, transform.position, quaternion.identity);

        return particle.GetComponent<ParticleScr>();
    }

    private void CheckCorner()
    {
        if (isDead) return;

        if (transform.position.y < Player.Instance.GetPlayerPosY())
        {
            StartCoroutine(DeadSequenceCo());
            var tempPos = new Vector2(transform.position.x, Player.Instance.GetPlayerPosY());
            transform.position = tempPos;
        }

        if (transform.position.y <= -10) this.gameObject.SetActive(false);
    }
}