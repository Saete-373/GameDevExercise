using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _amountText;
    [SerializeField] GameObject _highlight;

    int _slotIndex;
    InventoryUI _inventoryUI;
    bool _isEmpty;

    public int SlotIndex => _slotIndex;

    public void Initialize(int index, InventoryUI inventoryUI)
    {
        _slotIndex = index;
        _inventoryUI = inventoryUI;

        SetHighlight(false);
    }

    public void SetSlot(InventorySlot slot)
    {
        if (slot.IsEmpty)
        {
            _isEmpty = true;

            Clear();
            return;
        }

        _isEmpty = false;

        _icon.enabled = true;
        _icon.sprite = slot.Data.Icon;

        _amountText.text = slot.Amount > 1 ? slot.Amount.ToString() : string.Empty;
    }

    public void Clear()
    {
        _isEmpty = true;

        _icon.enabled = false;
        _icon.sprite = null;
        _amountText.text = string.Empty;
    }

    public void SetHighlight(bool active)
    {
        _highlight.SetActive(active);
    }

    public void SetIconVisible(bool visible)
    {
        _icon.enabled = visible;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isEmpty)
            return;

        _inventoryUI.BeginDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _inventoryUI.Drag(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _inventoryUI.EndDrag(this, eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        _inventoryUI.Drop(this);
    }
}