using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    [SerializeField] private PlayerManager playerstats;

    public void Start()
    {
        SetMaxHealth(playerstats.GetHealth());
        SetHealth(playerstats.GetHealth());
    }

    private void Update()
    {
        playerstats.GetHealth();
    }


    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
