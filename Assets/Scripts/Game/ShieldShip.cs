using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayerNS.Game
{
    public class ShieldShip : MonoBehaviour, IDamagable
    {
        [SerializeField] private Transform shieldPrefab;
        [SerializeField] private float speed;
        [SerializeField] private GameObject enemyParticlePrefab;

        private void FixedUpdate()
        {
            CheckCorners();
            Move();
        }

        private void Move()
        {
            transform.position += Vector3.right * (speed * Time.deltaTime);
        }

        private void CheckCorners()
        {
            //Do something
        }

        public void TakeDamage()
        {
            Instantiate(shieldPrefab, transform.position, quaternion.identity);
            SpawnParticle();
            Destroy(this.gameObject);
        }
        
        private void SpawnParticle()
        {
            var particle = Instantiate(enemyParticlePrefab, transform.position, quaternion.identity);

            particle.GetComponent<ParticleScr>().SelfDestroy(2f);
        }
    }
}