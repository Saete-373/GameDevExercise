using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] int _slotCount = 50;

    List<InventorySlot> _slots;

    public event Action OnInventoryChanged;

    public IReadOnlyList<InventorySlot> Slots => _slots;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _slots = new List<InventorySlot>(_slotCount);

        for (int i = 0; i < _slotCount; i++)
        {
            _slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(ItemData itemData, int amount)
    {
        if (!CanAddItem(itemData, amount))
            return false;

        int remaining = amount;

        // Fill existing stacks
        foreach (InventorySlot slot in _slots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.Data != itemData)
                continue;

            if (slot.Amount >= itemData.MaxStack)
                continue;

            int space = itemData.MaxStack - slot.Amount;
            int amountToAdd = Mathf.Min(space, remaining);

            slot.Add(amountToAdd);

            remaining -= amountToAdd;

            if (remaining <= 0)
                break;
        }

        // Create new stacks
        if (remaining > 0)
        {
            foreach (InventorySlot slot in _slots)
            {
                if (!slot.IsEmpty)
                    continue;

                int amountToAdd = Mathf.Min(itemData.MaxStack, remaining);

                slot.Set(itemData, amountToAdd);

                remaining -= amountToAdd;

                if (remaining <= 0)
                    break;
            }
        }

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool RemoveItem(ItemData itemData, int amount)
    {
        if (!HasItem(itemData, amount))
            return false;

        int remaining = amount;

        foreach (InventorySlot slot in _slots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.Data != itemData)
                continue;

            int amountToRemove = Mathf.Min(
                slot.Amount,
                remaining
            );

            slot.Remove(amountToRemove);

            remaining -= amountToRemove;

            if (remaining <= 0)
                break;
        }

        OnInventoryChanged?.Invoke();

        return true;
    }

    public bool HasItem(ItemData itemData, int amount)
    {
        if (itemData == null || amount <= 0)
            return false;

        int total = 0;

        foreach (InventorySlot slot in _slots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.Data != itemData)
                continue;

            total += slot.Amount;

            if (total >= amount)
                return true;
        }

        return false;
    }

    public bool CanAddItem(ItemData itemData, int amount)
    {
        if (itemData == null || amount <= 0)
            return false;

        int remaining = amount;

        // Existing stacks
        foreach (InventorySlot slot in _slots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.Data != itemData)
                continue;

            int space = itemData.MaxStack - slot.Amount;

            remaining -= space;

            if (remaining <= 0)
                return true;
        }

        // Empty slots
        foreach (InventorySlot slot in _slots)
        {
            if (!slot.IsEmpty)
                continue;

            remaining -= itemData.MaxStack;

            if (remaining <= 0)
                return true;
        }

        return false;
    }

    public bool MoveItem(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= _slots.Count)
            return false;

        if (toIndex < 0 || toIndex >= _slots.Count)
            return false;

        if (fromIndex == toIndex)
            return false;

        InventorySlot fromSlot = _slots[fromIndex];
        InventorySlot toSlot = _slots[toIndex];

        if (fromSlot.IsEmpty)
            return false;

        // Target is empty → Move
        if (toSlot.IsEmpty)
        {
            toSlot.Set(fromSlot.Data, fromSlot.Amount);
            fromSlot.Clear();

            OnInventoryChanged?.Invoke();
            return true;
        }

        // Same item → Try merge
        if (fromSlot.Data == toSlot.Data)
        {
            int maxStack = fromSlot.Data.MaxStack;
            int space = maxStack - toSlot.Amount;

            if (space <= 0)
                return false;

            int amountToMove = Mathf.Min(fromSlot.Amount, space);

            toSlot.Add(amountToMove);
            fromSlot.Remove(amountToMove);

            OnInventoryChanged?.Invoke();
            return true;
        }

        // Different item → Swap
        ItemData tempData = toSlot.Data;
        int tempAmount = toSlot.Amount;

        toSlot.Set(fromSlot.Data, fromSlot.Amount);
        fromSlot.Set(tempData, tempAmount);

        OnInventoryChanged?.Invoke();

        return true;
    }

    public int GetItemAmount(ItemData itemData)
    {
        if (itemData == null)
            return 0;

        int totalAmount = 0;

        foreach (InventorySlot slot in _slots)
        {
            if (slot.IsEmpty)
                continue;

            if (slot.Data != itemData)
                continue;

            totalAmount += slot.Amount;
        }

        return totalAmount;
    }
}