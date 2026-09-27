using UnityEngine;

public class WorldItemCollector : MonoBehaviour
{
    [SerializeField] LayerMask _targetLayer;

    WorldItem _worldItem;

    void Awake()
    {
        _worldItem = GetComponentInParent<WorldItem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsInTargetLayer(other.gameObject))
            return;

        PlayerContext playerContext = other.GetComponentInChildren<PlayerContext>();

        if (playerContext == null)
            return;

        _worldItem.Collect();
    }

    bool IsInTargetLayer(GameObject target)
    {
        return (_targetLayer.value & (1 << target.layer)) != 0;
    }
}