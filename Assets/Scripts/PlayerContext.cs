using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    public static PlayerContext Current { get; private set; }

    [SerializeField] EquipmentVisual _equipmentVisual;
    public EquipmentVisual EquipmentVisual => _equipmentVisual;

    void Start()
    {
        EquipmentManager.Instance.RegisterPlayer(_equipmentVisual);
    }

    void OnEnable()
    {
        Current = this;
    }

    void OnDisable()
    {
        if (Current == this)
            Current = null;
    }

    public Vector3 GetDropPosition(float distance)
    {
        Transform playerTransform = transform.parent;
        Vector3 dir = playerTransform.right * playerTransform.localScale.x;
        Vector3 dropPos = playerTransform.position + dir * distance;
        return dropPos;
    }
}