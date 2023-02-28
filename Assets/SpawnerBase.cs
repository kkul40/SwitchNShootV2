using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    [SerializeField] protected Transform spawnPrefab;
    [SerializeField] protected float spawnRate;
    [SerializeField] protected float waitForSecToSpawn;


    protected void Start()
    {
        InvokeRepeating("Spawn", waitForSecToSpawn, spawnRate);
    }
    

    protected virtual void Spawn()
    {
        float spawnPosx = Random.Range(CameraScr.Instance.cameraLeftCornerX.x, CameraScr.Instance.cameraRightCornerX.x);
        Vector3 spawnPos = new Vector3(spawnPosx, transform.position.y, 0f);
        Instantiate(spawnPrefab, spawnPos, Quaternion.identity);
    }
}
