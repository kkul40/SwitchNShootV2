using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;

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

    public void PlayerTurnOn()
    {
        animator.SetBool("isTurnOn", true);
    }

    public void PlayerTurnOff()
    {
        animator.SetBool("isTurnOn", false);
    }
}