using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
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

            // Attack input
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                animator.SetTrigger("Attack");
            }
        }

        movementInput = input.normalized;

        // Update walking animation
        bool isWalking = movementInput.x != 0 || movementInput.y != 0;
        animator.SetBool("isWalking", isWalking);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }
}
