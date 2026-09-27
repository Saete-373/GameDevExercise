using UnityEngine;
using UnityEngine.Pool;

public class MagicProjectile : MonoBehaviour
{
    [SerializeField] float _speed = 15f;
    [SerializeField] float _lifeTime = 2f;
    [SerializeField] LayerMask _targetLayer;
    float _damage;
    GameObject _dmgSource;

    IObjectPool<MagicProjectile> _pool;


    Vector2 _direction;
    float _timer;

    public void SetPool(IObjectPool<MagicProjectile> pool)
    {
        _pool = pool;
    }

    public void Launch(Vector2 direction, float damage, GameObject dmgSource)
    {
        _direction = direction.normalized;
        _timer = _lifeTime;
        _damage = damage;
        _dmgSource = dmgSource;
    }

    void Update()
    {
        transform.position += (Vector3)(_direction * _speed * Time.deltaTime);

        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            Release();
        }
    }

    void Release()
    {
        _pool.Release(this);
    }

    void OnDisable()
    {
        _timer = 0f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsInTargetLayer(other.gameObject))
            return;

        IDamagable damagable = other.GetComponent<IDamagable>();

        if (damagable == null)
            return;

        damagable.TakeDamage(_damage, _dmgSource);

        Release();
    }

    bool IsInTargetLayer(GameObject target)
    {
        return (_targetLayer.value & (1 << target.layer)) != 0;
    }
}