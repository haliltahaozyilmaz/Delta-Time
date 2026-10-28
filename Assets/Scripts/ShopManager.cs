using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string upgradeKey;    
        public string displayName;   
        public int baseCost = 100;
        public int costMultiplier = 50;
        public int maxLevel = 5;

        [Header("UI Bağlantıları")]
        public TextMeshProUGUI infoText;
        public TextMeshProUGUI costText;
        public Button buyButton;
    }

    public ShopItem[] shopItems;
    public TextMeshProUGUI totalMoneyText;

    void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        int money = PlayerPrefs.GetInt("TotalMoney", 0);
        totalMoneyText.text = "Money: " + money;

        foreach (var item in shopItems)
        {
            int currentLevel = UpgradeManager.instance.GetUpgradeLevel(item.upgradeKey);

            item.infoText.text = (string.IsNullOrEmpty(item.displayName) ? item.upgradeKey : item.displayName) 
                                 + " (Lv." + currentLevel + "/" + item.maxLevel + ")";

            if (currentLevel >= item.maxLevel)
            {
                item.costText.text = "MAX LEVEL";
                item.buyButton.interactable = false;
            }
            else
            {
                int currentCost = item.baseCost + (currentLevel * item.costMultiplier);
                item.costText.text = currentCost + " $";
                
                item.buyButton.interactable = (money >= currentCost);
            }
        }
    }

    public void OnBuyButtonClicked(int index)
    {
        ShopItem item = shopItems[index];
        int currentLevel = UpgradeManager.instance.GetUpgradeLevel(item.upgradeKey);

        if (currentLevel < item.maxLevel)
        {
            bool isSuccess = UpgradeManager.instance.BuyUpgrade(item.upgradeKey, item.baseCost, item.costMultiplier);
            
            if (isSuccess)
            {
                UpdateUI();
            }
        }
    }
}