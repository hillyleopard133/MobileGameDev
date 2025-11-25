using BayatGames.SaveGameFree;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Upgrades[] fruitUpgrades;

    private const int apple = 0;
    private const int banana = 1;
    private const int orange = 2;
    private const int pear = 3;
    private const int lemon = 4;

    private const float costMultiplier = 1.5f;

    private ResourceManager resourceManager;
    
    private readonly string UPGRADES = "UPGRADES";
    
    public enum FruitTypes
    {
        Apple,
        Banana,
        Orange,
        Pear,
        Lemon
    }

    public enum UpgradeTypes
    {
        TreeQuantity,
        FruitQuantity,
        FruitQuality,
    }

    public FruitTypes selectedFruit;
    
    private void Start()
    {
        selectedFruit = FruitTypes.Apple;
        resourceManager = ResourceManager.Instance;
    }

    private void Update()
    {
        
    }

    public Upgrades getFruitUpgrade(int fruitIndex)
    {
        return fruitUpgrades[fruitIndex];
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
                break;
            case FruitTypes.Banana:
                Upgrades bananaUpgrades = fruitUpgrades[banana];
                resourceManager.BananaAmount += bananaUpgrades.FruitQuantity * bananaUpgrades.TreeQuantity;
                break;
            case FruitTypes.Orange:
                Upgrades orangeUpgrades = fruitUpgrades[orange];
                resourceManager.OrangeAmount += orangeUpgrades.FruitQuantity * orangeUpgrades.TreeQuantity;
                break;
            case FruitTypes.Pear:
                Upgrades pearUpgrades = fruitUpgrades[pear];
                resourceManager.PearAmount += pearUpgrades.FruitQuantity * pearUpgrades.TreeQuantity;
                break;
            case FruitTypes.Lemon:
                Upgrades lemonUpgrades = fruitUpgrades[lemon];
                resourceManager.LemonAmount += lemonUpgrades.FruitQuantity * lemonUpgrades.TreeQuantity;
                break;
        }
        UIManager.Instance.UpdateFruitUI();
        Debug.Log("Earned Fruit");
    }

    

    private void Upgrade(int fruitIndex, UpgradeTypes upgradeType)
    {
        int upgradeCost;
        switch (upgradeType)
        {
            case UpgradeTypes.TreeQuantity:
                upgradeCost = GetUpgradeCost(fruitUpgrades[fruitIndex].TreeQuantity, fruitUpgrades[fruitIndex].BaseValue);
                if (upgradeCost <= resourceManager.CoinAmount)
                {
                    fruitUpgrades[fruitIndex].TreeQuantity++;
                    resourceManager.UseCoins(upgradeCost);
                    Debug.Log("Upgrade purchased");
                }
                break;
            
            case UpgradeTypes.FruitQuantity:
                upgradeCost = GetUpgradeCost(fruitUpgrades[fruitIndex].FruitQuantity, fruitUpgrades[fruitIndex].BaseValue);
                if (upgradeCost <= resourceManager.CoinAmount)
                {
                    fruitUpgrades[fruitIndex].FruitQuantity++;
                    resourceManager.UseCoins(upgradeCost);
                    Debug.Log("Upgrade purchased");
                }
                break;
            case UpgradeTypes.FruitQuality:
                upgradeCost = GetUpgradeCost(fruitUpgrades[fruitIndex].FruitQuality, fruitUpgrades[fruitIndex].BaseValue);
                if (upgradeCost <= resourceManager.CoinAmount)
                {
                    fruitUpgrades[fruitIndex].FruitQuality++;
                    resourceManager.UseCoins(upgradeCost);
                    Debug.Log("Upgrade purchased");
                }
                break;
        }
        
    }
    
    public void UpgradeTreeQuantity()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                Upgrade(apple, UpgradeTypes.TreeQuantity);
                break;
            case FruitTypes.Banana:                
                Upgrade(banana, UpgradeTypes.TreeQuantity);
                break;
            case FruitTypes.Orange:
                Upgrade(orange, UpgradeTypes.TreeQuantity);
                break;
            case FruitTypes.Pear:
                Upgrade(pear, UpgradeTypes.TreeQuantity);
                break;
            case FruitTypes.Lemon:
                Upgrade(lemon, UpgradeTypes.TreeQuantity);
                break;
        }
        
        UIManager.Instance.UpdateFruitUI();
        SaveLoadManager.Instance.SaveGameData();
    }
    
    public void UpgradeFruitQuantity()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                Upgrade(apple, UpgradeTypes.FruitQuantity);
                break;
            case FruitTypes.Banana:
                Upgrade(banana, UpgradeTypes.FruitQuantity);
                break;
            case FruitTypes.Orange:
                Upgrade(orange, UpgradeTypes.FruitQuantity);
                break;
            case FruitTypes.Pear:
                Upgrade(pear, UpgradeTypes.FruitQuantity);
                break;
            case FruitTypes.Lemon:
                Upgrade(lemon, UpgradeTypes.FruitQuantity);
                break;
        }
        
        UIManager.Instance.UpdateFruitUI();
        SaveLoadManager.Instance.SaveGameData();
    }
    
    public void UpgradeFruitQuality()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                Upgrade(apple, UpgradeTypes.FruitQuality);
                break;
            case FruitTypes.Banana:
                Upgrade(banana, UpgradeTypes.FruitQuality);
                break;
            case FruitTypes.Orange:
                Upgrade(orange, UpgradeTypes.FruitQuality);
                break;
            case FruitTypes.Pear:
                Upgrade(pear, UpgradeTypes.FruitQuality);
                break;
            case FruitTypes.Lemon:
                Upgrade(lemon, UpgradeTypes.FruitQuality);
                break;
        }
        
        UIManager.Instance.UpdateFruitUI();
        SaveLoadManager.Instance.SaveGameData();
    }

    public void SetFruit(int fruitIndex)
    {
        selectedFruit = (FruitTypes)fruitIndex;
        UIManager.Instance.UpdateFruitUI();
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
        
        UIManager.Instance.UpdateFruitUI();
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
        UIManager.Instance.UpdateFruitUI();
    }
}
