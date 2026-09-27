using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngredientSlotUI : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _amount;
    [SerializeField] Sprite _blankIcon;

    public void SetIngredient(Ingredient ingredient)
    {
        _icon.sprite = ingredient.Item.Icon;

        int currentAmount = InventoryManager.Instance.Inventory.GetItemAmount(ingredient.Item);

        _amount.text = $"{currentAmount} / {ingredient.Amount}";
    }

    public void Clear()
    {
        _icon.sprite = _blankIcon;
        _amount.text = "";
    }
}