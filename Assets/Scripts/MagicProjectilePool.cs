using UnityEngine;
using UnityEngine.Pool;

public class MagicProjectilePool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] MagicProjectile _projectilePrefab;
    [SerializeField] int _defaultCapacity = 10;
    [SerializeField] int _maxSize = 30;

    ObjectPool<MagicProjectile> _pool;

    public static MagicProjectilePool Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        _pool = new ObjectPool<MagicProjectile>(
            CreateProjectile,
            OnGetProjectile,
            OnReleaseProjectile,
            OnDestroyProjectile,
            collectionCheck: true,
            defaultCapacity: _defaultCapacity,
            maxSize: _maxSize
        );
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;

        _pool.Clear();
    }

    MagicProjectile CreateProjectile()
    {
        MagicProjectile projectile = Instantiate(
            _projectilePrefab,
            transform
        );

        projectile.SetPool(_pool);

        return projectile;
    }

    void OnGetProjectile(MagicProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    void OnReleaseProjectile(MagicProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    void OnDestroyProjectile(MagicProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    public MagicProjectile Get()
    {
        return _pool.Get();
    }

}