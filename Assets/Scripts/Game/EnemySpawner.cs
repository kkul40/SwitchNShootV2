public class EnemySpawner : SpawnerBase
{
    private StageSystem stageSystem;

    protected override void Start()
    {
        base.Start();
        stageSystem = FindObjectOfType<StageSystem>();
    }

    public override void StartSpawning()
    {
        InvokeRepeating(nameof(Spawn), waitForSecToSpawn, stageSystem.GetEnemySpawnRate);
    }
}