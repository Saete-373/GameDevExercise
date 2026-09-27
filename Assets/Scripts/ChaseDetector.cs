using UnityEngine;

public class ChaseDetector : MonoBehaviour
{
    [SerializeField] SlimeController _slime;
    [SerializeField] LayerMask _targetLayer;

    float _detectionRadius;

    void Start()
    {
        _detectionRadius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsInTargetLayer(other.gameObject))
            return;

        _slime.SetTarget(other.transform);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!IsInTargetLayer(other.gameObject))
            return;

        _slime.ClearTarget();
    }

    bool IsInTargetLayer(GameObject target)
    {
        return (_targetLayer.value & (1 << target.layer)) != 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}