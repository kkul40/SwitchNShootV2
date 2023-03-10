using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : SpawnerBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        FireWalls.OnStageChanged += StopSpawning;
        Boss.OnBossDeath += StartSpawning;
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        FireWalls.OnStageChanged -= StopSpawning;
        Boss.OnBossDeath -= StartSpawning;
    }



}
