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

    public const float costMultiplier = 1.5f;

    private ResourceManager resourceManager;
    
    private const string UPGRADES = "UPGRADES";
    private const string LOCKS = "LOCKS";

    private float[] harvestTimers;
    private const float baseHarvesterTime = 3.5f;
    
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
        HarvesterQuantity,
        HarvesterSpeed
    }

    public FruitTypes selectedFruit;
    
    private void Start()
    {
        selectedFruit = FruitTypes.Apple;
        resourceManager = ResourceManager.Instance;
        harvestTimers = new float[fruitUpgrades.Length];
    }

    private void Update()
    {
        HarvestFruit();
    }

    public int NumberOfFruitUpgrades()
    {
        int numberOfUpgrades = 0;

        foreach (Upgrades upgrade in fruitUpgrades)
        {
            numberOfUpgrades += upgrade.NumberOfUpgrades();
        }
        
        return numberOfUpgrades;
    }

    public bool HasHarvester()
    {
        foreach (Upgrades upgrade in fruitUpgrades)
        {
            if(upgrade.HarvesterQuantity > 0) return true;
        }
        
        return false;
    }

    public void OfflineHarvesters(long seconds)
    {
        int[] fruitAmounts = new int[fruitUpgrades.Length];
        int startingCoins = resourceManager.CoinAmount;
        
        for (int i = 0; i < fruitUpgrades.Length; i++)
        {
            if (fruitUpgrades[i].HarvesterQuantity == 0) continue;

            int harvestAmount = (int)(seconds/baseHarvesterTime);
            fruitAmounts[i] = CalculateHarvestAmount(i) * (harvestAmount/2);
            
            switch (i)
            {
                case 0:
                    resourceManager.AppleAmount += fruitAmounts[i];
                    break;
                case 1:
                    resourceManager.BananaAmount += fruitAmounts[i];
                    break;
                case 2:
                    resourceManager.OrangeAmount += fruitAmounts[i];
                    break;
                case 3:
                    resourceManager.PearAmount += fruitAmounts[i];
                    break;
                case 4:
                    resourceManager.LemonAmount += fruitAmounts[i];
                    break;
            }
            
            resourceManager.AddCoins((fruitUpgrades[i].BaseValue * (fruitUpgrades[i].FruitQuality + 1) * (harvestAmount/2))/2);

            UIManager.Instance.OfflineGenUI(resourceManager.CoinAmount - startingCoins, fruitAmounts);
        }
    }

    public int GetFruitValue(FruitTypes fruit)
    {
        int index = (int)fruit;
        return fruitUpgrades[index].BaseValue * (fruitUpgrades[index].FruitQuality + 1);
    }

    private void HarvestFruit()
    {
        for (int i = 0; i < fruitUpgrades.Length; i++)
        {
            if (fruitUpgrades[i].HarvesterQuantity == 0) continue;
            
            harvestTimers[i] -= Time.deltaTime;
            if (harvestTimers[i] <= 0)
            {
                harvestTimers[i] = CalculateHarvestTimer(i);
                
                switch (i)
                {
                    case 0:
                        resourceManager.AppleAmount += CalculateHarvestAmount(i);
                        break;
                    case 1:
                        resourceManager.BananaAmount += CalculateHarvestAmount(i);
                        break;
                    case 2:
                        resourceManager.OrangeAmount += CalculateHarvestAmount(i);
                        break;
                    case 3:
                        resourceManager.PearAmount += CalculateHarvestAmount(i);
                        break;
                    case 4:
                        resourceManager.LemonAmount += CalculateHarvestAmount(i);
                        break;
                }
                
                UIManager.Instance.UpdateFruitUI();
            }
            
        }
    }

    public void UnlockCurrentFruit()
    {
        if (resourceManager.CoinAmount >= GetCurrentUpgrade().unlockCost)
        {
            resourceManager.UseCoins(GetCurrentUpgrade().unlockCost);
            GetCurrentUpgrade().isLocked = false;
            SaveLocks();
            UIManager.Instance.HideLockScreen();
        }
    }

    public Upgrades GetCurrentUpgrade()
    {
        return fruitUpgrades[(int)selectedFruit];
    }

    private float CalculateHarvestTimer(int fruitIndex)
    {
        return baseHarvesterTime - (fruitUpgrades[fruitIndex].HarvesterSpeed * 0.1f);
    }

    private int CalculateHarvestAmount(int fruitIndex)
    {
        return fruitUpgrades[fruitIndex].HarvesterQuantity * (fruitUpgrades[fruitIndex].TreeQuantity + 1);
    }

    public Upgrades getFruitUpgrade(int fruitIndex)
    {
        return fruitUpgrades[fruitIndex];
    }

    private int GetUpgradeCost(int level, int baseCost)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level)* (1 - Prestige.Instance.upgradeDiscount * 0.02f));
    }

    public void ClickTree()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                Upgrades appleUpgrades = fruitUpgrades[apple];
                resourceManager.AppleAmount += (appleUpgrades.FruitQuantity + 1) * (appleUpgrades.TreeQuantity + 1);
                break;
            case FruitTypes.Banana:
                Upgrades bananaUpgrades = fruitUpgrades[banana];
                resourceManager.BananaAmount += (bananaUpgrades.FruitQuantity + 1) * (bananaUpgrades.TreeQuantity + 1);
                break;
            case FruitTypes.Orange:
                Upgrades orangeUpgrades = fruitUpgrades[orange];
                resourceManager.OrangeAmount += (orangeUpgrades.FruitQuantity + 1) * (orangeUpgrades.TreeQuantity + 1);
                break;
            case FruitTypes.Pear:
                Upgrades pearUpgrades = fruitUpgrades[pear];
                resourceManager.PearAmount += (pearUpgrades.FruitQuantity + 1) * (pearUpgrades.TreeQuantity + 1);
                break;
            case FruitTypes.Lemon:
                Upgrades lemonUpgrades = fruitUpgrades[lemon];
                resourceManager.LemonAmount += (lemonUpgrades.FruitQuantity + 1) * (lemonUpgrades.TreeQuantity + 1);
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
                upgradeCost = GetUpgradeCost(fruitUpgrades[fruitIndex].TreeQuantity, fruitUpgrades[fruitIndex].BaseTreeQuantityCost);
                if (upgradeCost <= resourceManager.CoinAmount)
                {
                    fruitUpgrades[fruitIndex].TreeQuantity++;
                    resourceManager.UseCoins(upgradeCost);
                    Debug.Log("Upgrade purchased: Tree Quantity");
                }
                break;
            
            case UpgradeTypes.FruitQuantity:
                upgradeCost = GetUpgradeCost(fruitUpgrades[fruitIndex].FruitQuantity, fruitUpgrades[fruitIndex].BaseFruitQuantityCost);
                if (upgradeCost <= resourceManager.CoinAmount)
                {
                    fruitUpgrades[fruitIndex].FruitQuantity++;
                    resourceManager.UseCoins(upgradeCost);
                    Debug.Log("Upgrade purchased: Fruit Quantity");
                }
                break;
            case UpgradeTypes.FruitQuality:
                upgradeCost = GetUpgradeCost(fruitUpgrades[fruitIndex].FruitQuality, fruitUpgrades[fruitIndex].BaseFruitQualityCost);
                if (upgradeCost <= resourceManager.CoinAmount)
                {
                    fruitUpgrades[fruitIndex].FruitQuality++;
                    resourceManager.UseCoins(upgradeCost);
                    Debug.Log("Upgrade purchased: Fruit Quality");
                }
                break;
            case UpgradeTypes.HarvesterQuantity:
                upgradeCost = GetUpgradeCost(fruitUpgrades[fruitIndex].HarvesterQuantity, fruitUpgrades[fruitIndex].BaseHarvesterQuantityCost);
                if (upgradeCost <= resourceManager.CoinAmount)
                {
                    fruitUpgrades[fruitIndex].HarvesterQuantity++;
                    resourceManager.UseCoins(upgradeCost);
                    Debug.Log("Upgrade purchased: Harvester Quantity");
                }
                break;
            case UpgradeTypes.HarvesterSpeed:
                upgradeCost = GetUpgradeCost(fruitUpgrades[fruitIndex].HarvesterSpeed, fruitUpgrades[fruitIndex].BaseHarvesterSpeedCost);
                if (upgradeCost <= resourceManager.CoinAmount)
                {
                    fruitUpgrades[fruitIndex].HarvesterSpeed++;
                    resourceManager.UseCoins(upgradeCost);
                    Debug.Log("Upgrade purchased: Harvester Speed");
                }
                break;
        }
        
    }
    
    public void UpgradeHarvesterQuantity()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                Upgrade(apple, UpgradeTypes.HarvesterQuantity);
                break;
            case FruitTypes.Banana:                
                Upgrade(banana, UpgradeTypes.HarvesterQuantity);
                break;
            case FruitTypes.Orange:
                Upgrade(orange, UpgradeTypes.HarvesterQuantity);
                break;
            case FruitTypes.Pear:
                Upgrade(pear, UpgradeTypes.HarvesterQuantity);
                break;
            case FruitTypes.Lemon:
                Upgrade(lemon, UpgradeTypes.HarvesterQuantity);
                break;
        }
        
        UIManager.Instance.UpdateFruitUI();
        SaveLoadManager.Instance.SaveGameData();
    }
    
    public void UpgradeHarvesterSpeed()
    {
        switch (selectedFruit)
        {
            case FruitTypes.Apple:
                Upgrade(apple, UpgradeTypes.HarvesterSpeed);
                break;
            case FruitTypes.Banana:                
                Upgrade(banana, UpgradeTypes.HarvesterSpeed);
                break;
            case FruitTypes.Orange:
                Upgrade(orange, UpgradeTypes.HarvesterSpeed);
                break;
            case FruitTypes.Pear:
                Upgrade(pear, UpgradeTypes.HarvesterSpeed);
                break;
            case FruitTypes.Lemon:
                Upgrade(lemon, UpgradeTypes.HarvesterSpeed);
                break;
        }
        
        UIManager.Instance.UpdateFruitUI();
        SaveLoadManager.Instance.SaveGameData();
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
        
        RecipeManager.Instance.LoadRecipes();
        RecipeManager.Instance.LoadSelectedRecipe();
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

    public void SaveLocks()
    {
        bool[] locked = new bool[fruitUpgrades.Length];
        for (int i = 0; i < fruitUpgrades.Length; i++)
        {
            locked[i] = fruitUpgrades[i].isLocked;
        }
        
        SaveGame.Save(LOCKS, locked);
    }

    public void LoadLocks()
    {
        if (SaveGame.Exists(LOCKS))
        {
            bool[] locked = SaveGame.Load<bool[]>(LOCKS);

            for (int i = 0; i < locked.Length; i++)
            {
                fruitUpgrades[i].isLocked = locked[i];
            }
        }
    }

    public void ResetLocks()
    {
        foreach (Upgrades upgrade in fruitUpgrades)
        {
            upgrade.isLocked = true;
        }
        
        fruitUpgrades[0].isLocked = false;
    }
    
    public void SaveUpgradeData()
    {
        UpgradeData upgradeData = new UpgradeData();
        
        int length = fruitUpgrades.Length;
        upgradeData.TreeQuantityLevel = new int[length];
        upgradeData.FruitQuantityLevel = new int[length];
        upgradeData.FruitQualityLevel = new int[length];
        upgradeData.HarvestQuantityLevel = new int[length];
        upgradeData.HarvestSpeedLevel = new int[length];
        
        for (int i = 0; i < length; i++)
        {
            upgradeData.TreeQuantityLevel[i] = fruitUpgrades[i].TreeQuantity;
            upgradeData.FruitQuantityLevel[i] = fruitUpgrades[i].FruitQuantity;
            upgradeData.FruitQualityLevel[i] = fruitUpgrades[i].FruitQuality;
            upgradeData.HarvestQuantityLevel[i] = fruitUpgrades[i].HarvesterQuantity;
            upgradeData.HarvestSpeedLevel[i] = fruitUpgrades[i].HarvesterSpeed;
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
                fruitUpgrades[i].HarvesterQuantity = upgradeData.HarvestQuantityLevel[i];
                fruitUpgrades[i].HarvesterSpeed = upgradeData.HarvestSpeedLevel[i];
            }
        }
        
        UIManager.Instance.UpdateFruitUI();
    }

    public void ResetUpgradeData()
    {
        for (int i = 0; i < fruitUpgrades.Length; i++)
        {
            fruitUpgrades[i].TreeQuantity = 0;
            fruitUpgrades[i].FruitQuantity = 0;
            fruitUpgrades[i].FruitQuality = 0;
            fruitUpgrades[i].HarvesterQuantity = 0;
            fruitUpgrades[i].HarvesterSpeed = 0;
        }
        
        SaveUpgradeData();
        UIManager.Instance.UpdateFruitUI();
    }
}
