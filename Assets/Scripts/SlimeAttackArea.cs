using UnityEngine;

public class SlimeAttackArea : MonoBehaviour
{
    [SerializeField] SlimeController _slime;
    [SerializeField] LayerMask _targetLayer;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsInTargetLayer(collision.gameObject))
            return;

        IDamagable damagable = collision.GetComponent<IDamagable>();

        if (damagable == null)
            return;

        damagable.TakeDamage(_slime.AttackDamage, _slime.gameObject);

        _slime.SetIsHit(true);
    }

    bool IsInTargetLayer(GameObject target)
    {
        return (_targetLayer.value & (1 << target.layer)) != 0;
    }
}