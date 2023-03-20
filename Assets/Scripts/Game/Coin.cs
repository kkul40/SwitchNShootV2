using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        CheckCorner();
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void Collect()
    {
        Destroy(gameObject);
    }

    private void CheckCorner()
    {
        if (transform.position.y <= -10) Destroy(gameObject);
    }
}