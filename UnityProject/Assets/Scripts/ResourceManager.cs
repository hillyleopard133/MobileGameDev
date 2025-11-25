using System;
using UnityEngine;
using BayatGames.SaveGameFree;
using TMPro;

public class ResourceManager : Singleton<ResourceManager>
{
    [HideInInspector] public int AppleAmount = 0;
    [HideInInspector] public int BananaAmount = 0;
    [HideInInspector] public int OrangeAmount = 0;
    [HideInInspector] public int PearAmount = 0;
    [HideInInspector] public int LemonAmount = 0;

    [HideInInspector] public int CoinAmount = 0;

    private readonly string RESOURCE_DATA = "RESOURCE_DATA";
    private readonly string OFFLINE_TIME = "OFFLINE_TIME";

    private long offlineTime;
    private long timeAway;
    
    private void Start()
    {
        LoadOfflineTime();
        AddOfflineResources();
    }
    
    public void AddOfflineResources()
    {
        timeAway = GetCurrentTimestamp() - offlineTime;

        if (timeAway <= 0) return;
        
        Debug.Log(timeAway);
        
    }
    

    private void LoadOfflineTime()
    {
        if (SaveGame.Exists(OFFLINE_TIME))
        {
            offlineTime = SaveGame.Load(OFFLINE_TIME, GetCurrentTimestamp());
        }
    }
    
    private long GetCurrentTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }


    public void AddCoins(int amount)
    {
        CoinAmount += amount;
        UpdateUI();
    }

    public void UseCoins(int amount)
    {
        CoinAmount -= amount;
        UpdateUI();
    }

    public void UseApple(int amount)
    {
        AppleAmount -= amount;
        UpdateUI();
    }

    public void UseBanana(int amount)
    {
        BananaAmount -= amount;
        UpdateUI();
    }

    public void UseOrange(int amount)
    {
        OrangeAmount -= amount;
        UpdateUI();
    }

    public void UsePear(int amount)
    {
        PearAmount -= amount;
        UpdateUI();
    }

    public void UseLemon(int amount)
    {
        LemonAmount -= amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        UIManager.Instance.UpdateFruitUI();
        UIManager.Instance.UpdateCoinAmountUI();
    }

    public void SaveResources()
    {
        ResourceData resourceData = new ResourceData();
        
        resourceData.AppleAmount = AppleAmount;
        resourceData.BananaAmount = BananaAmount;
        resourceData.OrangeAmount = OrangeAmount;
        resourceData.PearAmount = PearAmount;
        resourceData.LemonAmount = LemonAmount;
        resourceData.CoinAmount = CoinAmount;
        
        SaveGame.Save(RESOURCE_DATA, resourceData);
        UpdateUI();
    }

    public void LoadResources()
    {
        if (SaveGame.Exists(RESOURCE_DATA))
        {
            ResourceData resourceData = SaveGame.Load<ResourceData>(RESOURCE_DATA);
            
            AppleAmount = resourceData.AppleAmount;
            BananaAmount = resourceData.BananaAmount;
            OrangeAmount = resourceData.OrangeAmount;
            PearAmount = resourceData.PearAmount;
            LemonAmount = resourceData.LemonAmount;
            CoinAmount = resourceData.CoinAmount;
        }
        UpdateUI();
    }

    public void ResetResources()
    {
        AppleAmount = 0;
        BananaAmount = 0;
        OrangeAmount = 0;
        PearAmount = 0;
        LemonAmount = 0;
        CoinAmount = 0;
        
        SaveResources();
    }
    
    private void OnApplicationQuit()
    {
        SaveGame.Save(OFFLINE_TIME, GetCurrentTimestamp());
    }

}
