using System;
using BayatGames.SaveGameFree;
using UnityEngine;

public class RecipeManager : Singleton<RecipeManager>
{
    [SerializeField] private Recipe[] recipes;
    
    [SerializeField] private GameObject recipePrefab;
    [SerializeField] private Transform recipeContainer;

    private Recipe currentRecipe;
    
    private readonly string SELECTED_RECIPE = "SELECTED_RECIPE";  

    private void Start()
    {
        LoadRecipes();
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

    private void UpdateRecipeToggles()
    {
        Debug.Log("UpdateRecipeToggles");
        for (int i = 0; i < recipeContainer.childCount; i++)
        {
            RecipePrefab recipe = recipeContainer.GetChild(i).GetComponent<RecipePrefab>();
            if (recipe.recipe.productName == currentRecipe.productName)
            {
                Debug.Log(recipe.recipe.productName);
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
        UpdateRecipeToggles();
    }

    public void SaveSelectedRecipe()
    {
        SaveGame.Save(SELECTED_RECIPE, currentRecipe);
    }

    public void LoadSelectedRecipe()
    {
        if (SaveGame.Exists(SELECTED_RECIPE))
        {
            currentRecipe = SaveGame.Load<Recipe>(SELECTED_RECIPE);
            Debug.Log("Loading recipe: " + currentRecipe.productName);
        }
        UpdateRecipeToggles();
    }

    public void ResetSelectedRecipe()
    {
        currentRecipe = null;
    }
    
}
