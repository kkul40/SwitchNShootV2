using System.Collections;
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

    private IEnumerator SpawnCo()
    {
        while (true)
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

            Debug.Log(stageSystem.GetEnemySpawnRate);
            yield return new WaitForSeconds(stageSystem.GetEnemySpawnRate);
        }
    }

    public override void StartSpawning()
    {
        StartCoroutine(nameof(SpawnCo));
    }

    public override void StopSpawning()
    {
        StopAllCoroutines();
    }
}