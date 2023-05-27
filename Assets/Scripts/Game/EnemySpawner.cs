using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : SpawnerBase
{
    private StageSystem stageSystem;

    protected override void Start()
    {
        base.Start();
        stageSystem = FindObjectOfType<StageSystem>();
    }

    public override void Spawn()
    {
        var spawnPosx = Random.Range(CameraScr.Instance.cameraLeftCornerX.x + spawnPosOffsetX,
            CameraScr.Instance.cameraRightCornerX.x - spawnPosOffsetX);

        var spawnPos = new Vector3(spawnPosx, transform.position.y, 0f);


        var enemy = EnemyPool.SharedInstance.GetPooledEnemyObject();
        if (enemy != null)
        {
            enemy.transform.position = spawnPos;
            enemy.transform.rotation = quaternion.identity;
            enemy.SetActive(true);
        }

        //Instantiate(spawnPrefab, spawnPos, Quaternion.identity);
    }

    public override void StartSpawning()
    {
        InvokeRepeating(nameof(Spawn), waitForSecToSpawn, stageSystem.GetEnemySpawnRate);
    }
}