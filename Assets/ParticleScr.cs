using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScr : MonoBehaviour
{
    public void SelfDestroy(float time)
    {
        Destroy(this.gameObject, time);
    }
}
