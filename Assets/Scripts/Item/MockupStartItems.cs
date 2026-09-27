using UnityEngine;

public class MockupStartItems : MonoBehaviour
{
    [SerializeField] Inventory _inventory;
    [SerializeField] ItemData[] items;
    [SerializeField] int[] amounts;


    void Start()
    {
        for (int i = 0; i < items.Length; i++)
        {
            _inventory.AddItem(items[i], amounts[i]);
        }
    }

}
