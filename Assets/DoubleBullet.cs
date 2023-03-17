using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleBullet : Bullet
{
    [SerializeField] protected float speedX;

    [SerializeField] protected Transform leftBulletPrefab;
    [SerializeField] protected BoxCollider2D leftBulletCollider;

    [SerializeField] protected Transform rightBulletPrefab;
    [SerializeField] protected BoxCollider2D rightBulletCollider;


    protected override void FixedUpdate()
    {
        LeftBulletCheckCorner();
        RightBulletCheckCorner();

        LeftBulletCheckCollisions();
        RightBulletCheckCollisions();

        if (leftBulletPrefab != null)
            leftBulletPrefab.transform.position += new Vector3(-speedX, 1f, 0f).normalized * speed * Time.deltaTime;

        if (rightBulletPrefab != null)
            rightBulletPrefab.transform.position += new Vector3(speedX, 1f, 0f).normalized * speed * Time.deltaTime;

        CheckAllBulletIfNull();
    }

    private void CheckAllBulletIfNull()
    {
        if (leftBulletPrefab == null && rightBulletPrefab == null)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void LeftBulletCheckCorner()
    {
        if (leftBulletPrefab == null)
            return;

        if (leftBulletPrefab.transform.position.y >= 9)
        {
            Destroy(leftBulletPrefab.transform.gameObject);
        }
    }

    protected virtual void RightBulletCheckCorner()
    {
        if (rightBulletPrefab == null)
            return;

        if (rightBulletPrefab.transform.position.y >= 9)
        {
            Destroy(rightBulletPrefab.transform.gameObject);
        }
    }

    protected virtual void LeftBulletCheckCollisions()
    {
        if (leftBulletPrefab == null)
            return;


        RaycastHit2D[] colliderResults1 = Physics2D.BoxCastAll(leftBulletCollider.bounds.center, leftBulletCollider.bounds.extents * 2, 0, Vector2.zero);

        foreach (var item in colliderResults1)
        {
            if (item.transform.TryGetComponent(out IDamagable damagable) && !item.transform.CompareTag("Player"))
            {
                if (damagable != null)
                {
                    damagable.TakeDamage();
                    InstantaiteBubble(item.transform.position);
                    Destroy(leftBulletPrefab.transform.gameObject);
                }
            }
        }
    }

    protected virtual void RightBulletCheckCollisions()
    {
        if (rightBulletPrefab == null)
            return;

        RaycastHit2D[] colliderResults2 = Physics2D.BoxCastAll(rightBulletCollider.bounds.center, rightBulletCollider.bounds.extents * 2, 0, Vector2.zero);

        foreach (var item in colliderResults2)
        {
            if (item.transform.TryGetComponent(out IDamagable damagable) && !item.transform.CompareTag("Player"))
            {
                if (damagable != null)
                {
                    damagable.TakeDamage();
                    InstantaiteBubble(item.transform.position);
                    Destroy(rightBulletPrefab.transform.gameObject);
                }
            }
        }
    }
}
