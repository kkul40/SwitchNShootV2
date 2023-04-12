using System;
using System.Collections;
using UnityEngine;

namespace PlayerNS.Bullet
{
    public class LaserBubble : MonoBehaviour
    {
        [SerializeField] private GameObject bubble;
        [SerializeField] private float bubbleLifeTime;

        private void Start()
        {
            bubble.SetActive(false);
        }

        public void StartBubble(Vector2 pos)
        {
            StartCoroutine(BubbleRoutine(pos));
        }
        private IEnumerator BubbleRoutine(Vector2 pos)
        {
            bubble.SetActive(true);
            yield return new WaitForSeconds(bubbleLifeTime);
            bubble.SetActive(false);
        }
    }
}