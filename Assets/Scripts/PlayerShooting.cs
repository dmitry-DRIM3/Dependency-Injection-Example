using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootingRange = 100f;

    private DamageCalculateController _damageCalculateController;
    private Camera _playerCamera;

    public void Inject(DamageCalculateController damageCalculateController)
    {
        _damageCalculateController = damageCalculateController;
    }

    private void Awake()
    {
        _playerCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Ray ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 shootDirection;

        if (Physics.Raycast(ray, out hit, _shootingRange))
        {
            shootDirection = (hit.point - _playerCamera.transform.position).normalized;
        }
        else
        {
            shootDirection = _playerCamera.transform.forward;
        }

        Bullet bullet = Instantiate(_bulletPrefab, _playerCamera.transform.position, Quaternion.LookRotation(shootDirection)).GetComponent<Bullet>(); // It is better to use a pool of objects

        DamageType damageType = RandomSelectDamageType();
        bullet.Init(_damageCalculateController.DamageCalculate(damageType));
    }

    private DamageType RandomSelectDamageType()
    {
        Array values = Enum.GetValues(typeof(DamageType));
        return (DamageType)Random.Range(0, values.Length);
    }
}