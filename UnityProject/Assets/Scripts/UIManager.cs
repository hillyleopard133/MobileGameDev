using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject mainGame;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject upgradeMenu;
    
    public void LaunchGame()
    {
        startMenu.SetActive(false);
        mainGame.SetActive(true);
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
    }
}
