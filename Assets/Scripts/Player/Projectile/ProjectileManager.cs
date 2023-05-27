using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private List<Transform> projectileList = new();
    [SerializeField] private Transform laser;

    public int currentProjectileIndex { get; private set; }
    private Transform currentProjectile;
    public GameObject BulletContainer;
    
    [SerializeField] public Transform projectileSpawnPoint;

    private void SwichtToNextProjectile()
    {
        currentProjectileIndex++;
        
        if(currentProjectileIndex > projectileList.Count - 1)
        {
            currentProjectileIndex = projectileList.Count - 1;
            
            if (!isLaserFired) ShootLaser(); // Stage atla
        }
        
        OnIndexChange?.Invoke();

        currentProjectile = projectileList[currentProjectileIndex];
        ChooseProjectile(currentProjectileIndex);
    }

    private void SwitchTooPreviousProjectile()
    {
        currentProjectileIndex--;
        
        if(currentProjectileIndex < 0)
        {
            currentProjectileIndex = 0;
        }
        
        OnIndexChange?.Invoke();

        currentProjectile = projectileList[currentProjectileIndex];
        ChooseProjectile(currentProjectileIndex);
    }
    
    private void ChooseProjectile(int index)
    {
        if(index < 0 || index > projectileList.Count -1)
            Debug.LogError("Projectile index is incorrect : index = "+ index );
        
        currentProjectile = projectileList[index];
        
    }
    
    
    
    
    
    


    [SerializeField] private float laserDuration;

    private bool isLaserFired;
    private Transform laserTemp;


    // private int startingProjectileIndex;
    public int GetLaserFiredCount { get; private set; }

    private void Start()
    {
        currentProjectileIndex = 0;
        
        currentProjectile = projectileList[currentProjectileIndex];
    }

    private void OnEnable()
    {
        Coin.OnCoinCollected += SwichtToNextProjectile;
        Coin.OnCoinMissed += SwitchTooPreviousProjectile;
        PlayerManager.OnPlayerDeath += DestroyLaser;
        Boss.OnBossDeath += ResetLaserNow;
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= SwichtToNextProjectile;
        Coin.OnCoinMissed -= SwitchTooPreviousProjectile;
        PlayerManager.OnPlayerDeath -= DestroyLaser;
        Boss.OnBossDeath -= ResetLaserNow;
    }

    public static event Action OnShoot;
    public static event Action OnIndexChange;

    public static event Action OnLaserFired;
    public static event Action OnLaserStopped;

    private void DestroyLaser()
    {
        if (isLaserFired)
        {
            laserTemp.GetComponent<LaserManager>().DestroyLaser();
            isLaserFired = false;
        }
    }


    public void Shoot()
    {
        if (isLaserFired) return;
        
        var projectile = currentProjectile;

        if (projectile == null)
            return;

        var temp = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
        temp.transform.parent = BulletContainer.transform;
        OnShoot?.Invoke();
    }

    private void ShootLaser()
    {
        isLaserFired = true;
        laserTemp = Instantiate(laser, PlayerManager.Instance.GetFirePointPos(), Quaternion.identity);
        GetLaserFiredCount++;
        OnLaserFired?.Invoke();
        Invoke(nameof(ResetLaser), laserDuration);
    }

    private void ResetLaserNow()
    {
        CancelInvoke(nameof(ResetLaser));
        ResetLaser();
    }

    private void ResetLaser()
    {
        isLaserFired = false;
        OnLaserStopped?.Invoke();
        ChooseProjectile(0);

        if (laserTemp.TryGetComponent(out LaserManager laser))
            laser.DestroyLaser();

        //TODO daha sorna buradaki +1 olayını incele
        //SetProjectileIndex(GetLaserFiredCount + 1 % 3 == 0 ? startingProjectileIndex : ++startingProjectileIndex);
        ChooseProjectile(0);

        // if (projectileIndex > projectileList.Count - 2) projectileIndex = projectileList.Count - 2;
    }
}