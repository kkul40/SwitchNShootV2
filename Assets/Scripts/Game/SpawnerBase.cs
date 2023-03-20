using UnityEngine;

public class SpawnerBase : MonoBehaviour
{
    [SerializeField] protected Transform spawnPrefab;
    [SerializeField] protected float spawnPosOffsetX;
    [SerializeField] protected float spawnRate;
    [SerializeField] protected float waitForSecToSpawn;

    // 3 çeşit sapwnoffset var ilki 1 ikinci 1.5 ve sonunucsu 2 firewalla göre ayarama yap
    protected virtual void Start()
    {
        //InvokeRepeating("Spawn", waitForSecToSpawn, spawnRate);
    }

    protected virtual void OnEnable()
    {
        FireWalls.OnFireWallStopped += SpawnPosXTo1;
        FireWalls.OnFireWallFirstUsed += SpawnPosXTo15;
        FireWalls.OnFireWallSecondtUsed += SpawnPosXTo2;
    }

    protected virtual void OnDisable()
    {
        FireWalls.OnFireWallStopped -= SpawnPosXTo1;
        FireWalls.OnFireWallFirstUsed -= SpawnPosXTo15;
        FireWalls.OnFireWallSecondtUsed -= SpawnPosXTo2;
    }

    protected virtual void SpawnPosXTo1()
    {
        spawnPosOffsetX = 1;
    }

    protected virtual void SpawnPosXTo15()
    {
        spawnPosOffsetX = 1.5f;
    }

    protected virtual void SpawnPosXTo2()
    {
        spawnPosOffsetX = 2;
    }

    public virtual void Spawn()
    {
        var spawnPosx = Random.Range(CameraScr.Instance.cameraLeftCornerX.x + spawnPosOffsetX,
            CameraScr.Instance.cameraRightCornerX.x - spawnPosOffsetX);
        var spawnPos = new Vector3(spawnPosx, transform.position.y, 0f);
        Instantiate(spawnPrefab, spawnPos, Quaternion.identity);
    }


    [ContextMenu("StartSpawning")]
    public virtual void StartSpawning()
    {
        InvokeRepeating("Spawn", waitForSecToSpawn, spawnRate);
    }

    [ContextMenu("StopSpawning")]
    public virtual void StopSpawning()
    {
        CancelInvoke("Spawn");
    }
}