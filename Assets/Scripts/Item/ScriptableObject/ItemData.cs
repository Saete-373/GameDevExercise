using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [Header("Item Information")]
    [SerializeField] string _itemID;
    [SerializeField] string _itemName;
    [SerializeField] Sprite _icon;

    [Header("Inventory")]
    [SerializeField] int _maxStack = 1;

    [Header("Item Type")]
    [SerializeField] ItemType _itemType;

    [Header("World")]
    [SerializeField] GameObject _prefab;
    [SerializeField] Sprite _worldSprite;
    [SerializeField] GameObject _equipPrefab;


    public string ItemID => _itemID;
    public string ItemName => _itemName;
    public Sprite Icon => _icon;
    public int MaxStack => _maxStack;
    public ItemType ItemType => _itemType;
    public GameObject Prefab => _prefab;
    public Sprite WorldSprite => _worldSprite;
    public GameObject EquipPrefab => _equipPrefab;

}