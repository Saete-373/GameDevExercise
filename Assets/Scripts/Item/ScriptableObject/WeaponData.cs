using UnityEngine;

[CreateAssetMenu(
    fileName = "New Weapon",
    menuName = "Item/Weapon"
)]
public class WeaponData : ItemData
{
    [Header("Weapon")]
    [SerializeField] float _minDamage;
    [SerializeField] float _maxDamage;
    [SerializeField] WeaponType _weaponType;

    public float MinDamage => _minDamage;
    public float MaxDamage => _maxDamage;
    public WeaponType WeaponType => _weaponType;

}

public enum WeaponType
{
    Staff
}