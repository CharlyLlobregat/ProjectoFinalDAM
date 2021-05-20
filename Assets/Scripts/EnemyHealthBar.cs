using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {
    public UnityEngine.UI.Slider HealthBar;

    private void Start() {
        this.gameObject.SetActive(SettingsManager.Instance.ShowEnemyHealthBar);
    }

    public void UpdateHealthBar(uint _currentHealth, uint _maxHealth) {
        HealthBar.maxValue = _maxHealth;
        HealthBar.value = _currentHealth;
    }
}
