using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private Enemy enemy; 
    [SerializeField] private Text healthText;

    private void OnEnable()
    {
        enemy.OnHealthChanged += UpdateText;
    }

    private void OnDisable()
    {
        enemy.OnHealthChanged -= UpdateText;
    }

    private void UpdateText(float health)
    {
        if (enemy != null)
        {
            healthText.text = $"Health: {health}"; 
        }
    }
}
