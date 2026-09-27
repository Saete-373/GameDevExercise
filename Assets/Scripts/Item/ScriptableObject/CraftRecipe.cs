using System;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Craft Recipe",
    menuName = "Crafting/Recipe"
)]
public class CraftRecipe : ScriptableObject
{
    [SerializeField] private ItemData _result;
    [SerializeField] private int _resultAmount = 1;

    [SerializeField] private Ingredient[] _ingredients;

    public ItemData Result => _result;
    public int ResultAmount => _resultAmount;
    public Ingredient[] Ingredients => _ingredients;
}

[Serializable]
public class Ingredient
{
    public ItemData Item;
    public int Amount;
}