using System;

[Serializable]
public class InventorySlot
{
    public ItemData Data { get; private set; }
    public int Amount { get; private set; }

    public bool IsEmpty => Data == null;

    public InventorySlot()
    {
        Clear();
    }

    public InventorySlot(ItemData data, int amount)
    {
        Data = data;
        Amount = amount;
    }

    public void Set(ItemData data, int amount)
    {
        Data = data;
        Amount = amount;
    }

    public void Add(int amount)
    {
        Amount += amount;
    }

    public bool Remove(int amount)
    {
        if (amount > Amount)
            return false;

        Amount -= amount;

        if (Amount <= 0)
            Clear();

        return true;
    }

    public void Clear()
    {
        Data = null;
        Amount = 0;
    }
}