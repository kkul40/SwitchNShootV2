using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] private Transform powerUpPrefab;

    [SerializeField] private float speed;

    [FormerlySerializedAs("soundEffect")] [SerializeField]
    private AudioClip CoinCollected;

    [FormerlySerializedAs("soundEffect")] [SerializeField]
    private AudioClip coinMissed;

    private bool isDestroyed;

    public static event Action OnCoinCollected;
    public static event Action OnCoinMissed;

    private void FixedUpdate()
    {
        CheckCorner();
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void Collect()
    {
        OnCoinCollected?.Invoke();
        SpawnText("POWER-UP");
        SoundManager.Instance.PlayOneShot(CoinCollected);
        Destroy(gameObject);
    }

    private void CheckCorner()
    {
        if (isDestroyed) return;

        if (transform.position.y <= -7.5)
        {
            isDestroyed = true;
            OnCoinMissed?.Invoke();
            SpawnText("MISSED");
            SoundManager.Instance.PlayOneShot(coinMissed);
            Destroy(gameObject, 2);
        }
    }

    private void SpawnText(string message)
    {
        var text = Instantiate(powerUpPrefab, transform.position, quaternion.identity);
        text.GetComponent<PopupText>().SetText(message);
        Destroy(text.gameObject, 0.5f);
    }
}