using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("Panels")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject mainGame;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject blenderPanel;
    
    [SerializeField] private GameObject newGameWarning;
    [SerializeField] private Button continueButton;
    
    [Header("Trees")]
    [SerializeField] private GameObject[] trees;
    [SerializeField] private TextMeshProUGUI treeTypeText;

    private readonly string[] treeTypes = {"Apples", "Bananas", "Oranges", "Pears", "Lemons"};

    private int treeNumber = 0;
    
    private ResourceManager resourceManager;
    private GameManager.FruitTypes selectedFruit;
    
    [Header("Upgrades")]
    [SerializeField] private TextMeshProUGUI[] fruitAmountTexts;
    [SerializeField] private TextMeshProUGUI[] upgradeCostTexts;
    [SerializeField] private TextMeshProUGUI[] upgradeLevelTexts;
    
    [SerializeField] private TextMeshProUGUI coinAmountText;
    
    private const int apple = 0;
    private const int banana = 1;
    private const int orange = 2;
    private const int pear = 3;
    private const int lemon = 4;
    
    private const int treeQuantity = 0;
    private const int fruitQuantity = 1;
    private const int fruitQuality = 2;
    private const int harvesterQuantity = 3;
    private const int harvesterSpeed = 4;
    
    private void Start()
    {
        selectedFruit = GameManager.Instance.selectedFruit;
        resourceManager = ResourceManager.Instance;
        UpdateFruitUI();
    }

    public void UpdateFruitUI()
    {
        selectedFruit = GameManager.Instance.selectedFruit;
        switch (selectedFruit)
        {
            case GameManager.FruitTypes.Apple:
                UpdateUpgradeUI(apple);
                break;
            case GameManager.FruitTypes.Banana:
                UpdateUpgradeUI(banana);
                break;
            case GameManager.FruitTypes.Orange:
                UpdateUpgradeUI(orange);
                break;
            case GameManager.FruitTypes.Pear:
                UpdateUpgradeUI(pear);
                break;
            case GameManager.FruitTypes.Lemon:
                UpdateUpgradeUI(lemon);
                break;
        }

        fruitAmountTexts[apple].text = resourceManager.AppleAmount.ToString();
        fruitAmountTexts[banana].text = resourceManager.BananaAmount.ToString();
        fruitAmountTexts[orange].text = resourceManager.OrangeAmount.ToString();
        fruitAmountTexts[pear].text = resourceManager.PearAmount.ToString();
        fruitAmountTexts[lemon].text = resourceManager.LemonAmount.ToString();
    }

    public void UpdateCoinAmountUI()
    {
        coinAmountText.text = resourceManager.CoinAmount.ToString();
    }

    private void UpdateUpgradeUI(int fruitIndex)
    {
        Upgrades upgrade = GameManager.Instance.getFruitUpgrade(fruitIndex);
        
        upgradeLevelTexts[treeQuantity].text = upgrade.TreeQuantity.ToString();
        upgradeLevelTexts[fruitQuantity].text = upgrade.FruitQuantity.ToString();
        upgradeLevelTexts[fruitQuality].text = upgrade.FruitQuality.ToString();
        upgradeLevelTexts[harvesterQuantity].text = upgrade.HarvesterQuantity.ToString();
        upgradeLevelTexts[harvesterSpeed].text = upgrade.HarvesterSpeed.ToString();
        
        upgradeCostTexts[treeQuantity].text = GetUpgradeCost(upgrade.TreeQuantity, upgrade.BaseTreeQuantityCost).ToString();
        upgradeCostTexts[fruitQuantity].text = GetUpgradeCost(upgrade.FruitQuantity, upgrade.BaseFruitQuantityCost).ToString();
        upgradeCostTexts[fruitQuality].text = GetUpgradeCost(upgrade.FruitQuality, upgrade.BaseFruitQualityCost).ToString();
        upgradeCostTexts[harvesterQuantity].text = GetUpgradeCost(upgrade.HarvesterQuantity, upgrade.BaseHarvesterQuantityCost).ToString();
        upgradeCostTexts[harvesterSpeed].text = GetUpgradeCost(upgrade.HarvesterSpeed, upgrade.BaseHarvesterSpeedCost).ToString();
    }
    
    private int GetUpgradeCost(int level, int baseCost)
    {
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level));
    }
    
    private const float costMultiplier = 1.5f;
    
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
