using UnityEngine;

public class RecipeManager : Singleton<RecipeManager>
{
    [SerializeField] private Recipe[] recipes;
    
    [SerializeField] private GameObject recipePrefab;
    [SerializeField] private Transform recipeContainer;

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
}
