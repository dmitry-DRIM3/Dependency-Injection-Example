public class DamageCalculatorSimple : IDamageCalculator
{
    private readonly SimpleWeaponConfig _simpleWeaponConfig;

    public DamageCalculatorSimple(SimpleWeaponConfig simpleWeaponConfig)
    {
        _simpleWeaponConfig = simpleWeaponConfig;
    }

    public float CalculateDamage()
    {
        return _simpleWeaponConfig.Damage;
    }
}
