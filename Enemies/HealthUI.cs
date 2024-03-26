using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider slider;
    public Text currentHP;
    public Text maxHP;

    public void SetMaxHealth(int health)
    {
        maxHP.text = health.ToString();
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        currentHP.text = health.ToString();
        slider.value = health;
    }
}
