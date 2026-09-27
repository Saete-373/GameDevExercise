using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;

    ItemData _itemData;
    int _amount;

    public ItemData Data => _itemData;
    public int Amount => _amount;

    public void Initialize(ItemData itemData, int amount = 1)
    {
        _itemData = itemData;
        _amount = amount;

        _spriteRenderer.sprite = itemData.WorldSprite;
        ResizeSprite(itemData.WorldSprite, 2f);
    }

    public bool Collect()
    {
        bool success = InventoryManager.Instance.TryAddItem(_itemData, _amount);

        if (!success)
            return false;

        Destroy(gameObject);
        return true;
    }

    public void ResizeSprite(Sprite sprite, float size)
    {
        if (_spriteRenderer.sprite == null)
            return;

        float spriteWidth = sprite.bounds.size.x;
        float spriteHeight = sprite.bounds.size.y;

        float newScaleX = size / spriteWidth;
        float newScaleY = size / spriteHeight;

        _spriteRenderer.transform.localScale = new Vector3(newScaleX, newScaleY, 1f);
    }
}