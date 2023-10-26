using PlayerNS.Components;
using UnityEngine;

namespace PlayerNS.Bullet
{
    public class BulletBase : MonoBehaviour
    {
        [SerializeField] protected float speed;

        protected BoxCollider2D boxCollider;
        protected BubbleCreater bubbleCreater;
        protected SoundPlayer soundPlayer;

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
        ///     Move UP
        /// </summary>
        protected virtual void Move()
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
        }

        /// <summary>
        ///     Check If Object outside of the map
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
                if (item.transform.TryGetComponent(out IDamagable damagable) && !item.transform.CompareTag("Player"))
                    if (damagable != null)
                    {
                        damagable.TakeDamage();
                        bubbleCreater.StartBubble(transform.position);
                        Destroy(gameObject);
                    }
        }

        public void SelfDestroy()
        {
            Destroy(this.gameObject);
        }
    }
}