using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    ItemData _equippedItem;

    public ItemData EquippedItem => _equippedItem;
    [SerializeField] WeaponSystem _weaponSystem;
    EquipmentVisual _equipmentVisual;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void RegisterPlayer(EquipmentVisual equipmentVisual)
    {
        _equipmentVisual = equipmentVisual;
    }

    public void Equip(ItemData itemData)
    {
        if (itemData == null)
        {
            Unequip();
            return;
        }

        if (itemData.ItemType != ItemType.Tool &&
            itemData.ItemType != ItemType.Weapon)
        {
            Unequip();
            return;
        }

        _equippedItem = itemData;
        _equipmentVisual?.SetItem(_equippedItem);
    }

    public void Unequip()
    {
        if (_equippedItem == null)
            return;

        _equippedItem = null;
        _equipmentVisual?.ClearItem();
    }

    public void UseEquippedItem(Vector2 direction, GameObject owner)
    {
        if (_equippedItem == null)
            return;

        if (_equippedItem is WeaponData weapon)
        {
            _weaponSystem.Attack(weapon, direction, owner);

            return;
        }

        // Add logic for using tools in the future.
    }

}