using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    public int currentCoins = 0;       // Total coins saved between sessions
    private int levelCoins = 0;        // Coins collected during the current level

    public TMP_Text coinLevelText;     // Shows coins collected during level
    public TMP_Text coinTotalText;     // Shows total coins collected

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persists across scenes
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

    // Call this at the beginning of a level
    public void StartLevel()
    {
        levelCoins = 0;
        UpdateCoinUI();
    }

    // Call this during gameplay to add coins
    public void AddLevelCoins(int amount)
    {
        levelCoins += amount;
        UpdateCoinUI();
    }

    // Call this if the player completes the level
    public void OnLevelComplete()
    {
        currentCoins += levelCoins;
        SaveCoins();
        levelCoins = 0;
        UpdateCoinUI();
    }

    // Call this if the player dies or fails the level
    public void OnPlayerDeath()
    {
        levelCoins = 0;
        UpdateCoinUI();
    }

    // Updates both level and total coin UI texts
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

    // Save total coins to PlayerPrefs
    private void SaveCoins()
    {
        PlayerPrefs.SetInt("Coins", currentCoins);
    }

    // Load total coins from PlayerPrefs
    private void LoadCoins()
    {
        currentCoins = PlayerPrefs.GetInt("Coins", 0);
    }

    // Optional: Reset saved coins (for development/testing)
    //public void ResetCoins()
    //{
    //    currentCoins = 0;
    //    levelCoins = 0;
//        SaveCoins();
     //   UpdateCoinUI();
    //}
}
