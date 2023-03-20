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
    private int howManyTimeLaserFired;
    private Transform laserTemp;

    private void Start()
    {
        choosenProjectile = projectileList[projectileIndex];
    }

    public static event Action OnLaserFired;

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
        OnLaserFired?.Invoke();
        StartCoroutine(ResetLaser());
    }

    private IEnumerator ResetLaser()
    {
        yield return new WaitForSeconds(laserDuration);
        isLaserFired = false;
        howManyTimeLaserFired++;

        // add stage checks here do it later
        SetProjectileIndex(2);
        laserTemp.GetComponent<Laser>().DestroyLaser();
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