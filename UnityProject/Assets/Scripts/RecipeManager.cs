using System;
using BayatGames.SaveGameFree;
using UnityEngine;

public class RecipeManager : Singleton<RecipeManager>
{
    [SerializeField] private Recipe[] recipes;
    
    [SerializeField] private GameObject recipePrefab;
    [SerializeField] private Transform recipeContainer;

    private Recipe currentRecipe;
    private int currentRecipeIndex;
    private ResourceManager resourceManager;
    private Prestige prestigeManager;
    
    private const string SELECTED_RECIPE = "SELECTED_RECIPE";

    private float smoothieCounter = 0;

    private void Start()
    {
        LoadRecipes();
        prestigeManager = Prestige.Instance;
        resourceManager = ResourceManager.Instance;
    }

    private void Update()
    {
        if(currentRecipe is null) return;
        if(!CheckIngredients()) return;
        
        smoothieCounter -= Time.deltaTime;

        if (smoothieCounter <= 0)
        {
            smoothieCounter = currentRecipe.baseCraftingTime * (1 - prestigeManager.blenderSpeed * 0.02f);
            CraftSmoothie();
        }
    }

    private bool CheckIngredients()
    {
        foreach (Ingredient ingredient in currentRecipe.ingredients)
        {
            switch (ingredient.fruitType)
            {
                case GameManager.FruitTypes.Apple:
                    if (resourceManager.AppleAmount < ingredient.amount)
                    {
                        return false;
                    }
                    break;
                
                case GameManager.FruitTypes.Banana:
                    if (resourceManager.BananaAmount < ingredient.amount)
                    {
                        return false;
                    }
                    break;
            }
        }
        return true;
    }

    private void CraftSmoothie()
    {
        resourceManager.AddCoins(currentRecipe.GetProductValue());

        foreach (Ingredient ingredient in currentRecipe.ingredients)
        {
            switch (ingredient.fruitType)
            {
                case GameManager.FruitTypes.Apple:
                    resourceManager.UseApple(ingredient.amount);
                    break;
                
                case GameManager.FruitTypes.Banana:
                    resourceManager.UseBanana(ingredient.amount);
                    break;
            }
        }
        
        Debug.Log("Smoothie Crafted");
    }

    public void LoadRecipes()
    {
        if (recipeContainer.childCount > 0)
        {
            for (int i = 0; i < recipeContainer.childCount; i++)
            {
                Destroy(recipeContainer.GetChild(i).gameObject);
            }
        }
        
        foreach (Recipe recipe in recipes)
        {
            GameObject recipeObject = Instantiate(recipePrefab, recipeContainer);
            recipeObject.GetComponent<RecipePrefab>().LoadRecipe(recipe);
        }
    }

    public void UpdateRecipeTimes()
    {
        for (int i = 0; i < recipeContainer.childCount; i++)
        {
            RecipePrefab recipe = recipeContainer.GetChild(i).GetComponent<RecipePrefab>();
            recipe.UpdateCraftingTimeText();
        }
    }

    private void UpdateRecipeToggles()
    {
        for (int i = 0; i < recipeContainer.childCount; i++)
        {
            RecipePrefab recipe = recipeContainer.GetChild(i).GetComponent<RecipePrefab>();
            if (recipe.recipe.productName == currentRecipe.productName)
            {
                Debug.Log("Selected recipe: " + recipe.recipe.productName);
                recipe.ToggleOn();
            }
            else
            {
                recipe.ToggleOff();
            }
        }
    }
    
    public void SelectRecipe(Recipe recipe)
    {
        currentRecipe = recipe;
        currentRecipeIndex = Array.IndexOf(recipes, currentRecipe);
        UpdateRecipeToggles();
        smoothieCounter = recipe.baseCraftingTime;
        SaveSelectedRecipe();
    }

    public void SaveSelectedRecipe()
    {
        SaveGame.Save(SELECTED_RECIPE, currentRecipeIndex);
    }

    public void LoadSelectedRecipe()
    {
        if (SaveGame.Exists(SELECTED_RECIPE))
        {
            currentRecipeIndex = SaveGame.Load<int>(SELECTED_RECIPE);
            currentRecipe = recipes[currentRecipeIndex];
        }
        else
        {
            currentRecipe = recipes[0];
        }
        UpdateRecipeToggles();
    }

    public void ResetSelectedRecipe()
    {
        currentRecipe = recipes[0];
    }
    
}
