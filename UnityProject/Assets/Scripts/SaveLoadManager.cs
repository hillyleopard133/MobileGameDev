using System;
using System.Collections;
using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : Singleton<SaveLoadManager>
{ 
    //Instances
    private UIManager uIManager;

    [SerializeField] private bool isFirstTimeStartingGame;
    
    private readonly string FIRST_START = "FIRST_START";
    
    private bool hasStarted = false;
    
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        uIManager = UIManager.Instance;
        
        //TODO add this, run it and then remove it before building game!
        //SaveGame.Delete(FIRST_START);
        
        if (SaveGame.Exists(FIRST_START))
        {
            isFirstTimeStartingGame = SaveGame.Load<bool>(FIRST_START);
        }
        if (isFirstTimeStartingGame)
        {
            UIManager.Instance.DisableContinueButton();
        }
        
        hasStarted = true;
        
        //LoadSaveGame();
    }

    public void SaveGameData()
    {
        ResourceManager.Instance.SaveResources();
        GameManager.Instance.SaveUpgradeData();
        RecipeManager.Instance.SaveSelectedRecipe();
        Debug.Log("Game Saved");
    }
    
    public void StartNewGame()
    {
        ResetGameData();
        SaveGame.Save(FIRST_START, false);
        RecipeManager.Instance.LoadSelectedRecipe();
        Debug.Log("New Game Started");
    }

    private void ResetGameData()
    {
        ResourceManager.Instance.ResetResources();
        GameManager.Instance.ResetUpgradeData();
        RecipeManager.Instance.ResetSelectedRecipe();
        Debug.Log("Game Reset");
    }
    
    public void LoadSaveGame()
    {
        ResourceManager.Instance.LoadResources();
        GameManager.Instance.LoadUpgradeData();
        RecipeManager.Instance.LoadSelectedRecipe();
        Debug.Log("Game Loaded");
    }
    
    private void OnApplicationQuit()
    {
        SaveGameData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (!hasStarted) return;
        if (pause) SaveGameData();
    }
}