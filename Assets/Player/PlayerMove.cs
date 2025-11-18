using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileOffset = 0.5f;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    private Vector2 dashDirection;
    private float cooldownTimer;
    private bool isDashing = false;

    // 🔥 INVINCIBILITY FLAG
    public bool isInvincible = false;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isAttacking = false;

    public static event Action OnPlayerAttack;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (rb == null)
            Debug.LogError("PlayerMove: No Rigidbody2D found on player!");
        if (animator == null)
            Debug.LogWarning("PlayerMove: No Animator found on player (dash animation won't play).");
        if (spriteRenderer == null)
            Debug.LogWarning("PlayerMove: No SpriteRenderer found (facing won't flip).");
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (!isAttacking && Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.aKey.isPressed)
            {
                input.x -= 1;
                if (spriteRenderer) spriteRenderer.flipX = true;
            }
            if (Keyboard.current.dKey.isPressed)
            {
                input.x += 1;
                if (spriteRenderer) spriteRenderer.flipX = false;
            }
        }

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && !isAttacking)
        {
            if (animator) animator.SetTrigger("Attack");
            FireProjectile();
            OnPlayerAttack?.Invoke();
        }

        movementInput = input.normalized;
        if (animator) animator.SetBool("isWalking", movementInput != Vector2.zero);

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (!isDashing && cooldownTimer <= 0f)
        {
            bool pressedShift = (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame)
                                || Input.GetKeyDown(KeyCode.LeftShift);

            if (pressedShift)
            {
                Vector2 moveDir = movementInput;
                if (moveDir == Vector2.zero)
                {
                    moveDir = (spriteRenderer != null && spriteRenderer.flipX) ? Vector2.left : Vector2.right;
                }

                StartCoroutine(DashRoutine(moveDir.normalized));
            }
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
            return;

        if (!isAttacking)
            rb.linearVelocity = movementInput * moveSpeed;
        else
            rb.linearVelocity = Vector2.zero;
    }

    private void FireProjectile()
    {
        Vector2 direction = (spriteRenderer != null && spriteRenderer.flipX) ? Vector2.left : Vector2.right;
        Vector3 spawnPos = transform.position + (Vector3)(direction * projectileOffset);

        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        var projComp = projectile.GetComponent<Projectile>();
        if (projComp != null)
            projComp.SetDirection(direction);
    }

    private IEnumerator DashRoutine(Vector2 direction)
    {
        if (isDashing)
            yield break;

        if (rb.bodyType != RigidbodyType2D.Dynamic)
        {
            Debug.LogWarning($"PlayerMove: Rigidbody2D bodyType is {rb.bodyType}. Velocity-based dash works with Dynamic bodyType.");
        }

        isDashing = true;
        cooldownTimer = dashCooldown;
        dashDirection = direction;

        Debug.Log($"[PlayerMove] Dash started. Direction: {dashDirection} | Speed: {dashSpeed} | Duration: {dashDuration}");

        if (animator != null)
            animator.SetTrigger("Slide");

        // 🔥 INVINCIBILITY ADDED HERE
        isInvincible = true;
        Debug.Log("PLAYER IS NOW INVINCIBLE");

        rb.linearVelocity = dashDirection * dashSpeed;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        // 🔥 END INVINCIBILITY
        isInvincible = false;
        Debug.Log("PLAYER IS NO LONGER INVINCIBLE");

        Debug.Log("[PlayerMove] Dash ended.");
        yield break;
    }
}
