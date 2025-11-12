using UnityEngine;

public class FXSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;

    void OnEnable()
    {
        PlayerMove.OnPlayerAttack += PlayAttackSound;
    }

    void OnDisable()
    {
        PlayerMove.OnPlayerAttack -= PlayAttackSound;
    }

    private void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
            audioSource.PlayOneShot(attackSound);
        else
            Debug.LogWarning("FXSounds: Missing AudioSource or AttackSound reference!");
    }
}