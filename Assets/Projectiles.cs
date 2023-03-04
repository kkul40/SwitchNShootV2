using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public static event Action OnLaserFired;

    [SerializeField] private List<Transform> projectileList = new List<Transform>();
    public Transform choosenProjectile;

    [SerializeField] private Transform laser;
    private Transform laserTemp;
    private int howManyTimeLaserFired = 0;

    [SerializeField] private int projectileIndex;
    [SerializeField] private float laserDuration;
    [SerializeField] private bool isLaserFired;

    private void Start()
    {
        choosenProjectile = projectileList[projectileIndex];

    }
    public void ChooseProjectile()
    {
        if (isLaserFired)
            return;

        if (projectileIndex < projectileList.Count)
        {
            choosenProjectile = projectileList[projectileIndex];
        }
        else
        {
            choosenProjectile = null;
        }
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

        // add stage checks here
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

            if (!isLaserFired)
            {
                ShootLaser();
            }
        }
        else if (projectileIndex < projectileList.Count)
        {
            choosenProjectile = projectileList[projectileIndex];
        }
    }
}
