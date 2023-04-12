using System;
using PlayerNS.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerNS.Bullet
{
    public class BulletBase : MonoBehaviour
    {
        [SerializeField] protected float speed;

        protected BoxCollider2D boxCollider;
        protected SoundPlayer soundPlayer;
        protected BubbleCreater bubbleCreater;

        protected virtual void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
            soundPlayer = GetComponent<SoundPlayer>();
            bubbleCreater = GetComponent<BubbleCreater>();
        }

        protected virtual void FixedUpdate()
        {
            Move();
            CheckCorner();
            CheckCollisions();
        }
        /// <summary>
        /// Move UP
        /// </summary>
        protected virtual void Move()
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }
        /// <summary>
        /// Check If Object outside of the map
        /// </summary>
        protected virtual void CheckCorner()
        {
            if (transform.position.y >= 9) Destroy(gameObject);
        }
        
        protected virtual void CheckCollisions()
        {
            var colliderResults =
                Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.extents * 2, 0, Vector2.zero);

            foreach (var item in colliderResults)
            {
                if (item.transform.TryGetComponent(out IDamagable damagable) && !item.transform.CompareTag("Player"))
                {
                    if (damagable != null)
                    {
                        damagable.TakeDamage();
                        bubbleCreater.StartBubble(transform.position);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}