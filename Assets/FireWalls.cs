using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWalls : MonoBehaviour
{
    public static event Action OnFireWallStopped;
    public static event Action OnFireWallFirstUsed;
    public static event Action OnFireWallSecondtUsed;

    [SerializeField] private Transform LeftFireWall, RightFireWall;


    [SerializeField] private float firstPosX;
    [SerializeField] private float secondPosX;

    private int triggerCount;


    private void Start()
    {
        transform.position = Vector3.zero;
    }


    public void OpenFireWalls()
    {
        SetFireWallsPos();
        LeftFireWall.gameObject.SetActive(true);
        RightFireWall.gameObject.SetActive(true);
    }
    public void CloseFireWalls()
    {
        LeftFireWall.gameObject.SetActive(false);
        RightFireWall.gameObject.SetActive(false);

        OnFireWallStopped?.Invoke();
        TriggerCount();
    }

    public void SetFireWallsPos()
    {
        if (triggerCount < 0 || triggerCount > 1)
        {
            Debug.LogError("trigger count unavable");
            return;
        }

        switch (triggerCount)
        {
            case 0:
                Vector3 leftWallFirstPos = new Vector3(-firstPosX, 0f, 0f);
                Vector3 rightWallFirstPos = new Vector3(firstPosX, 0f, 0f);

                LeftFireWall.position = leftWallFirstPos;
                RightFireWall.position = rightWallFirstPos;

                OnFireWallFirstUsed?.Invoke();
                break;
            case 1:
                Vector3 leftWallSecondPos = new Vector3(-secondPosX, 0f, 0f);
                Vector3 rightWallSecondPos = new Vector3(secondPosX, 0f, 0f);

                LeftFireWall.position = leftWallSecondPos;
                RightFireWall.position = rightWallSecondPos;

                OnFireWallSecondtUsed?.Invoke();
                break;

            default:
                break;
        }
        
    }

    private void TriggerCount()
    {
        triggerCount++;

        if (triggerCount > 1)
        {
            // new stage alert
            Debug.Log("Next Stage Alert");
            triggerCount = 0;
        }
    }
}
