using UnityEngine;
using System; 

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    public static event Action OnUpgradesChanged;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject); 
    }

    public int GetUpgradeLevel(string key)
    {
        return PlayerPrefs.GetInt("Upgrade_" + key, 0);
    }

    public bool BuyUpgrade(string key, int baseCost, int costMultiplier)
    {
        int currentLevel = GetUpgradeLevel(key);
        int cost = baseCost + (currentLevel * costMultiplier);
        int totalMoney = PlayerPrefs.GetInt("TotalMoney", 0);

        if (totalMoney >= cost)
        {
            totalMoney -= cost;
            PlayerPrefs.SetInt("TotalMoney", totalMoney);
            PlayerPrefs.SetInt("Upgrade_" + key, currentLevel + 1);
            PlayerPrefs.Save();
            
            OnUpgradesChanged?.Invoke(); 

            Debug.Log(key + " geliştirildi! Yeni Seviye: " + (currentLevel + 1));
            return true;
        }

        Debug.Log("Yetersiz bakiye!");
        return false;
    }
}