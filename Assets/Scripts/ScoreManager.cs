using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("UI Elemanları")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI totalMoneyText;

    private int currentScore = 0;
    private int highScore = 0;
    private int totalMoney = 0;

    void Awake()
    {
        instance = this;
        LoadData();
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateUI();
    }

    public void FinalizeRun()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        totalMoney += currentScore;
        PlayerPrefs.SetInt("TotalMoney", totalMoney);

        PlayerPrefs.Save(); 
        UpdateUI();
    }

    void LoadData()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "SCORE: " + currentScore;
        if (highScoreText != null) highScoreText.text = "BEST: " + highScore;
        if (totalMoneyText != null) totalMoneyText.text = "CASH: $" + totalMoney;
    }
}