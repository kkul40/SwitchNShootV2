using Unity.Mathematics;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private Transform powerUpPrefab;

    [SerializeField] private float speed;

    [SerializeField] private AudioClip soundEffect;


    private void FixedUpdate()
    {
        CheckCorner();
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void Collect()
    {
        var text = Instantiate(powerUpPrefab, transform.position, quaternion.identity);
        Destroy(text.gameObject, 0.5f);
        SoundManager.Instance.PlayOneShot(soundEffect);
        Destroy(gameObject);
    }

    private void CheckCorner()
    {
        if (transform.position.y <= -10) Destroy(gameObject);
    }
}