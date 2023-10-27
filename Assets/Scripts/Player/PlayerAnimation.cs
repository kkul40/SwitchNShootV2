using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;
    
    [Header("Shield")]
    [SerializeField] private SpriteRenderer shieldSprite;
    [SerializeField] private Animator shieldAnimator;

    private bool isTurnOn;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void FlipRight()
    {
        sprite.flipX = false;
    }

    public void FlipLeft()
    {
        sprite.flipX = true;
    }

    public void FlipX(Vector3 direction)
    {
        if (direction == Vector3.right)
            FlipRight();
        else if (direction == Vector3.left) FlipLeft();
    }

    public void PlayerTurnOn()
    {
        animator.SetBool("isTurnOn", true);
    }

    public void PlayerTurnOff()
    {
        animator.SetBool("isTurnOn", false);
    }

    public void ToggleShieldAnimation(bool toggle)
    {
        
    }
}