using UnityEngine;
using UnityEngine.InputSystem;

public class TestItem : MonoBehaviour
{
    [SerializeField] Inventory _inventory;
    [SerializeField] ItemData woodData;


    void Update()
    {
        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            _inventory.AddItem(woodData, 10);
        }
        else if (Keyboard.current.numpad2Key.wasPressedThisFrame)
        {
            _inventory.RemoveItem(woodData, 5);
        }
    }
}
