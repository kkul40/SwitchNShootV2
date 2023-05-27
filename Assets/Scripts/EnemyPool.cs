using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool SharedInstance;

    public List<GameObject> enemyObjects;
    public List<GameObject> bossEnemyObjects;

    public GameObject enemyObjectToPool;
    public GameObject bossEnemyObjectToPool;

    public int amountEnemyToPool;
    public int amountBossEnemyToPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        enemyObjects = new List<GameObject>();
        GameObject tmp;
        for (var i = 0; i < amountEnemyToPool; i++)
        {
            tmp = Instantiate(enemyObjectToPool);
            tmp.transform.parent = transform;
            tmp.SetActive(false);
            enemyObjects.Add(tmp);
        }

        for (var i = 0; i < amountBossEnemyToPool; i++)
        {
            tmp = Instantiate(bossEnemyObjectToPool);
            tmp.transform.parent = transform;
            tmp.SetActive(false);
            bossEnemyObjects.Add(tmp);
        }
    }

    private GameObject AddEnemyToPool()
    {
        amountEnemyToPool++;
        GameObject tmp;
        tmp = Instantiate(enemyObjectToPool);
        tmp.transform.parent = transform;
        tmp.SetActive(false);
        enemyObjects.Add(tmp);

        return tmp;
    }

    private GameObject AddBossEnemyToPool()
    {
        amountBossEnemyToPool++;
        GameObject tmp;
        tmp = Instantiate(bossEnemyObjectToPool);
        tmp.transform.parent = transform;
        tmp.SetActive(false);
        bossEnemyObjects.Add(tmp);

        return tmp;
    }

    public GameObject GetPooledEnemyObject()
    {
        for (var i = 0; i < amountEnemyToPool; i++)
            if (!enemyObjects[i].activeInHierarchy)
                return enemyObjects[i];

        var temp = AddEnemyToPool();
        return temp;
    }

    public GameObject GetPooledBossEnemyObject()
    {
        for (var i = 0; i < amountBossEnemyToPool; i++)
            if (!bossEnemyObjects[i].activeInHierarchy)
                return bossEnemyObjects[i];

        var temp = AddBossEnemyToPool();
        return temp;
    }
}