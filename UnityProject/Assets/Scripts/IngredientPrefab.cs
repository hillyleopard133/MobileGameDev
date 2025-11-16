using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientPrefab : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amount;
    
    public void LoadIngredient(Ingredient ingredient)
    {
        image.sprite = ingredient.sprite;
        amount.text = ingredient.amount.ToString();
    }
}
