using System;
using UnityEngine;

public class ParticleScr : MonoBehaviour
{
    public void SelfDestroy(float time)
    {
        Destroy(gameObject, time);
    }

    public void PlayParticleSystem()
    {
        transform.GetComponent<ParticleSystem>().Play();
    }
}