using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float speed;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        CheckCorner();
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
        var duration = 0.1f;
        var magnitude = 0.2f;
        CameraScr.Instance.CameraShake(duration, magnitude);
    }

    public void TakeDamage(float t)
    {
        Destroy(gameObject, t);
    }

    private void CheckCorner()
    {
        if (transform.position.y <= Player.Instance.GetPlayerPosY())
        {
            //Start Death Sequence
            TakeDamage(1);
            speed = 0;
        }

        if (transform.position.y <= -10) Destroy(gameObject);
    }
}