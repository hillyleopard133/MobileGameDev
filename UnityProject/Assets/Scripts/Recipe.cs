using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    public Ingredient[] ingredients;
    public string productName;
    public float baseCraftingTime;

    public int GetProductValue()
    {
        int value = 0;

        foreach (Ingredient ingredient in ingredients)
        {
            value += ingredient.amount * GameManager.Instance.GetFruitValue(ingredient.fruitType);
        }
        
        return value;
    }
}

[Serializable]
public class Ingredient
{
    public Sprite sprite;
    public int amount;
    public GameManager.FruitTypes fruitType;
}
