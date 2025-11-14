using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject mainGame;
    [SerializeField] private GameObject pauseMenu;
    
    public void LaunchGame()
    {
        startMenu.SetActive(false);
        mainGame.SetActive(true);
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
