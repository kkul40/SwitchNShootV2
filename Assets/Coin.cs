using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private float speed;

    public void Collect()
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
