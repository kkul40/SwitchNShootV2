using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float speed;

    public void TakeDamage()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        CheckCorner();
        transform.position += Vector3.down * speed * Time.deltaTime;
    }
    private void CheckCorner()
    {
        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }
}
