using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed;

    [SerializeField] protected Transform Bubble;

    [SerializeField] protected AudioClip fireSoundEffect;

    protected BoxCollider2D _boxCollider;

    protected virtual void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        SoundManager.Instance.PlayOneShot(fireSoundEffect);
    }

    protected virtual void FixedUpdate()
    {
        CheckCorner();
        CheckCollisions();
        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    protected virtual void CheckCorner()
    {
        if (transform.position.y >= 9) Destroy(gameObject);
    }

    protected virtual void CheckCollisions()
    {
        var colliderResults =
            Physics2D.BoxCastAll(_boxCollider.bounds.center, _boxCollider.bounds.extents * 2, 0, Vector2.zero);

        foreach (var item in colliderResults)
            if (item.transform.TryGetComponent(out IDamagable damagable) && !item.transform.CompareTag("Player"))
                if (damagable != null)
                {
                    damagable.TakeDamage();
                    InstantaiteBubble(item.transform.position);
                    Destroy(gameObject);
                }
    }

    protected void InstantaiteBubble(Vector2 pos)
    {
        if (Random.value < 0.5)
        {
            var bubble = Instantiate(Bubble, pos, Quaternion.identity);
            Destroy(bubble.gameObject, 0.066f);
        }
    }
}