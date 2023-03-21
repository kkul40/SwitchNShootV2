using UnityEngine;

public class EnemyBubble : MonoBehaviour
{
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}