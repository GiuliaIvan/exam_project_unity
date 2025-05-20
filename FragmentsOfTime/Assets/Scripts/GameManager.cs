using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject gameOverScreen;

    private Hero currentHero;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartGame(Hero hero)
    {
        currentHero = hero;
        CoinManager.Instance.StartLevel();
        currentHero.ResetLives();
    }

    public void PlayerDied()
    {
        CoinManager.Instance.OnPlayerDeath();
        InventorySystem.Instance.OnLevelCompleted(false); // If player dies
        StartCoroutine(DelayedGameOverScreen(2f)); // 2-second delay

    }

    private IEnumerator DelayedGameOverScreen(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowGameOverScreen();
    }

    private void ShowGameOverScreen()
    {
        Time.timeScale = 0f;
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LevelComplete()
    {
        CoinManager.Instance.OnLevelComplete();
        InventorySystem.Instance.OnLevelCompleted(true); // If player wins

    }
}
