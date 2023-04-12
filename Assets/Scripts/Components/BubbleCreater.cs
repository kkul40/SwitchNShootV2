using System;
using System.Collections;
using UnityEngine;

namespace PlayerNS.Components
{
    public class BubbleCreater : MonoBehaviour
    {
        [SerializeField] private GameObject bubble;
        [SerializeField] private float bubbleLifeTime;

        public void StartBubble(Vector2 pos)
        {
            var bubbleTemp = Instantiate(bubble, pos, Quaternion.identity);
            Destroy(bubbleTemp, bubbleLifeTime);
        }
    }
}