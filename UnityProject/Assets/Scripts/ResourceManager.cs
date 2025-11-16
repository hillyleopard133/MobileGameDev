using System;
using UnityEngine;
using BayatGames.SaveGameFree;

public class ResourceManager : Singleton<ResourceManager>
{
    public int AppleAmount = 0;
    public int BananaAmount = 0;

    public int CoinAmount = 0;

    private readonly string RESOURCE_DATA = "RESOURCE_DATA";  

    public void SaveResources()
    {
        ResourceData resourceData = new ResourceData();
        
        resourceData.AppleAmount = AppleAmount;
        resourceData.BananaAmount = BananaAmount;
        resourceData.CoinAmount = CoinAmount;
        
        SaveGame.Save(RESOURCE_DATA, resourceData);
    }

    public void LoadResources()
    {
        if (SaveGame.Exists(RESOURCE_DATA))
        {
            ResourceData resourceData = SaveGame.Load<ResourceData>(RESOURCE_DATA);
            
            AppleAmount = resourceData.AppleAmount;
            BananaAmount = resourceData.BananaAmount;
            CoinAmount = resourceData.CoinAmount;
        }
    }

    public void ResetResources()
    {
        AppleAmount = 0;
        BananaAmount = 0;
        CoinAmount = 0;
        
        SaveResources();
    }
}
