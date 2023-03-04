using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Bullet
{
    [SerializeField] private Vector3 laserStartPosOffset;


    private void Update()
    {
        Vector3 firePoint = Player.Instance.GetFirePointPos();
        firePoint = firePoint + laserStartPosOffset;
        transform.position = firePoint;

        CheckCollisions();
    }

    protected override void FixedUpdate()
    {
        // DoNothing
    }

    public void DestroyLaser()
    {
        Destroy(gameObject);
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
