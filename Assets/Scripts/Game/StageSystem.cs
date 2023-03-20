using UnityEngine;

public class StageSystem : MonoBehaviour
{
    [SerializeField] private Transform bossPrefab;
    [SerializeField] private Transform bossSpawnPos;

    private bool isBossActive;

    private void OnEnable()
    {
        FireWalls.OnStageChanged += SpawnBoss;
        Boss.OnBossDeath += SetBossActiveToFalse;
    }

    private void OnDisable()
    {
        FireWalls.OnStageChanged -= SpawnBoss;
        Boss.OnBossDeath -= SetBossActiveToFalse;
    }

    private void SpawnBoss()
    {
        if (isBossActive)
            return;

        isBossActive = true;
        Instantiate(bossPrefab, bossSpawnPos.position, Quaternion.identity);
    }

    private void SetBossActiveToFalse()
    {
        isBossActive = false;
    }
}