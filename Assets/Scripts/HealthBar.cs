using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider _slider;

    PlayerController _player;

    public void SetPlayer(PlayerController player)
    {
        if (_player != null)
        {
            _player.OnHealthChanged -= HandleHealthChanged;
        }

        _player = player;

        if (_player == null)
            return;

        _player.OnHealthChanged += HandleHealthChanged;

        SetMaxHealth(_player.MaxHealth);
        SetHealth(_player.CurrentHealth);
    }

    void HandleHealthChanged(float currentHealth, float maxHealth)
    {
        SetMaxHealth(maxHealth);
        SetHealth(currentHealth);
    }

    void SetMaxHealth(float maxHealth)
    {
        _slider.maxValue = maxHealth;
    }

    void SetHealth(float health)
    {
        _slider.value = health;
    }

    void OnDestroy()
    {
        if (_player != null)
        {
            _player.OnHealthChanged -= HandleHealthChanged;
        }
    }
}