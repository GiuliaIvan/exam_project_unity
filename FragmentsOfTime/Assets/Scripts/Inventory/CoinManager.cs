using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    public int currentCoins = 0;
    public TMP_Text coinText; // Assign in Inspector

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist across scenes
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
       // ResetCoins(); //remove after testing
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
        SaveCoins();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = " " + currentCoins;
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

    public void ResetCoins() // remove after testing
    {
        currentCoins = 0;
        UpdateCoinUI();
    }

}
