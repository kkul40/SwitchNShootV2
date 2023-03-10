using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEye : MonoBehaviour, IDamagable
{
    [SerializeField] private BoxCollider2D eyeCollider;
    [SerializeField] private Animator anim;

    public bool isEyeOpen;

   
    private void Start()
    {
        /*eyeCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();*/

        //SetEyeClose();
    }

    public void TakeDamage()
    {
        SetEyeClose();
    }

    public void SetEyeOpen()
    {
        if (eyeCollider.enabled)
            return;

        eyeCollider.enabled = true;
        isEyeOpen = true;
        anim.SetBool("isEyeOpen", true);
    }

    public void SetEyeClose()
    {
        if (!eyeCollider.enabled)
            return;

        eyeCollider.enabled = false;
        isEyeOpen = false;
        anim.SetBool("isEyeOpen", false);
    }
}
