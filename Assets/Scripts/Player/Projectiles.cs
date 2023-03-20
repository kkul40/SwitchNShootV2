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
    [SerializeField] private bool isLaserFired;
    private Transform laserTemp;

    private int LaserFiredCount;
    public int GetLaserFiredCount => LaserFiredCount;
    private void Start()
    {
        choosenProjectile = projectileList[projectileIndex];
    }

    public static event Action OnLaserFired;
    public static event Action OnLaserStopped;


    private void OnEnable()
    {
        Player.OnPlayerDeath += DestroyLaser;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= DestroyLaser;
    }

    private void DestroyLaser()
    {
        if(isLaserFired) 
            laserTemp.GetComponent<Laser>().DestroyLaser();;
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
        LaserFiredCount++;
        OnLaserFired?.Invoke();
        StartCoroutine(ResetLaser());
    }

    private IEnumerator ResetLaser()
    {
        yield return new WaitForSeconds(laserDuration);
        isLaserFired = false;
        OnLaserStopped?.Invoke();
        laserTemp.GetComponent<Laser>().DestroyLaser();

        //TODO add stage checks here do it later
        SetProjectileIndex(2);
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