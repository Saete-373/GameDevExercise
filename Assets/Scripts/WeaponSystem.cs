using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public void Attack(WeaponData weapon, Vector2 direction, GameObject owner)
    {
        switch (weapon.WeaponType)
        {
            case WeaponType.Staff:
                AttackStaff(weapon, direction, owner);
                break;

                // And more in the future.
        }
    }

    void AttackStaff(WeaponData weapon, Vector2 direction, GameObject owner)
    {
        MagicProjectile projectile = MagicProjectilePool.Instance.Get();

        projectile.transform.position = owner.transform.position;

        float damage = Random.Range(weapon.MinDamage, weapon.MaxDamage);

        projectile.Launch(direction, damage, owner);
    }

}