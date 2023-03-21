using UnityEngine;

public class ParticleScr : MonoBehaviour
{
    public void SelfDestroy(float time)
    {
        Destroy(gameObject, time);
    }
}