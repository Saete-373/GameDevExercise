using UnityEngine;

public class CraftUI : MonoBehaviour
{
    [Header("Recipe Slots")]
    [SerializeField] RecipeSlotUI[] _recipeSlots;

    [Header("Recipes")]
    [SerializeField] CraftRecipe[] _recipes;

    [Header("Recipe Detail")]
    [SerializeField] RecipeDetailUI _recipeDetail;

    void Start()
    {
        InitializeRecipeSlots();
        LoadRecipes();
    }

    void InitializeRecipeSlots()
    {
        foreach (RecipeSlotUI slot in _recipeSlots)
        {
            slot.Initialize(this);
        }
    }

    void LoadRecipes()
    {
        for (int i = 0; i < _recipeSlots.Length; i++)
        {
            if (i < _recipes.Length)
            {
                CraftRecipe recipe = _recipes[i];

                if (recipe != null && recipe.Result != null)
                {
                    _recipeSlots[i].SetSlot(recipe.Result);
                    continue;
                }
            }

            _recipeSlots[i].Clear();
        }
    }

    public void SelectRecipe(ItemData itemData)
    {
        CraftRecipe recipe = GetRecipe(itemData);

        if (recipe == null)
            return;

        _recipeDetail.SetRecipe(recipe);
    }

    CraftRecipe GetRecipe(ItemData itemData)
    {
        if (itemData == null)
            return null;

        foreach (CraftRecipe recipe in _recipes)
        {
            if (recipe == null)
                continue;

            if (recipe.Result == itemData)
                return recipe;
        }

        return null;
    }
}