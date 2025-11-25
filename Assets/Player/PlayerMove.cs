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
    [SerializeField] private float baseFireCooldown = 0.5f;
    private float fireCooldownTimer = 0f;

    private Vector2 lastAimDirection = Vector2.right;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    private Vector2 dashDirection;
    private float cooldownTimer;
    private bool isDashing = false;

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

    void Start()
    {
        ApplySpeedStat();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (fireCooldownTimer > 0f)
            fireCooldownTimer -= Time.deltaTime;

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

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && !isAttacking && fireCooldownTimer <= 0f)
        {
            Vector2 aimDir = input;

            if (aimDir == Vector2.zero)
            {
                aimDir = (spriteRenderer != null && spriteRenderer.flipX)
                    ? Vector2.left
                    : Vector2.right;
            }

            lastAimDirection = aimDir;

            if (animator) animator.SetTrigger("Attack");
            FireProjectile(aimDir);
            OnPlayerAttack?.Invoke();

            float currentCooldown = GetCurrentFireCooldown();
            fireCooldownTimer = currentCooldown;
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
                    moveDir = (spriteRenderer != null && spriteRenderer.flipX)
                        ? Vector2.left
                        : Vector2.right;
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

    private void ApplySpeedStat()
    {
        if (CurrencyManagerScript.Instance == null)
            return;

        moveSpeed *= GetSpeedMultiplier();
    }

    private float GetSpeedMultiplier()
    {
        float spd = CurrencyManagerScript.Instance != null ? CurrencyManagerScript.Instance.Spd : 0f;
        return 1f + (spd * 0.1f);
    }

    private float GetCurrentFireCooldown()
    {
        if (CurrencyManagerScript.Instance == null)
            return baseFireCooldown;

        float atk = CurrencyManagerScript.Instance.Atk;
        float t = Mathf.Clamp01(atk / 10f);
        return Mathf.Lerp(baseFireCooldown, 0f, t);
    }

    private void FireProjectile(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            direction = (spriteRenderer != null && spriteRenderer.flipX)
                        ? Vector2.left
                        : Vector2.right;
        }

        direction = direction.normalized;

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

        isInvincible = true;
        Debug.Log("PLAYER IS NOW INVINCIBLE");

        rb.linearVelocity = dashDirection * dashSpeed;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            float finalDashSpeed = dashSpeed * GetSpeedMultiplier();
            rb.linearVelocity = dashDirection * finalDashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDashing = false;

        isInvincible = false;
        Debug.Log("PLAYER IS NO LONGER INVINCIBLE");

        Debug.Log("[PlayerMove] Dash ended.");
        yield break;
    }
}

