using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class SpawnerBase : MonoBehaviour
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
        /*
        GameObject enemy = EnemyPool.SharedInstance.GetPooledEnemyObject(); 
        if (enemy != null) {
            enemy.transform.position = spawnPos;
            enemy.transform.rotation = quaternion.identity;
            enemy.SetActive(true);
        }*/

        Instantiate(spawnPrefab, spawnPos, Quaternion.identity);
    }

    public void Spawn(Vector3 pos)
    {
        Debug.Log("Coin Spawner Cam " + pos);
        var spawnPos = pos;
        var coin = Instantiate(spawnPrefab, spawnPos, quaternion.identity);
        Coin coinScr = coin.GetComponent<Coin>();
        coinScr.moveSpeed = 0f;
        coinScr.SetGravity(1);
    }


    [ContextMenu("StartSpawning")]
    public virtual void StartSpawning()
    {
        InvokeRepeating("Spawn", waitForSecToSpawn, spawnRate);
    }

    public abstract void StopSpawning();
    
}