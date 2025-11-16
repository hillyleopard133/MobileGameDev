using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject mainGame;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject blenderPanel;
    
    [SerializeField] private GameObject newGameWarning;
    [SerializeField] private Button continueButton;

    [SerializeField] private GameObject[] trees;
    [SerializeField] private TextMeshProUGUI treeTypeText;

    private readonly string[] treeTypes = {"Apples", "Bananas"};

    private int treeNumber = 0;

    
    
    public void OpenBlenderPanel()
    {
        blenderPanel.SetActive(true);
        DeactivateTrees();
    }

    public void CloseBlenderPanel()
    {
        blenderPanel.SetActive(false);
        LoadTree();
    }

    public void NextTree()
    {
        if (treeNumber + 1 < trees.Length)
        {
            treeNumber++;
        }
        else
        {
            treeNumber = 0;
        }
        
        LoadTree();
        Debug.Log("Switch tree");
    }

    public void PreviousTree()
    {
        if (treeNumber - 1 >= 0)
        {
            treeNumber--;
        }
        else
        {
            treeNumber = trees.Length - 1;
        }
        
        LoadTree();
        Debug.Log("Switch tree");
    }

    private void LoadTree()
    {
        for (int i = 0; i < trees.Length; i++)
        {
            if (i == treeNumber)
            {
                trees[i].SetActive(true);
                GameManager.Instance.SetFruit(i);
            }
            else
            {
                trees[i].SetActive(false);
            }
        }
        
        treeTypeText.text = treeTypes[treeNumber];
    }

    public void DeactivateTrees()
    {
        foreach (GameObject tree in trees)
        {
            tree.SetActive(false);
        }
    }
    
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
        Debug.Log("Continue Game");
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
