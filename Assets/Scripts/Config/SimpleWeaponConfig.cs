using UnityEngine;

[CreateAssetMenu(fileName = "SimpleWeaponConfig", menuName = "DIExample/WeaponConfigs/SimpleWeaponConfig")]
public class SimpleWeaponConfig : ScriptableObject
{
    public float Damage => _damage;

    [SerializeField] private float _damage;
}