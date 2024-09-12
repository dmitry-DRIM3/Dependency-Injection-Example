using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float CurrentHealth
    {
        get
        {
            return _currentHealth;
        }
        private set
        {
            _currentHealth = value;
            OnHealthChanged.Invoke(_currentHealth);
        }      
    }
    public Action<float> OnHealthChanged;
    
    [SerializeField] private float _maxHealth = 5;
    
    private float _currentHealth;

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        Debug.Log($"Enemy took damage! Current health: {CurrentHealth}");
        
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Start()
    {
        CurrentHealth = _maxHealth;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}