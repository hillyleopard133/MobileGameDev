using BayatGames.SaveGameFree;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Upgrades[] fruitUpgrades;
    [SerializeField] private TextMeshProUGUI[] fruitAmountTexts;
    
    [SerializeField] private TextMeshProUGUI[] upgradeCostTexts;
    [SerializeField] private TextMeshProUGUI[] upgradeLevelTexts;

    private const int treeQuantity = 0;
    private const int fruitQuantity = 1;
    private const int fruitQuality = 2;

    private const int apple = 0;
    private const int banana = 1;

    private const float costMultiplier = 1.5f;

    private ResourceManager resourceManager;
    
    private readonly string UPGRADES = "UPGRADES";
    
    public enum FruitTypes
    {
        Apple,
        Banana
    }

    private FruitTypes selectedFruit;
    
    private void Start()
    {
        selectedFruit = FruitTypes.Apple;
        resourceManager = ResourceManager.Instance;
        UpdateUI();
    }

    private void Update()
    {
        
    }

    private int GetUpgradeCost(int level, int baseCost)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level));
    }

    public void ClickTree()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                Upgrades appleUpgrades = fruitUpgrades[apple];
                resourceManager.AppleAmount += appleUpgrades.FruitQuantity * appleUpgrades.TreeQuantity;
                fruitAmountTexts[apple].text = resourceManager.AppleAmount.ToString();
                break;
            case FruitTypes.Banana:
                Upgrades bananaUpgrades = fruitUpgrades[banana];
                resourceManager.BananaAmount += bananaUpgrades.FruitQuantity * bananaUpgrades.TreeQuantity;
                fruitAmountTexts[banana].text = resourceManager.BananaAmount.ToString();
                break;
        }
    }

    private void UpdateUI()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                UpdateUpgradeUI(apple);
                break;
            case FruitTypes.Banana:
                UpdateUpgradeUI(banana);
                break;
        }
        
        fruitAmountTexts[apple].text = resourceManager.AppleAmount.ToString();
        fruitAmountTexts[banana].text = resourceManager.BananaAmount.ToString();
    }

    private void UpdateUpgradeUI(int fruitIndex)
    {
        Upgrades upgrade = fruitUpgrades[fruitIndex];
        
        upgradeLevelTexts[treeQuantity].text = upgrade.TreeQuantity.ToString();
        upgradeLevelTexts[fruitQuantity].text = upgrade.FruitQuantity.ToString();
        upgradeLevelTexts[fruitQuality].text = upgrade.FruitQuality.ToString();
        
        upgradeCostTexts[treeQuantity].text = GetUpgradeCost(upgrade.TreeQuantity, upgrade.BaseValue).ToString();
        upgradeCostTexts[fruitQuantity].text = GetUpgradeCost(upgrade.FruitQuantity, upgrade.BaseValue).ToString();
        upgradeCostTexts[fruitQuality].text = GetUpgradeCost(upgrade.FruitQuality, upgrade.BaseValue).ToString();
    }

    public void UpgradeTreeQuantity()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                fruitUpgrades[apple].TreeQuantity++;
                break;
            case FruitTypes.Banana:
                fruitUpgrades[banana].TreeQuantity++;
                break;
        }
        
        UpdateUI();
        SaveLoadManager.Instance.SaveGameData();
    }
    
    public void UpgradeFruitQuantity()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                fruitUpgrades[apple].FruitQuantity++;
                break;
            case FruitTypes.Banana:
                fruitUpgrades[banana].FruitQuantity++;
                break;
        }
        
        UpdateUI();
        SaveLoadManager.Instance.SaveGameData();
    }
    
    public void UpgradeFruitQuality()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                fruitUpgrades[apple].FruitQuality++;
                break;
            case FruitTypes.Banana:
                fruitUpgrades[banana].FruitQuality++;
                break;
        }
        
        UpdateUI();
        SaveLoadManager.Instance.SaveGameData();
    }

    public void SetFruit(int fruitIndex)
    {
        selectedFruit = (FruitTypes)fruitIndex;
        UpdateUI();
    }

    public FruitTypes GetFruitType()
    {
        return selectedFruit;
    }

    
    // Save and Load
    
    public void SaveUpgradeData()
    {
        UpgradeData upgradeData = new UpgradeData();
        
        int length = fruitUpgrades.Length;
        upgradeData.TreeQuantityLevel = new int[length];
        upgradeData.FruitQuantityLevel = new int[length];
        upgradeData.FruitQualityLevel = new int[length];
        
        for (int i = 0; i < length; i++)
        {
            upgradeData.TreeQuantityLevel[i] = fruitUpgrades[i].TreeQuantity;
            upgradeData.FruitQuantityLevel[i] = fruitUpgrades[i].FruitQuantity;
            upgradeData.FruitQualityLevel[i] = fruitUpgrades[i].FruitQuality;
        }
        
        SaveGame.Save(UPGRADES, upgradeData);
    }

    public void LoadUpgradeData()
    {
        if (SaveGame.Exists(UPGRADES))
        {
            UpgradeData upgradeData = SaveGame.Load<UpgradeData>(UPGRADES);
            
            for (int i = 0; i < fruitUpgrades.Length; i++)
            {
                fruitUpgrades[i].TreeQuantity = upgradeData.TreeQuantityLevel[i];
                fruitUpgrades[i].FruitQuantity = upgradeData.FruitQuantityLevel[i];
                fruitUpgrades[i].FruitQuality = upgradeData.FruitQualityLevel[i];
            }
        }
        
        UpdateUI();
    }

    public void ResetUpgradeData()
    {
        for (int i = 0; i < fruitUpgrades.Length; i++)
        {
            fruitUpgrades[i].TreeQuantity = 1;
            fruitUpgrades[i].FruitQuantity = 1;
            fruitUpgrades[i].FruitQuality = 0;
        }
        
        SaveUpgradeData();
        UpdateUI();
    }
}
