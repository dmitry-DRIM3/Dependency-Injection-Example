using UnityEngine;

[CreateAssetMenu(fileName = "RangeWeaponConfig", menuName = "DIExample/WeaponConfigs/RangeWeaponConfig")]
public class RangeWeaponConfig : ScriptableObject
{
    public float MaxDamage => _maxDamage;
    public float MinDamage => _minDamage;

    [SerializeField] private float _maxDamage;
    [SerializeField] private float _minDamage;
}
