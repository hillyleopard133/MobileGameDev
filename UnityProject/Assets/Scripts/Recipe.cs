using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    public Ingredient[] ingredients;
    public string productName;
    public int productValue;
    public float craftingTime;
}

[Serializable]
public class Ingredient
{
    public Sprite sprite;
    public int amount;
    public GameManager.FruitTypes fruitType;
}
