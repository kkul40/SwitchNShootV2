using UnityEngine;
using UnityEngine.Serialization;


public class BossEye : MonoBehaviour, IDamagable
{
    public enum WhichEye
    {
        LeftEye,
        RightEye,
    }

    [SerializeField] private Boss boss;
    [SerializeField] private BoxCollider2D eyeCollider;
    [SerializeField] private Animator anim;
    
    public WhichEye whichEye;
    public bool isEyeOpen;

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
        InformBoss(isEyeOpen);
        anim.SetBool("isEyeOpen", true);
    }

    public void SetEyeClose()
    {
        if (!eyeCollider.enabled)
            return;

        eyeCollider.enabled = false;
        isEyeOpen = false;
        InformBoss(isEyeOpen);
        anim.SetBool("isEyeOpen", false);
    }

    private void InformBoss(bool isEyeOpen)
    {
        switch (whichEye)
        {
            case WhichEye.LeftEye:
                boss.IsLeftEyeOpen(isEyeOpen);
                break;
            case WhichEye.RightEye:
                boss.IsRightEyeOpen(isEyeOpen);
                break;
        }
    }
}