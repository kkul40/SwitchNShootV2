using UnityEngine;

public class BossLaser : MonoBehaviour, IDamagable
{
    [SerializeField] private Transform enemyLaser;
    [SerializeField] private ParticleScr laserPartical;


    public void TakeDamage()
    {
        // Do Nothing Just Sit Here
    }


    public void StartLaser(float laserDuration)
    {
        laserPartical.PlayParticleSystem();

        var shooTime = laserDuration - 2;
        Invoke(nameof(ShootLaser), 2);
    }

    private void ShootLaser()
    {
        float duration = 3;
        var magnitute = 0.5f;

        CameraScr.Instance.CameraShakeY(duration, magnitute);
        enemyLaser.gameObject.SetActive(true);
    }

    public void StopLaser()
    {
        laserPartical.StopParticleSystem();
        enemyLaser.gameObject.SetActive(false);
    }
}