using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private List<Transform> projectileList = new();
    public Transform choosenProjectile;

    [SerializeField] private Transform laser;

    [SerializeField] private int projectileIndex;
    [SerializeField] private float laserDuration;
    private bool isLaserFired;
    private Transform laserTemp;
    

    private int startingProjectileIndex;
    public int GetLaserFiredCount { get; private set; }
    
    [SerializeField] public Transform projectileSpawnPoint;
    
    public static event Action OnShoot;

    private void Start()
    {
        choosenProjectile = projectileList[projectileIndex];
    }

    private void OnEnable()
    {
        PlayerManager.OnPlayerDeath += DestroyLaser;
        Boss.OnBossDeath += ResetLaserNow;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerDeath -= DestroyLaser;
        Boss.OnBossDeath -= ResetLaserNow;
    }

    public static event Action OnLaserFired;
    public static event Action OnLaserStopped;

    private void DestroyLaser()
    {
        isLaserFired = false;
        
        if (isLaserFired)
            laserTemp.GetComponent<Laser>().DestroyLaser();
    }

    
    public void Shoot()
    {
        var projectile = choosenProjectile;

        if (projectile == null)
            return;

        Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
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
        
        if(laserTemp.TryGetComponent(out Laser laser))
            laser.DestroyLaser();
        
        //TODO daha sorna buradaki +1 olayını incele
        //SetProjectileIndex(GetLaserFiredCount + 1 % 3 == 0 ? startingProjectileIndex : ++startingProjectileIndex);
        SetProjectileIndex(startingProjectileIndex);

        if (startingProjectileIndex > projectileList.Count - 2) startingProjectileIndex = projectileList.Count - 2;
    }

    private void SetProjectileIndex(int index)
    {
        // for now
        if (index < 0 || index >= projectileList.Count)
        {
            Debug.LogError("Projectile Index Incorrect");
            return;
        }

        projectileIndex = index;
        choosenProjectile = projectileList[projectileIndex];
    }


    public void LevelUp()
    {
        if (isLaserFired) return;
        
        projectileIndex++;

        if (projectileIndex > projectileList.Count - 1)
        {
            projectileIndex = projectileList.Count - 1;
            choosenProjectile = null;
            Debug.Log("shoot Laser");
            if (!isLaserFired) ShootLaser();
        }
        else
        {
            choosenProjectile = projectileList[projectileIndex];
        }
    }
}