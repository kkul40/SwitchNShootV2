using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    [SerializeField] private Vector3 laserStartPosOffset;


    protected override void FixedUpdate()
    {
        Vector3 firePoint = Player.Instance.projectileSpawnPoint.position;

        firePoint = firePoint + laserStartPosOffset;

        transform.position = firePoint;

        CheckCollisions();
    }

    protected override void CheckCollisions()
    {
        RaycastHit2D[] colliderResults = Physics2D.BoxCastAll(_boxCollider.bounds.center, _boxCollider.bounds.extents * 2, 0, Vector2.zero);

        foreach (var item in colliderResults)
        {
            if (item.transform.TryGetComponent(out IDamagable damagable) && !item.transform.CompareTag("Player"))
            {
                if (damagable != null)
                {
                    damagable.TakeDamage();
                }
            }
        }
    }
}
