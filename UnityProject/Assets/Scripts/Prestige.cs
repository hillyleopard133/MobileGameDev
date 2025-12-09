using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.Serialization;

public class Prestige : Singleton<Prestige>
{
    [HideInInspector] public int prestigePoints;
    
    [HideInInspector] public int upgradeDiscount;
    [HideInInspector] public int blenderSpeed;
    [HideInInspector] public int maxOfflineTime;

    [SerializeField] public int upgradeDiscountBaseCost;
    [SerializeField] public int blenderSpeedBaseCost;
    [SerializeField] public int maxOfflineTimeBaseCost;

    private const int upgradeAmount = 3;
    
    private const string PRESTIGE_POINTS = "PRESTIGE_POINTS";
    private const string UPGRADE_LEVELS = "UPGRADE_LEVELS";
    
    private float costMultiplier;

    private const float BASE_MAX_OFFLINE_TIME = 2f;
    private const float OFFLINE_TIME_INCREASE_PER_LEVEL = 0.5f;

    private void Start()
    {
        costMultiplier = GameManager.costMultiplier;
    }

    public void GainPrestige()
    {
        prestigePoints += CalculatePrestigePoints();
        SaveLoadManager.Instance.PrestigeSafeReset();
        SavePrestige();
    }
    
    public int CalculatePrestigePoints()
    {
        return GameManager.Instance.NumberOfFruitUpgrades();
    }

    public void UpgradeDiscount()
    {
        if (UsePrestige(GetUpgradeCost(upgradeDiscount, upgradeDiscountBaseCost)))
        {
            upgradeDiscount++;
            SavePrestige();
        }
    }

    public void SetMaxTimeAway()
    {
        ResourceManager.Instance.SetMaxTimeAway(BASE_MAX_OFFLINE_TIME + (maxOfflineTime * OFFLINE_TIME_INCREASE_PER_LEVEL));
    }

    public void UpgradeBlenderSpeeds()
    {
        if (UsePrestige(GetUpgradeCost(blenderSpeed, blenderSpeedBaseCost)))
        {
             blenderSpeed++;
             SavePrestige();
        }
    }

    public void UpgradeMaxOfflineTime()
    {
        if (UsePrestige(GetUpgradeCost(maxOfflineTime, maxOfflineTimeBaseCost)))
        {
            maxOfflineTime++;
            SavePrestige();
        }
    }

    public int GetUpgradeCost(int level, int baseCost)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level));
    }
    
    
    private bool UsePrestige(int points)
    {
        if (points <= prestigePoints)
        {
            prestigePoints -= points;
            return true;
        }
        return false;
    }

    public void SavePrestige()
    {
        SaveGame.Save(PRESTIGE_POINTS, prestigePoints);
        
        int[] levels = new int[upgradeAmount];
        levels[0] = upgradeDiscount;
        levels[1] = blenderSpeed;
        levels[2] = maxOfflineTime;
        SaveGame.Save(UPGRADE_LEVELS, levels);
        
        UIManager.Instance.UpdatePrestigeUI();
    }

    public void LoadPrestige()
    {
        if (SaveGame.Exists(PRESTIGE_POINTS))
        {
            prestigePoints = SaveGame.Load<int>(PRESTIGE_POINTS);
        }

        if (SaveGame.Exists(UPGRADE_LEVELS))
        {
            int[] levels = SaveGame.Load<int[]>(UPGRADE_LEVELS);
            upgradeDiscount = levels[0];
            blenderSpeed = levels[1];
            maxOfflineTime = levels[2];
        }
        
        UIManager.Instance.UpdatePrestigeUI();
    }

    public void ResetPrestige()
    {
        prestigePoints = 0;
        upgradeDiscount = 0;
        blenderSpeed = 0;
        maxOfflineTime = 0;
        SavePrestige();
    }
}
