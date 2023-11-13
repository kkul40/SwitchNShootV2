using System;
using PlayerNS.Bullet;
using PlayerNS.Components;
using Unity.VisualScripting;
using UnityEngine;

public class LaserManager : MonoBehaviour
{
    [SerializeField] private Vector3 laserStartPosOffset;
    protected BoxCollider2D _boxCollider;
    private LaserBubble bubbleCreater;

    private SoundPlayer soundPlayer;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        soundPlayer = GetComponent<SoundPlayer>();
        bubbleCreater = GetComponent<LaserBubble>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(PlaySoundRepeatedly), 0, 0.1f);
    }

    private void Update()
    {
        var firePoint = PlayerManager.Instance.GetFirePointPos();
        firePoint = firePoint + laserStartPosOffset;
        transform.position = firePoint;

        CheckCollisions();
    }

    public void DestroyLaser()
    {
        CancelInvoke(nameof(PlaySoundRepeatedly));
        Destroy(gameObject);
    }

    private void PlaySoundRepeatedly()
    {
        soundPlayer.PlaySound();
    }

    private void CheckCollisions()
    {
        var colliderResults =
            Physics2D.BoxCastAll(_boxCollider.bounds.center, _boxCollider.bounds.extents * 2, 0, Vector2.zero);

        foreach (var col in colliderResults)
            if (col.transform.CompareTag("Player"))
            {
                // DO Nothing
            }
            else if (col.transform.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage();
                // Create Bubble
                bubbleCreater.StartBubble(col.point);
            }
    }
}