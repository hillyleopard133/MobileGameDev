using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject mainGame;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject upgradeMenu;
    
    [SerializeField] private GameObject newGameWarning;
    [SerializeField] private Button continueButton;


    public void StartNewGame()
    {
        SaveLoadManager.Instance.StartNewGame();
        EnableContinueButton();
        HideNewGameWarning();
        LaunchGame();
    }

    public void ContinueGame()
    {
        SaveLoadManager.Instance.LoadSaveGame();
        LaunchGame();
    }
    
    public void LaunchGame()
    {
        startMenu.SetActive(false);
        mainGame.SetActive(true);
    }
    
    public void DisableContinueButton()
    {
        continueButton.interactable = false;
    }

    public void EnableContinueButton()
    {
        continueButton.interactable = true;
    }

    public void ActivateNewGameWarning()
    {
        newGameWarning.SetActive(true);
        startMenu.SetActive(false);
    }

    public void CancelNewGame()
    {
        HideNewGameWarning();
        startMenu.SetActive(true);
    }

    public void HideNewGameWarning()
    {
        newGameWarning.SetActive(false);
    }

    public void OpenUpgrades()
    {
        upgradeMenu.SetActive(true);
    }

    public void CloseUpgrades()
    {
        upgradeMenu.SetActive(false);
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void StartMenu()
    {
        startMenu.SetActive(true);
        pauseMenu.SetActive(false);
        mainGame.SetActive(false);
        SaveLoadManager.Instance.SaveGameData();
    }
}
