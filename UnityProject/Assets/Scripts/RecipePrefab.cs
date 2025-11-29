using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipePrefab : MonoBehaviour
{
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private Transform ingredientContainer;
    
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private TextMeshProUGUI recipeValueText;
    [SerializeField] private TextMeshProUGUI craftingTimeText;
    
    [SerializeField] private Toggle recipeSelectedToggle;
    
    [HideInInspector] public Recipe recipe;

    public void LoadRecipe(Recipe recipe)
    {
        this.recipe = recipe;
        recipeNameText.text = recipe.productName;
        recipeValueText.text = recipe.GetProductValue().ToString();
        craftingTimeText.text = recipe.baseCraftingTime + "s";
        
        if (ingredientContainer.childCount > 0)
        {
            for (int i = 0; i < ingredientContainer.childCount; i++)
            {
                Destroy(ingredientContainer.GetChild(i).gameObject);
            }
        }

        foreach (Ingredient ingredient in recipe.ingredients)
        {
            GameObject ingredientObject = Instantiate(ingredientPrefab, ingredientContainer);
            ingredientObject.GetComponent<IngredientPrefab>().LoadIngredient(ingredient);
        }
    }

    public void ToggleOff()
    {
        recipeSelectedToggle.isOn = false;
    }

    public void ToggleOn()
    {
        recipeSelectedToggle.isOn = true;
    }
    
    private void Awake()
    {
        recipeSelectedToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnDestroy()
    {
        recipeSelectedToggle.onValueChanged.RemoveListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        if (!isOn) return;

        RecipeManager.Instance.SelectRecipe(recipe);
    }
    
    
}
