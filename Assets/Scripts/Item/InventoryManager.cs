using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] Inventory _inventory;

    public Inventory Inventory => _inventory;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool TryAddItem(ItemData itemData, int amount)
    {
        if (!_inventory.CanAddItem(itemData, amount))
            return false;

        return _inventory.AddItem(itemData, amount);
    }


}