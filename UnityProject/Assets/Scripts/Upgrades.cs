using UnityEngine;

[CreateAssetMenu(fileName = "Upgrades", menuName = "Scriptable Objects/Upgrades")]
public class Upgrades : ScriptableObject
{
    public int TreeQuantity;    //Number of trees in orchard
    public int FruitQuantity;   //Number of fruits per tree per tap
    public int FruitQuality;    //Increase value of fruit products

    public int HarvesterQuantity; //Number of automatic workers
    public int HarvesterSpeed;  //Automation speed

    public int BaseTreeQuantityCost;
    public int BaseFruitQuantityCost;
    public int BaseFruitQualityCost;
    
    public int BaseHarvesterQuantityCost;
    public int BaseHarvesterSpeedCost;

    public int BaseValue;

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


/*
 *Upgrades
 *
 * quality - improve smoothie value
 * tree quantity - improve collection rates
 * fertiliser - improve apples per tree?
 * harvester - automate collecting
 *
 */