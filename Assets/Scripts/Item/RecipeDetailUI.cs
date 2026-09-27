using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeDetailUI : MonoBehaviour
{
    [Header("Result")]
    [SerializeField] Image _resultIcon;
    [SerializeField] TMP_Text _resultName;

    [Header("Ingredients")]
    [SerializeField] IngredientSlotUI[] _ingredientSlots;

    [Header("Craft")]
    [SerializeField] Button _craftButton;

    CraftRecipe _currentRecipe;

    void OnEnable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.Inventory.OnInventoryChanged += Refresh;
        }
    }

    void OnDisable()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.Inventory.OnInventoryChanged -= Refresh;
        }
    }

    void Refresh()
    {
        if (_currentRecipe == null)
            return;

        LoadIngredients(_currentRecipe);
        UpdateCraftButton();
    }

    public void SetRecipe(CraftRecipe recipe)
    {
        _currentRecipe = recipe;

        if (recipe == null)
        {
            Clear();
            return;
        }

        _resultIcon.sprite = recipe.Result.Icon;
        _resultName.text = recipe.Result.ItemName;

        LoadIngredients(recipe);
        UpdateCraftButton();
    }

    void LoadIngredients(CraftRecipe recipe)
    {
        for (int i = 0; i < _ingredientSlots.Length; i++)
        {
            if (i < recipe.Ingredients.Length)
            {
                _ingredientSlots[i].SetIngredient(
                    recipe.Ingredients[i]
                );
            }
            else
            {
                _ingredientSlots[i].Clear();
            }
        }
    }

    void UpdateCraftButton()
    {
        _craftButton.interactable = _currentRecipe != null && CraftManager.Instance.CanCraft(_currentRecipe);
    }

    void Clear()
    {
        _currentRecipe = null;

        _resultIcon.sprite = null;
        _resultName.text = "";

        foreach (IngredientSlotUI slot in _ingredientSlots)
        {
            slot.Clear();
        }

        _craftButton.interactable = false;
    }

    public void OnCraftButton()
    {
        if (_currentRecipe == null)
            return;

        CraftManager.Instance.Craft(_currentRecipe);
    }
}