public class CoinSpawner : SpawnerBase
{
    public override void StopSpawning()
    {
        CancelInvoke("Spawn");
    }
}