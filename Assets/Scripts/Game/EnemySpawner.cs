using System.Collections;
using Game.Manager;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : SpawnerBase
{
    private StageManager stageManager;

    protected override void Start()
    {
        base.Start();
        stageManager = FindObjectOfType<StageManager>();
    }

    private IEnumerator SpawnCo()
    {
        while (true)
        {
            var spawnPosx = Random.Range(CameraBorder.leftX + spawnPosOffsetX,
                CameraBorder.rightX - spawnPosOffsetX);

            var spawnPos = new Vector3(spawnPosx, transform.position.y, 0f);


            var enemy = EnemyPool.SharedInstance.GetPooledEnemyObject();
            if (enemy != null)
            {
                enemy.transform.position = spawnPos;
                enemy.transform.rotation = quaternion.identity;
                enemy.SetActive(true);
            }

            Debug.Log(EnemySpawnRateManager.instance.enemySpawnRate);
            yield return new WaitForSeconds(EnemySpawnRateManager.instance.enemySpawnRate);
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