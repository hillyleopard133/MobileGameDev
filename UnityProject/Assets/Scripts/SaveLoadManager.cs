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
    }

    public void SaveGameData()
    {
        ResourceManager.Instance.SaveResources();
        GameManager.Instance.SaveUpgradeData();
    }
    
    public void StartNewGame()
    {
        ResetGameData();
        SaveGame.Save(FIRST_START, false);
    }

    private void ResetGameData()
    {
        ResourceManager.Instance.ResetResources();
        GameManager.Instance.ResetUpgradeData();
    }
    
    public void LoadSaveGame()
    {
        ResourceManager.Instance.LoadResources();
        GameManager.Instance.LoadUpgradeData();
    }
    
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}