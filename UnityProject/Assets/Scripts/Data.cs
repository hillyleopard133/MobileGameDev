using System;
using UnityEngine;
using BayatGames.SaveGameFree;

[Serializable]
public class ResourceData
{
    public int AppleAmount;
    public int BananaAmount;
    public int OrangeAmount;
    public int PearAmount;
    public int LemonAmount;

    public int CoinAmount;
}

[Serializable]
public class UpgradeData
{
    public int[] TreeQuantityLevel;
    public int[] FruitQuantityLevel;
    public int[] FruitQualityLevel;
    public int[] HarvestQuantityLevel;
    public int[] HarvestSpeedLevel;
}
