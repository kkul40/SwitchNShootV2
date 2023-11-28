using System;
using UnityEngine;
using UnityEngine.Serialization;

public class FireWalls : MonoBehaviour
{
    [SerializeField] private Transform LeftFireWall, RightFireWall;

    [SerializeField] private float startPosX;
    private float tempStartPosX;

    private int triggerCount;

    private void Start()
    {
        transform.position = Vector3.zero;

        tempStartPosX = startPosX;
    }

    public static event Action OnFireWallStopped;
    public static event Action OnFireWallFirstUsed;
    public static event Action OnFireWallSecondtUsed;

    public void OpenFireWalls()
    {
        tempStartPosX -= 1;
        
        var leftWallFirstPos = new Vector3(-tempStartPosX, 0f, 0f);
        var rightWallFirstPos = new Vector3(tempStartPosX, 0f, 0f);
        
        CameraBorder.SetBorder(-tempStartPosX, tempStartPosX);

        LeftFireWall.position = leftWallFirstPos;
        RightFireWall.position = rightWallFirstPos;

        OnFireWallFirstUsed?.Invoke();

        LeftFireWall.gameObject.SetActive(true);
        RightFireWall.gameObject.SetActive(true);
    }

    public void ResetFireWalls()
    {
        tempStartPosX = startPosX;
        
        if (LeftFireWall != null)
            LeftFireWall.gameObject.SetActive(false);

        if (RightFireWall != null)
            RightFireWall.gameObject.SetActive(false);

        OnFireWallStopped?.Invoke();
    }
}