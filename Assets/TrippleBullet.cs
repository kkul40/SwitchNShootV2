using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrippleBullet : DoubleBullet
{
    [SerializeField] private Transform middleBulletPrefab;
    [SerializeField] private BoxCollider2D middleBulletCollider;

    protected override void FixedUpdate()
    {
        //Base Class Controlls
        LeftBulletCheckCorner();
        RightBulletCheckCorner();

        LeftBulletCheckCollisions();
        RightBulletCheckCollisions();

        if (leftBulletPrefab != null)
            leftBulletPrefab.transform.position += new Vector3(-speedX, 1f, 0f).normalized * speed * Time.deltaTime;

        if (rightBulletPrefab != null)
            rightBulletPrefab.transform.position += new Vector3(speedX, 1f, 0f).normalized * speed * Time.deltaTime;

        //Current Class Controlls
        MiddleBulletCheckCorner();
        MiddleBulletCheckCollisions();

        if (middleBulletPrefab != null)
            middleBulletPrefab.transform.position += Vector3.up * speed * Time.deltaTime;

        CheckAllBulletIfNull();
    }

    private void CheckAllBulletIfNull()
    {
        if (leftBulletPrefab == null && middleBulletPrefab == null && rightBulletPrefab == null)
        {
            Destroy(gameObject);
        }
    }

    private void MiddleBulletCheckCorner()
    {
        if (middleBulletPrefab == null)
            return;

        if (middleBulletPrefab.transform.position.y >= 9)
        {
            Destroy(middleBulletPrefab.transform.gameObject);
        }
    }

    private void MiddleBulletCheckCollisions()
    {
        if (middleBulletPrefab == null)
            return;


        RaycastHit2D[] colliderResults1 = Physics2D.BoxCastAll(middleBulletCollider.bounds.center, middleBulletCollider.bounds.extents * 2, 0, Vector2.zero);

        foreach (var item in colliderResults1)
        {
            if (item.transform.TryGetComponent(out IDamagable damagable) && !item.transform.CompareTag("Player"))
            {
                if (damagable != null)
                {
                    damagable.TakeDamage();
                    Destroy(middleBulletCollider.transform.gameObject);
                }
            }
        }
    }
}
