using UnityEngine;

public class CraftManager : MonoBehaviour
{
    public static CraftManager Instance { get; private set; }

    Inventory Inventory => InventoryManager.Instance.Inventory;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool CanCraft(CraftRecipe recipe)
    {
        if (recipe == null)
            return false;

        foreach (Ingredient ingredient in recipe.Ingredients)
        {
            if (!Inventory.HasItem(
                    ingredient.Item,
                    ingredient.Amount))
            {
                return false;
            }
        }

        return true;
    }

    public bool Craft(CraftRecipe recipe)
    {
        if (!CanCraft(recipe))
            return false;

        if (!Inventory.CanAddItem(
                recipe.Result,
                recipe.ResultAmount))
        {
            return false;
        }

        foreach (Ingredient ingredient in recipe.Ingredients)
        {
            Inventory.RemoveItem(
                ingredient.Item,
                ingredient.Amount);
        }

        Inventory.AddItem(
            recipe.Result,
            recipe.ResultAmount);

        return true;
    }
}