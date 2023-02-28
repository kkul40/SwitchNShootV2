using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed;

    protected BoxCollider2D _boxCollider;

    protected void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void FixedUpdate()
    {
        CheckCorner();
        CheckCollisions();
        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    protected virtual void CheckCorner()
    {
        if (transform.position.y >= 9)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void CheckCollisions()
    {
        RaycastHit2D[] colliderResults = Physics2D.BoxCastAll(_boxCollider.bounds.center, _boxCollider.bounds.extents * 2, 0, Vector2.zero);

        foreach (var item in colliderResults)
        {
            if (item.transform.TryGetComponent(out IDamagable damagable) && !item.transform.CompareTag("Player"))
            {
                if (damagable != null)
                {
                    damagable.TakeDamage();
                    Destroy(gameObject);
                }
            }
        }
    }
}
