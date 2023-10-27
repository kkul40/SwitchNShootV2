using System;
using Unity.Mathematics;
using UnityEngine;

public class Shield : MonoBehaviour, ICollectable
{
    [SerializeField] private Transform powerUpPrefab;

    [SerializeField] private float speed;
    private AudioClip shieldCollectedSound;
    private AudioClip shieldMissedSound;

    private bool isDestroyed;

    public static event Action OnShieldCollected;

    private void FixedUpdate()
    {
        CheckCorner();
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void CheckCorner()
    {
        if (isDestroyed) return;
        
        if (transform.position.y <= -7.5)
        {
            isDestroyed = true;
            SpawnText("MISSED SHIELD");
            SoundManager.Instance.PlayOneShot(shieldMissedSound);
            Destroy(gameObject, 2);
        }
    }

    public void SelfDestroy()
    {
        SpawnText("REMOVED");
        Destroy(gameObject);
    }
    
    private void SpawnText(string message)
    {
        var text = Instantiate(powerUpPrefab, transform.position, quaternion.identity);
        text.GetComponent<PopupText>().Instantiate(message, new Color(0, 97,255));
        Destroy(text.gameObject, 0.5f);
    }


    public void Collect()
    {
        OnShieldCollected?.Invoke();
        SpawnText("SHIELD-UP");
        // SoundManager.Instance.PlayOneShot(CoinCollected);
        Destroy(gameObject);
    }
}
