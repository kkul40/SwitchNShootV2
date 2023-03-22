using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
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

    private void Start()
    {
        choosenProjectile = projectileList[projectileIndex];
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += DestroyLaser;
        Boss.OnBossDeath += ResetLaserNow;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= DestroyLaser;
        Boss.OnBossDeath -= ResetLaserNow;
    }

    public static event Action OnLaserFired;
    public static event Action OnLaserStopped;

    private void DestroyLaser()
    {
        if (isLaserFired)
            laserTemp.GetComponent<Laser>().DestroyLaser();
        ;
    }

    public void ChooseProjectile()
    {
        if (isLaserFired)
            return;

        if (projectileIndex < projectileList.Count)
            choosenProjectile = projectileList[projectileIndex];
        else
            choosenProjectile = null;
    }

    private void ShootLaser()
    {
        isLaserFired = true;
        laserTemp = Instantiate(laser, Player.Instance.GetFirePointPos(), Quaternion.identity);
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
        SetProjectileIndex(GetLaserFiredCount + 1 % 3 == 0 ? startingProjectileIndex : ++startingProjectileIndex);

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
        projectileIndex++;

        if (projectileIndex >= projectileList.Count)
        {
            projectileIndex = projectileList.Count;
            choosenProjectile = null;

            if (!isLaserFired) ShootLaser();
        }
        else if (projectileIndex < projectileList.Count)
        {
            choosenProjectile = projectileList[projectileIndex];
        }
    }
}