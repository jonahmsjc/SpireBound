using UnityEngine;

public class SantaAnimationController : MonoBehaviour
{
     public Animator animator;
    public float attackInterval = 1.5f;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        StartCoroutine(AttackRoutine());
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        while (true)
        {
            animator.SetBool("Attack", true);
            yield return new WaitForSeconds(0.1f); // keep it true long enough for transition
            animator.SetBool("Attack", false);

            yield return new WaitForSeconds(attackInterval);
        }
    }
}
