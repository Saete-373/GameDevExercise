using UnityEngine;

public class EquipmentVisual : MonoBehaviour
{
    [SerializeField] Transform _heldItemPoint;

    GameObject _currentObject;

    public void SetItem(ItemData itemData)
    {
        ClearItem();

        if (itemData == null)
            return;

        if (itemData.EquipPrefab == null)
            return;

        _currentObject = Instantiate(itemData.EquipPrefab, _heldItemPoint);

        _currentObject.transform.localPosition = Vector3.zero;
        _currentObject.transform.localRotation = itemData.EquipPrefab.transform.rotation;
        _currentObject.transform.localScale = Vector3.one;
    }

    public void ClearItem()
    {
        if (_currentObject == null)
            return;

        Destroy(_currentObject);
        _currentObject = null;
    }
}