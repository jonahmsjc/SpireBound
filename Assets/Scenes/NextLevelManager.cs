using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelManager : MonoBehaviour
{
    public void LoadNextLevel()
    {
        
        SceneManager.LoadScene("ShopScene");
    }
}