using TMPro;
using UnityEngine;

public class RecipePrefab : MonoBehaviour
{
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private Transform ingredientContainer;
    
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private TextMeshProUGUI recipeValueText;
    [SerializeField] private TextMeshProUGUI craftingTimeText;

    public void LoadRecipe(Recipe recipe)
    {
        recipeNameText.text = recipe.productName;
        recipeValueText.text = recipe.productValue.ToString();
        craftingTimeText.text = recipe.craftingTime + "s";
        
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
}
