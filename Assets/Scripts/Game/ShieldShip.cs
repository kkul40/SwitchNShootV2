using System;
using Unity.Mathematics;
using UnityEngine;

namespace PlayerNS.Game
{
    public class ShieldShip : MonoBehaviour, IDamagable
    {
        [SerializeField] private Transform shieldPrefab;
        [SerializeField] private float speed;

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
        }
    }
}