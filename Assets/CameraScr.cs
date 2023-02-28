using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScr : MonoBehaviour
{
    public static CameraScr Instance;

    [SerializeField] private BoxCollider2D boxCollider;

    private Vector3 orignalPosition;

    public Vector3 cameraLeftCornerX, cameraRightCornerX;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        orignalPosition = transform.position;
        boxCollider = GetComponent<BoxCollider2D>();

        cameraLeftCornerX = new Vector3(transform.position.x - boxCollider.bounds.extents.x, 0f, 0f);
        cameraRightCornerX = new Vector3(transform.position.x + boxCollider.bounds.extents.x, 0f, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(cameraLeftCornerX, cameraRightCornerX);
    }


    public void CameraShake(float duration, float magnitude)
    {
        StartCoroutine(Shake(duration, magnitude));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = orignalPosition;
    }
}
