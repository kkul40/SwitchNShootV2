using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] private GameObject bubble;
    [SerializeField] private float bubbleLifeTime;

    private void Start()
    {
        bubble.SetActive(false);
    }

    private void OnEnable()
    {
        Player.OnShoot += StartBubble;
    }

    private void OnDisable()
    {
        Player.OnShoot -= StartBubble;
    }

    private void StartBubble()
    {
        StartCoroutine(BubbleRoutine());
    }

    IEnumerator BubbleRoutine()
    {
        bubble.SetActive(true);
        yield return new WaitForSeconds(bubbleLifeTime);
        bubble.SetActive(false);
    }

}
