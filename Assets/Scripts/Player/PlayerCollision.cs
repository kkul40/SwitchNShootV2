using System;
using UnityEngine;

namespace PlayerNS
{
    public class PlayerCollision : MonoBehaviour
    {
        public PlayerManager playerManager;
        
        [SerializeField] private BoxCollider2D boxCollider;

        private void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            boxCollider = GetComponent<BoxCollider2D>();
        }
        
        public void CheckCollisions()
        {
            var colliderResults =
                Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.extents * 2, 0, Vector2.zero);
        
            foreach (var item in colliderResults)
                if (item.transform.TryGetComponent(out IDamagable damagable))
                {
                    if (item.transform.CompareTag("Enemy"))
                    {
                        damagable.TakeDamage();
                        playerManager.TakeDamage();
                    }
                    else if (item.transform.CompareTag("Boss"))
                    {
                        playerManager.TakeDamage();
                    }
                }
                else if (item.transform.TryGetComponent(out ICollectable collectable))
                {
                    collectable.Collect();
                    playerManager.projectileManager.LevelUp();
                }
                else if (item.transform.TryGetComponent(out EnemyBubble enemyBubble))
                {
                    playerManager.TakeDamage();
                }
                else if (item.transform.CompareTag("FireWall"))
                {
                    playerManager.TakeDamage();
                }
        }
    }
}