using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float slideSpeed = 10f;
    public float slideDuration = 0.3f;
    public float slideCooldown = 1f;

    [Header("Attack Settings")]
    public float attackCooldown = 0.5f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private InputAction moveAction;
    private InputAction attackAction;
    private InputAction slideAction;

    private Vector2 moveValue;
    private bool isSliding = false;
    private bool canSlide = true;
    private bool canAttack = true;

    private void OnEnable()
    {
        // Get actions from Input System
        moveAction = InputSystem.actions.FindAction("Move");
        attackAction = InputSystem.actions.FindAction("Attack");
        slideAction = InputSystem.actions.FindAction("Slide");

        attackAction.performed += ctx => TryAttack();
        slideAction.performed += ctx => TrySlide();
    }

    private void OnDisable()
    {
        attackAction.performed -= ctx => TryAttack();
        slideAction.performed -= ctx => TrySlide();
    }

    private void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        if (!isSliding)
            MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 moveDir = new Vector2(moveValue.x, 0);
        Vector2 newPosition = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        // Flip sprite when changing direction
        if (moveDir.x != 0)
            spriteRenderer.flipX = moveDir.x < 0;
    }

    private void HandleAnimations()
    {
        bool isMoving = Mathf.Abs(moveValue.x) > 0.1f;
        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isSliding", isSliding);
    }

    private void TryAttack()
    {
        if (!canAttack) return;
        StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        canAttack = false;
        animator.SetTrigger("Attack");
        // Optional: Add logic here to detect hits (e.g. enable hitbox)

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void TrySlide()
    {
        if (!canSlide || isSliding) return;
        StartCoroutine(SlideCoroutine());
    }

    private IEnumerator SlideCoroutine()
    {
        canSlide = false;
        isSliding = true;

        animator.SetTrigger("Slide");

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector2 slideDir = new Vector2(spriteRenderer.flipX ? -1 : 1, 0);
        rb.linearVelocity = slideDir * slideSpeed;

        yield return new WaitForSeconds(slideDuration);

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isSliding = false;

        yield return new WaitForSeconds(slideCooldown);
        canSlide = true;
    }
}
