using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] Inventory _inventory;
    [SerializeField] InventorySlotUI[] _slotUIs;
    [SerializeField] DragIconUI _dragIcon;
    [SerializeField] ItemDropController _itemDropController;

    [SerializeField] GameObject _inventoryPanel;
    [SerializeField] GameObject _inventoryPage;
    [SerializeField] GameObject _craftingPage;
    [SerializeField] GameObject _invShadow;
    [SerializeField] GameObject _craftShadow;

    bool _isOpen;

    InventorySlotUI _draggedSlot;

    int _selectedSlotIndex = -1;

    void OnEnable()
    {
        _inventory.OnInventoryChanged += Refresh;
    }

    void Awake()
    {
        for (int i = 0; i < _slotUIs.Length; i++)
        {
            _slotUIs[i].Initialize(i, this);
        }

        _dragIcon.gameObject.SetActive(false);
    }

    void Start()
    {
        Refresh();
    }

    void OnDisable()
    {
        if (_inventory != null)
            _inventory.OnInventoryChanged -= Refresh;
    }

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }

        HandleHotbarInput();
    }

    void ToggleInventory()
    {
        _isOpen = !_isOpen;

        _inventoryPanel.SetActive(_isOpen);

        GameStateManager.Instance.SetState(_isOpen ? GameState.Inventory : GameState.Gameplay);
    }

    void Refresh()
    {
        for (int i = 0; i < _slotUIs.Length; i++)
        {
            if (_slotUIs[i] == null)
                continue;

            if (i >= _inventory.Slots.Count)
            {
                _slotUIs[i].Clear();
                continue;
            }

            _slotUIs[i].SetSlot(_inventory.Slots[i]);
        }
    }

    public void BeginDrag(InventorySlotUI slot)
    {
        _draggedSlot = slot;

        InventorySlot inventorySlot = _inventory.Slots[slot.SlotIndex];

        if (inventorySlot.IsEmpty)
            return;

        _dragIcon.SetIcon(inventorySlot.Data.Icon);

        _dragIcon.gameObject.SetActive(true);

        slot.SetIconVisible(false);
    }

    public void Drag(InventorySlotUI slot, PointerEventData eventData)
    {
        if (_draggedSlot == null)
            return;

        RectTransform dragRect = _dragIcon.transform as RectTransform;

        RectTransform canvasRect = _dragIcon.transform.GetComponentInParent<Canvas>().transform as RectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        dragRect.localPosition = localPoint;
    }

    public void Drop(InventorySlotUI targetSlot)
    {
        if (_draggedSlot == null)
            return;

        if (targetSlot == null)
            return;

        _inventory.MoveItem(_draggedSlot.SlotIndex, targetSlot.SlotIndex);
    }

    public void EndDrag(InventorySlotUI slot, PointerEventData eventData)
    {
        if (_draggedSlot == null)
            return;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            _itemDropController.DropItem(_draggedSlot.SlotIndex);
        }

        _dragIcon.gameObject.SetActive(false);
        _draggedSlot = null;

        Refresh();
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= _slotUIs.Length)
            return;

        if (_selectedSlotIndex == index)
            return;

        if (_selectedSlotIndex >= 0)
        {
            _slotUIs[_selectedSlotIndex].SetHighlight(false);
        }

        _selectedSlotIndex = index;
        _slotUIs[_selectedSlotIndex].SetHighlight(true);
        InventorySlot slot = _inventory.Slots[index];

        if (slot.IsEmpty)
        {
            EquipmentManager.Instance.Unequip();
            return;
        }

        EquipmentManager.Instance.Equip(slot.Data);
    }

    void HandleHotbarInput()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            SelectSlot(0);

        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
            SelectSlot(1);

        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
            SelectSlot(2);

        else if (Keyboard.current.digit4Key.wasPressedThisFrame)
            SelectSlot(3);

        else if (Keyboard.current.digit5Key.wasPressedThisFrame)
            SelectSlot(4);

        else if (Keyboard.current.digit6Key.wasPressedThisFrame)
            SelectSlot(5);

        else if (Keyboard.current.digit7Key.wasPressedThisFrame)
            SelectSlot(6);

        else if (Keyboard.current.digit8Key.wasPressedThisFrame)
            SelectSlot(7);

        else if (Keyboard.current.digit9Key.wasPressedThisFrame)
            SelectSlot(8);

        else if (Keyboard.current.digit0Key.wasPressedThisFrame)
            SelectSlot(9);
    }


    public void OpenInventoryPage()
    {
        ShowInventoryPage(true);
    }

    public void OpenCraftingPage()
    {
        ShowInventoryPage(false);
    }

    void ShowInventoryPage(bool isInventoryPage)
    {
        _inventoryPage.SetActive(isInventoryPage);
        _invShadow.SetActive(!isInventoryPage);
        _craftingPage.SetActive(!isInventoryPage);
        _craftShadow.SetActive(isInventoryPage);
    }
}