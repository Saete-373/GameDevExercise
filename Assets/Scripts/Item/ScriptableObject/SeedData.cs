using UnityEngine;

[CreateAssetMenu(
    fileName = "New Seed",
    menuName = "Item/Seed"
)]
public class SeedData : ItemData
{
    [Header("Seed")]
    [SerializeField] private GameObject _plantPrefab;
    [SerializeField] private float _growTime;

    public GameObject PlantPrefab => _plantPrefab;
    public float GrowTime => _growTime;
}