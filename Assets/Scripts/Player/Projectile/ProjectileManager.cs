using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private List<Transform> projectileList = new();
    [SerializeField] private Transform laser;

    public int currentProjectileIndex { get; private set; }
    public GameObject BulletContainer;

    private bool isLaserFired;
    
    [SerializeField] public Transform projectileSpawnPoint;
    
    
    public static event Action OnShoot;
    public static event Action OnIndexChange;
    public static event Action OnLaserFired;
    public static event Action OnLaserStopped;
    public static event Action OnHyperDrived;
    
    
    private void Start()
    {
        currentProjectileIndex = 0;
        
        laser.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Coin.OnCoinCollected += SwichtToNextProjectile;
        Coin.OnCoinMissed += SwitchToPreviousProjectile;
        // PlayerManager.OnPlayerDeath += DestroyLaser;
        // Boss.OnBossDeath += ResetLaserNow;
    }

    private void OnDisable()
    {
        Coin.OnCoinCollected -= SwichtToNextProjectile;
        Coin.OnCoinMissed -= SwitchToPreviousProjectile;
        // PlayerManager.OnPlayerDeath -= DestroyLaser;
        // Boss.OnBossDeath -= ResetLaserNow;
    }

    private void SwichtToNextProjectile()
    {
        if (isLaserFired)
        {
            Debug.Log("Next Stage");
            OnHyperDrived?.Invoke();
            return;
        }
        
        currentProjectileIndex++;
        OnIndexChange?.Invoke();
        
        if(currentProjectileIndex >= projectileList.Count)
        {
            currentProjectileIndex = projectileList.Count;
            // TODO Create laser
            isLaserFired = true;
            OnLaserFired?.Invoke();
            laser.gameObject.SetActive(true);
        }
    }

    private void SwitchToPreviousProjectile()
    {
        currentProjectileIndex--;
        OnIndexChange?.Invoke();
        
        if(currentProjectileIndex < 0)
        {
            currentProjectileIndex = 0;
        }

        if (isLaserFired)
        {
            isLaserFired = false;
            OnLaserStopped?.Invoke();
            laser.gameObject.SetActive(false);
        }
    }

    public void ChooseProjectile(int index)
    {
        if (isLaserFired)
        {
            isLaserFired = false;
            OnLaserStopped?.Invoke();
            laser.gameObject.SetActive(false);
        }
        
        if(index < 0 || index > projectileList.Count -1)
            Debug.LogError("Projectile index is incorrect : index = "+ index );

        currentProjectileIndex = index;
        
    }

    public void Shoot()
    {
        if (isLaserFired)
            return;

        if (currentProjectileIndex == projectileList.Count)
        {
            //ShootLaser
            Debug.Log("Testttt");
            return;
        }
        
        var projectile = projectileList[currentProjectileIndex];

        var temp = Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
        temp.transform.parent = BulletContainer.transform;
        OnShoot?.Invoke();
    }
}