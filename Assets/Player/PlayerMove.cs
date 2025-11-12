using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint; // Where projectile spawns
    [SerializeField] private float projectileOffset = 0.5f; // how far in front to spawn

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (!isAttacking && Keyboard.current != null)
        {
            // Movement input
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.aKey.isPressed)
            {
                input.x -= 1;
                spriteRenderer.flipX = true;
            }
            if (Keyboard.current.dKey.isPressed)
            {
                input.x += 1;
                spriteRenderer.flipX = false;
            }
        }

        // Attack input
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && !isAttacking)
        {
            animator.SetTrigger("Attack");
            FireProjectile();
        }

        movementInput = input.normalized;
        animator.SetBool("isWalking", movementInput != Vector2.zero);
    }

    void FixedUpdate()
    {
        if (!isAttacking)
            rb.linearVelocity = movementInput * moveSpeed;
        else
            rb.linearVelocity = Vector2.zero;
    }

    private void FireProjectile()
    {
        // Get direction based on facing
        Vector2 direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;

        // Calculate spawn position slightly in front of the player
        Vector3 spawnPos = transform.position + (Vector3)(direction * projectileOffset);

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // Set direction for projectile
        projectile.GetComponent<Projectile>().SetDirection(direction);
    }
}
