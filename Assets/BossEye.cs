using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEye : MonoBehaviour, IDamagable
{
    private BoxCollider2D eyeCollider;
    private Animator anim;

    public bool isEyeOpen;

   
    private void Start()
    {
        eyeCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        SetEyeClose();
    }

    public void TakeDamage()
    {
        eyeCollider.enabled = false;
        anim.SetBool("isEyeOpen", false);
        isEyeOpen = false;
    }

    public void SetEyeOpen()
    {
        eyeCollider.enabled = true;
        isEyeOpen = true;
        anim.SetBool("isEyeOpen", true);
    }

    public void SetEyeClose()
    {
        eyeCollider.enabled = false;
        isEyeOpen = false;
        anim.SetBool("isEyeOpen", false);
    }
}
