public class DamageCalculateController
{
    private SimpleWeaponConfig _simpleWeaponConfig;
    private RangeWeaponConfig _rangeWeaponConfig;

    public void Inject(SimpleWeaponConfig simpleWeaponConfig, RangeWeaponConfig rangeWeaponConfig)
    {
        _simpleWeaponConfig = simpleWeaponConfig;
        _rangeWeaponConfig = rangeWeaponConfig;
    }

    public float DamageCalculate(DamageType damageType)
    {
        IDamageCalculator damageCalculator = SelectCalculator(damageType);
        return damageCalculator.CalculateDamage();
    }

    private IDamageCalculator SelectCalculator(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Simple:
                return new DamageCalculatorSimple(_simpleWeaponConfig);
            case DamageType.Range:
                return new DamageCalculatorRange(_rangeWeaponConfig);
            default:
                return new DamageCalculatorSimple(_simpleWeaponConfig);
        }
    }

}
