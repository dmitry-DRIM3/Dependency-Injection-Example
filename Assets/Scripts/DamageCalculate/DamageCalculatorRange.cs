using UnityEngine;

public class DamageCalculatorRange : IDamageCalculator
{
    private readonly RangeWeaponConfig _rangeWeaponConfig;

    public DamageCalculatorRange(RangeWeaponConfig rangeWeaponConfig)
    {
        _rangeWeaponConfig = rangeWeaponConfig;
    }

    public float CalculateDamage()
    {
        return Random.Range(_rangeWeaponConfig.MinDamage, _rangeWeaponConfig.MaxDamage);
    }
}
