using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerNS
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Vector2 playerStartPos;
        [SerializeField] private float speed;
        
        public Vector3 direction;

        private float cornerOffsetX;

        private void Start()
        {
            direction = Vector2.right;
            transform.position = playerStartPos;
            
            cornerOffsetX = GetComponent<BoxCollider2D>().bounds.extents.x;
        }
        public void MoveHorizontal()
        {
            transform.position += direction * (speed * Time.deltaTime);
            CheckCorner();
        }
        public void MoveVertical()
        {
            transform.position += Vector3.down * speed / 5 * Time.deltaTime;
        }

        public void Switch()
        {
            if (direction == Vector3.right)
            {
                direction = Vector3.left;
            }
            else if (direction == Vector3.left)
            {
                direction = Vector3.right;
            }
        }
        
        private void CheckCorner()
        {
            // TODO Daha sonra left right border yerine sadece 1 de�i�ken kullanacak �ekilde de�i�tir
            if (transform.position.x + cornerOffsetX <= CameraScr.Instance.cameraLeftCornerX.x)
                transform.position = new Vector3(CameraScr.Instance.cameraRightCornerX.x + cornerOffsetX,
                    transform.position.y, 0f);
            
            else if (transform.position.x - cornerOffsetX >= CameraScr.Instance.cameraRightCornerX.x)
                transform.position = new Vector3(CameraScr.Instance.cameraLeftCornerX.x - cornerOffsetX,
                    transform.position.y, 0f);
        }
    }
}