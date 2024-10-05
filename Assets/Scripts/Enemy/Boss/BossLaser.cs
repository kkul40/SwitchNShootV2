using UnityEngine;

public class BossLaser : MonoBehaviour, IDamagable
{
    [SerializeField] private Transform enemyLaser;
    [SerializeField] private ParticleScr laserPartical;

    [SerializeField] private AudioClip bossLaserSoundEffect;

    public void TakeDamage()
    {
        // Do Nothing Just Sit Here
    }

    public void StartLaser(float laserDuration)
    {
        laserPartical.PlayParticleSystem();

        var shootTime = laserDuration - 2;
        Invoke(nameof(ShootLaser), 2);
    }

    private void ShootLaser()
    {
        float duration = 3;
        var magnitute = 0.5f;

        CameraScr.Instance.CameraShakeY(duration, magnitute);
        enemyLaser.gameObject.SetActive(true);
        InvokeRepeating(nameof(PlaySound), 0, 0.1f);
    }

    public void StopLaser()
    {
        CancelInvoke(nameof(PlaySound));
        laserPartical.StopParticleSystem();
        enemyLaser.gameObject.SetActive(false);
    }

    public void StoplaserNow()
    {
        CancelInvoke(nameof(PlaySound));
        CancelInvoke(nameof(ShootLaser));
        CameraScr.Instance.StopCameraShakeY();
        laserPartical.StopParticleSystem();
        enemyLaser.gameObject.SetActive(false);
    }

    private void PlaySound()
    {
        SoundManager.Instance.PlaySoundEffect(bossLaserSoundEffect);
    }
}