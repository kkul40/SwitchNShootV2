using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWalls : MonoBehaviour
{
    [SerializeField] private Transform LeftFireWall, RightFireWall;

    [SerializeField] private float firstPosX;
    [SerializeField] private float secondPosX;


    private void Start()
    {
        transform.position = Vector3.zero;
        CloseFireWalls();
    }


    public void OpenFireWalls()
    {
        LeftFireWall.gameObject.SetActive(true);
        RightFireWall.gameObject.SetActive(true);
    }
    public void CloseFireWalls()
    {
        LeftFireWall.gameObject.SetActive(false);
        RightFireWall.gameObject.SetActive(false);
    }

    public void SetFireWallsPos()
    {
        Vector3 leftWallFirstPos = new Vector3(-firstPosX, 0f, 0f);
        Vector3 leftWallSecondPos = new Vector3(-secondPosX, 0f, 0f);

        Vector3 rightWallFirstPos = new Vector3(firstPosX, 0f, 0f);
        Vector3 rightWallSecondPos = new Vector3(secondPosX, 0f, 0f);
    }
}
