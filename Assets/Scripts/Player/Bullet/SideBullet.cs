using UnityEngine;

namespace PlayerNS.Bullet
{
    public enum Directions
    {
        Left,
        Right
    }

    public class SideBullet : BulletBase
    {
        public Directions direction;
        private readonly float horizontalSpeed = 1f;

        protected override void Move()
        {
            base.Move();
            MoveHorizontal();
        }

        private void MoveHorizontal()
        {
            switch (direction)
            {
                case Directions.Left:
                    transform.position += Vector3.left * horizontalSpeed * Time.deltaTime;
                    break;
                case Directions.Right:
                    transform.position += Vector3.right * horizontalSpeed * Time.deltaTime;
                    break;
            }
        }
    }
}