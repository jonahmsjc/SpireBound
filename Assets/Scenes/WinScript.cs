using UnityEngine;

public class WinScript : MonoBehaviour
{
    public int enemiesToKill = 5;        // Set in Inspector
    public GameObject winUI;            // Assign your Win UI panel in Inspector

    private int currentKills = 0;
    private bool hasWon = false;

    void Start()
    {
        if (winUI != null)
            winUI.SetActive(false);     // Hide on start
    }

    public void EnemyDied()
    {
        if (hasWon) return;

        currentKills++;
        Debug.Log("Enemy died. Total kills: " + currentKills);

        if (currentKills >= enemiesToKill)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        hasWon = true;
        Debug.Log("YOU WIN!");
        winUI.SetActive(true);          // Show the Win UI
        Time.timeScale = 0f;            // Optional: pause the game
    }
}
