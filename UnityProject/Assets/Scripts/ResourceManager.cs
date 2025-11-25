using System;
using UnityEngine;
using BayatGames.SaveGameFree;
using TMPro;

public class ResourceManager : Singleton<ResourceManager>
{
    [SerializeField] private TextMeshProUGUI coinAmountText;
    
    [HideInInspector] public int AppleAmount = 0;
    [HideInInspector] public int BananaAmount = 0;

    [HideInInspector] public int CoinAmount = 0;

    private readonly string RESOURCE_DATA = "RESOURCE_DATA";
    private readonly string OFFLINE_TIME = "OFFLINE_TIME";

    private void OnApplicationQuit()
    {
        
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

    public void UpdateUI()
    {
        coinAmountText.text = CoinAmount.ToString();
        GameManager.Instance.UpdateUI();
    }

    public void SaveResources()
    {
        ResourceData resourceData = new ResourceData();
        
        resourceData.AppleAmount = AppleAmount;
        resourceData.BananaAmount = BananaAmount;
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
            CoinAmount = resourceData.CoinAmount;
        }
        UpdateUI();
    }

    public void ResetResources()
    {
        AppleAmount = 0;
        BananaAmount = 0;
        CoinAmount = 0;
        
        SaveResources();
    }
}
