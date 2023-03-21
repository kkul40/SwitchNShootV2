using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossLaser : MonoBehaviour, IDamagable
{
    [SerializeField] private Transform enemyLaser;
    [SerializeField] private ParticleScr laserPartical;
    
    

    public void StartLaser(float laserDuration)
    {
        laserPartical.PlayParticleSystem();

        var shooTime = laserDuration - 2;
        Invoke(nameof(ShootLaser), 2);
    }

    private void ShootLaser()
    {
        float duration = 3;
        float magnitute = 0.5f;
        
        CameraScr.Instance.CameraShakeY(duration,magnitute);
        enemyLaser.gameObject.SetActive(true);
    }
    public void StopLaser()
    {
        laserPartical.StopParticleSystem();
        enemyLaser.gameObject.SetActive(false);
    }
    

    public void TakeDamage()
    {
        // Do Nothing Just Sit Here
    }
}
