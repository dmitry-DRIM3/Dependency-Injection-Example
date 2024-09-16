# Проект "Dependency Injection Example" 
В этом проекте реализован паттерн Dependency Injection, основанный на простой механике где пользователь имеет возможность стрелять по врагу, нанося разные типы урона.
Прежде чем рассмотреть применение хотелось бы затронуть слова известного разработчика "Martin Fowler" о паттерне.
## Мартин Фаулер (Martin Fowler) о паттерне **Dependency Injection**.
Мартин Фаулер в своей книге **"Patterns of Enterprise Application Architecture"** рассматривает плюсы и минусы паттерна Dependency Injection:

**Плюсы Dependency Injection:**
1. **Уменьшение связанности** (Loose Coupling): Dependency Injection помогает уменьшить связанность между компонентами, так как зависимости внедряются извне, что делает код более гибким и легко тестируемым.
   
2. **Облегчение тестирования** (Easier Testing): Поскольку зависимости внедряются извне, можно легко заменить реальные зависимости на макеты (mock objects) во время тестирования, что делает модульное тестирование более простым.

3. **Улучшение читаемости кода** (Improves Readability): Использование Dependency Injection делает код более явным и понятным, поскольку все зависимости указаны явно при внедрении.

4. **Облегчение конфигурации** (Easier Configuration): Dependency Injection облегчает конфигурацию приложения, так как все зависимости могут быть настроены и внедрены централизованно.

Минусы Dependency Injection:
1. **Усложнение кода** (Adds Complexity): Введение Dependency Injection может увеличить сложность кода, особенно если не используется никакой инструмент для внедрения зависимостей.

2. **Избыточное внедрение** (Over Injection): Использование Dependency Injection может привести к избыточному внедрению зависимостей, если не продумано, какие зависимости действительно нужны для каждого компонента.

3. **Конфигурация и настройка** (Configuration Overhead): Некоторые разработчики могут считать, что настройка и конфигурация Dependency Injection контейнера требует дополнительных усилий и может быть сложной.

В целом, Dependency Injection представляет собой мощный паттерн проектирования, который обладает множеством преимуществ, но необходимо балансировать его использование и учитывать потенциальные недостатки при разработке приложений.

## В проекте присутствуют несколько основных скриптов:
- `PlayerShooting.cs`
- `DamageCalculateController.cs`
- `RangeWeaponConfig.cs`
- `SimpleWeaponConfig.cs`
- `Injector.cs`

С помощью паттерна Dependency Injection решаются проблемы зависимостей между этими скриптами.

## Скрипт PlayerShooting.cs
Этот скрипт нужен для того чтобы делать выстрел в напревлении на ту точку куда пользователь нажал и присвоить пули тип урона.
```csharp
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
```
В этом скрипте есть зависимость с `DamageCalculateController`. Воспользуемся паттерном DI:
```csharp
 public void Inject(DamageCalculateController damageCalculateController)
 {
     _damageCalculateController = damageCalculateController;
 }
```
## Скрипт DamageCalculateController.cs
Рассмотрим как расчитывается урон
```csharp
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
```
В этом скрипте так же есть зависимости с `SimpleWeaponConfig.cs` и `RangeWeaponConfig.cs`. Воспользуемся паттерном DI:
```csharp
public void Inject(SimpleWeaponConfig simpleWeaponConfig, RangeWeaponConfig rangeWeaponConfig)
{
  _simpleWeaponConfig = simpleWeaponConfig;
  _rangeWeaponConfig = rangeWeaponConfig;
}
```
## Рассмотрим разные типы урона скрипты: DamageCalculatorRange.cs и DamageCalculatorSimple.cs
Оба этих скрипта имплементированы от IDamageCalculator следовательно мы объявляем контракт методом "float CalculateDamage();" что и позволяет воплотить паттерн DI 
```csharp
//Реализация метода в DamageCalculatorSimple
public float CalculateDamage()
{
  return _simpleWeaponConfig.Damage;
}
```

```csharp
//Реализация метода в DamageCalculatorRange
 public float CalculateDamage()
    {
        return Random.Range(_rangeWeaponConfig.MinDamage, _rangeWeaponConfig.MaxDamage);
    }
```
Для того что бы удобно было работать с данными различных типов урона я создал для каждого конфиг "ScriptableObject"
```csharp
[CreateAssetMenu(fileName = "RangeWeaponConfig", menuName = "DIExample/WeaponConfigs/RangeWeaponConfig")]
public class RangeWeaponConfig : ScriptableObject
{
    public float MaxDamage => _maxDamage;
    public float MinDamage => _minDamage;

    [SerializeField] private float _maxDamage;
    [SerializeField] private float _minDamage;
}
```
```csharp
[CreateAssetMenu(fileName = "SimpleWeaponConfig", menuName = "DIExample/WeaponConfigs/SimpleWeaponConfig")]
public class SimpleWeaponConfig : ScriptableObject
{
    public float Damage => _damage;

    [SerializeField] private float _damage;
}
```
## Скрипт Injector.cs 
С помощью Injector-а я уменьшаю связанность между компонентами для которых нужны зависимости.
Иницилизируя зависимости централизованно и в нужном мне порядке.
```csharp
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
```
## Заключение 
