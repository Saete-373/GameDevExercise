using UnityEngine;

[CreateAssetMenu(
    fileName = "New Tool",
    menuName = "Item/Tool"
)]
public class ToolData : ItemData
{
    [Header("Tool")]
    [SerializeField] private float _damage;

    [SerializeField] private ToolType _toolType;

    public float Damage => _damage;
    public ToolType ToolType => _toolType;
}

public enum ToolType
{
    Axe
}