using UnityEngine;

public class Injector : MonoBehaviour
{
    [SerializeField] private PlayerShooting _playerShooting;

    [SerializeField] private RangeWeaponConfig _rangeWeaponConfig;
    [SerializeField] private SimpleWeaponConfig _simpleWeaponConfig;

    private DamageCalculateController _damageCalculateController;

    private void Awake()
    {
        _damageCalculateController = new DamageCalculateController();
        _damageCalculateController.Inject(_simpleWeaponConfig, _rangeWeaponConfig);
        _playerShooting.Inject(_damageCalculateController);
    }
}
