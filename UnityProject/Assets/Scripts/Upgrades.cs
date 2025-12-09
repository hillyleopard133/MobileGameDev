using UnityEngine;

[CreateAssetMenu(fileName = "Upgrades", menuName = "Scriptable Objects/Upgrades")]
public class Upgrades : ScriptableObject
{
    [HideInInspector] public int TreeQuantity;    //Number of trees in orchard
    [HideInInspector] public int FruitQuantity;   //Number of fruits per tree per tap
    [HideInInspector] public int FruitQuality;    //Increase value of fruit products
    [HideInInspector] public int HarvesterQuantity; //Number of automatic workers
    [HideInInspector] public int HarvesterSpeed;  //Automation speed

    public bool isLocked;
    public int unlockCost;

    [Header("Base Costs")]
    public int BaseTreeQuantityCost;
    public int BaseFruitQuantityCost;
    public int BaseFruitQualityCost;
    
    public int BaseHarvesterQuantityCost;
    public int BaseHarvesterSpeedCost;

    [HideInInspector] public int MaxHarvesterSpeed = 30;

    public int BaseValue;

    public int NumberOfUpgrades()
    {
        return TreeQuantity + FruitQuantity + FruitQuality + HarvesterQuantity + HarvesterSpeed;
    }

    public void IncreaseTreeQuantity()
    {
        TreeQuantity++;
    }

    public void IncreaseFruitQuantity()
    {
        FruitQuantity++;
    }

    public void IncreaseFruitQuality()
    {
        FruitQuality++;
    }

    public void IncreaseHarvesterAmount()
    {
        HarvesterQuantity++;
    }

    public void IncreaseHarvesterSpeed()
    {
        HarvesterSpeed++;
    }
    
}
