using UnityEngine;

public class ItemDropController : MonoBehaviour
{
    [SerializeField] Inventory _inventory;
    [SerializeField] float _dropDistance = 4f;

    public bool DropItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= _inventory.Slots.Count)
        {
            return false;
        }

        InventorySlot slot = _inventory.Slots[slotIndex];

        if (slot.IsEmpty)
            return false;

        ItemData itemData = slot.Data;
        int amountToDrop = slot.Amount;

        Vector3 dropPosition = PlayerContext.Current.GetDropPosition(_dropDistance);

        GameObject obj = Instantiate(itemData.Prefab, dropPosition, Quaternion.identity);
        WorldItem worldItem = obj.GetComponent<WorldItem>();
        worldItem.Initialize(itemData, amountToDrop);

        _inventory.RemoveItem(itemData, amountToDrop);

        return true;
    }
}