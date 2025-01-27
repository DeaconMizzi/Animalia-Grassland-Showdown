using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public BossController boss;

    void Start()
    {
        healthSlider.maxValue = boss.maxHealth;
        healthSlider.value = boss.maxHealth;
    }

    void Update()
    {
        healthSlider.value = boss.currentHealth;
    }
}
