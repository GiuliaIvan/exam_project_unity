using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int currentCoins = 0; // Total coins saved between sessions
    private int levelCoins = 0;  // Coins collected during the current level

    public TMP_Text coinLevelText; // Shows coins collected during level
    public TMP_Text coinTotalText; // Shows total coins collected

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadCoins();
        UpdateCoinUI();
    }

    // Used by scene-specific script to assign UI elements
    public void SetUI(TMP_Text levelText, TMP_Text totalText)
    {
        coinLevelText = levelText;
        coinTotalText = totalText;
        UpdateCoinUI();
    }

    public void StartLevel()
    {
        levelCoins = 0;
        UpdateCoinUI();
    }

    public void AddLevelCoins(int amount)
    {
        levelCoins += amount;
        UpdateCoinUI();
    }

    public void OnLevelComplete()
    {
        currentCoins += levelCoins;
        SaveCoins();
        UpdateCoinUI();
    }

    public void OnPlayerDeath()
    {
        levelCoins = 0;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinLevelText != null)
        {
            coinLevelText.text = levelCoins.ToString();
        }

        if (coinTotalText != null)
        {
            coinTotalText.text = currentCoins.ToString();
        }
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", currentCoins);
    }

    private void LoadCoins()
    {
        currentCoins = PlayerPrefs.GetInt("Coins", 0);
    }

}
