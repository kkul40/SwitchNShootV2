using System;
using Unity.Mathematics;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private Transform powerUpPrefab;
    public Rigidbody2D rb2d;
    
    [Header("Settings")]
    public float moveSpeed;

    [SerializeField] private Vector2 spawnTextPos; // CointCollectedText Position
    [SerializeField] private Vector2 spawnTextPos2; // CoinMissedText Position

    [Header("Sound")]
    [SerializeField] private AudioClip CoinCollected;
    [SerializeField] private AudioClip coinMissed;

    private bool isDestroyed;

    public static event Action OnCoinCollected;
    public static event Action OnCoinMissed;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0;
    }

    private void FixedUpdate()
    {
        CheckCorner();
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

    public void Collect()
    {
        OnCoinCollected?.Invoke();
        SpawnText("POWER-UP", false);
        SoundManager.Instance.PlayOneShot(CoinCollected);
        Destroy(gameObject);
    }

    public void SelfDestroy()
    {
        // SpawnText("REMOVED");
        Destroy(gameObject);
    }

    private void CheckCorner()
    {
        if (isDestroyed) return;

        if (transform.position.y <= -7.5)
        {
            isDestroyed = true;
            OnCoinMissed?.Invoke();
            SpawnText("MISSED", true);
            SoundManager.Instance.PlayOneShot(coinMissed);
            Destroy(gameObject, 2);
        }
    }

    public void SetGravity(float gravityScale)
    {
        rb2d.gravityScale = gravityScale;
    }

    private void SpawnText(string message, bool isMissed)
    {
        Vector3 spawnPos = transform.position;
        if (isMissed)
            spawnPos = spawnTextPos2;
        else
            spawnPos = spawnTextPos;

        spawnPos.x = transform.position.x;
        
        var text = Instantiate(powerUpPrefab, spawnPos, quaternion.identity);
        text.GetComponent<PopupText>().Instantiate(message, Color.yellow);
        Destroy(text.gameObject, 0.5f);
    }
}