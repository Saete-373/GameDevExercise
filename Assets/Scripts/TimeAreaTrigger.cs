using UnityEngine;

public class TimeAreaTrigger : MonoBehaviour
{
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] TimeAreaAction _action;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsInTargetLayer(other.gameObject))
            return;

        if (_action == null)
        {
            Debug.LogWarning($"{name} has no TimeAreaAction assigned.", this);

            return;
        }

        _action.Execute();
    }

    bool IsInTargetLayer(GameObject target)
    {
        return (_targetLayer.value & (1 << target.layer)) != 0;
    }
}