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
    [SerializeField] private GameObject prestigePanel;
    
    [SerializeField] private GameObject newGameWarning;
    [SerializeField] private Button continueButton;
    
    [Header("Trees")]
    [SerializeField] private GameObject[] trees;
    [SerializeField] private TextMeshProUGUI treeTypeText;
    
    private const int apple = 0;
    private const int banana = 1;
    private const int orange = 2;
    private const int pear = 3;
    private const int lemon = 4;

    private readonly string[] treeTypes = {"Apples", "Bananas", "Oranges", "Pears", "Lemons"};

    private int treeNumber = 0;
    
    private ResourceManager resourceManager;
    private GameManager.FruitTypes selectedFruit;
    
    [Header("Upgrades")]
    [SerializeField] private TextMeshProUGUI[] upgradeCostTexts;
    [SerializeField] private TextMeshProUGUI[] upgradeLevelTexts;
    
    private const int treeQuantity = 0;
    private const int fruitQuantity = 1;
    private const int fruitQuality = 2;
    private const int harvesterQuantity = 3;
    private const int harvesterSpeed = 4;
    
    [Header("Resources")]
    [SerializeField] private TextMeshProUGUI coinAmountText;
    [SerializeField] private TextMeshProUGUI[] fruitAmountTexts;
    
    [Header("Prestige")]
    [SerializeField] private TextMeshProUGUI[] prestigeCostTexts;
    [SerializeField] private TextMeshProUGUI[] prestigeLevelTexts;
    [SerializeField] private TextMeshProUGUI prestigePointsText;
    [SerializeField] private TextMeshProUGUI prestigeButtonText;

    private const int upgradeDiscount = 0;
    private const int blenderSpeed = 1;

    private Prestige prestigeManager;
    
    private float costMultiplier;
    
    private void Start()
    {
        selectedFruit = GameManager.Instance.selectedFruit;
        resourceManager = ResourceManager.Instance;
        prestigeManager = Prestige.Instance;
        costMultiplier = GameManager.costMultiplier;
        UpdatePrestigeUI();
    }

    public void UpdatePrestigeUI()
    {
        prestigePointsText.text = prestigeManager.prestigePoints.ToString();
        prestigeButtonText.text = "+" + prestigeManager.CalculatePrestigePoints();
        
        prestigeLevelTexts[upgradeDiscount].text = prestigeManager.upgradeDiscount.ToString();
        prestigeLevelTexts[blenderSpeed].text = prestigeManager.blenderSpeed.ToString();
        
        prestigeCostTexts[upgradeDiscount].text = prestigeManager.GetUpgradeCost(prestigeManager.upgradeDiscount, prestigeManager.upgradeDiscountBaseCost).ToString();
        prestigeCostTexts[blenderSpeed].text = prestigeManager.GetUpgradeCost(prestigeManager.blenderSpeed, prestigeManager.blenderSpeedBaseCost).ToString();
        
        UpdateFruitUI();
        RecipeManager.Instance.UpdateRecipeTimes();
    }

    public void UpdateFruitUI()
    {
        selectedFruit = GameManager.Instance.selectedFruit;

        UpdateUpgradeUI((int)selectedFruit);

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
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level) * (1 - prestigeManager.upgradeDiscount * 0.02f));
    }
    
    public void OpenBlenderPanel()
    {
        blenderPanel.SetActive(true);
        ClosePrestigePanel();
        CloseUpgrades();
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
        ClosePrestigePanel();
        CloseBlenderPanel();
    }

    public void CloseUpgrades()
    {
        upgradeMenu.SetActive(false);
    }
    
    public void OpenPrestigePanel()
    {
        prestigePanel.SetActive(true);
        UpdatePrestigeUI();
        CloseUpgrades();
        CloseBlenderPanel();
    }

    public void ClosePrestigePanel()
    {
        prestigePanel.SetActive(false);
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
