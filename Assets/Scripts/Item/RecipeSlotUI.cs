using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeSlotUI : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _name;
    [SerializeField] Sprite _blankIcon;

    ItemData _itemData;
    CraftUI _craftUI;

    public ItemData Data => _itemData;

    public void Initialize(CraftUI craftUI)
    {
        _craftUI = craftUI;
    }

    public void SetSlot(ItemData itemData)
    {
        _itemData = itemData;

        if (itemData == null)
        {
            Clear();
            return;
        }

        _icon.sprite = itemData.Icon;
        _name.text = itemData.ItemName;
    }

    public void Clear()
    {
        _itemData = null;

        _icon.sprite = _blankIcon;
        _name.text = "";
    }

    public void OnClick()
    {
        if (_itemData == null)
            return;

        _craftUI.SelectRecipe(_itemData);
    }
}