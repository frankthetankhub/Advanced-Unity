using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider slider;
    public Text healthText;
    [SerializeField]public Gradient healthbarColor;
    public Image fill;
    public Unit currentUnit;
    void Start()
    {
        slider = GetComponent<Slider>();
        healthText = transform.GetComponentInChildren<Text>();
        fill = GetComponentInChildren<Image>();
        SetMaxHealth(GameManager.instance.selectedUnit.health);
    }

    // Update is called once per frame
    public void SetHealth(int health)
    {
        slider.value = health;
        healthText.text = health + "/" + slider.maxValue;
        fill.color = healthbarColor.Evaluate(slider.value/slider.maxValue);
        //Canvas.ForceUpdateCanvases();
    }

    public void SetMaxHealth(int val)
    {
        slider.maxValue = val;
        slider.value = val;
        healthText.text = val + "/" + val;
        fill.color = healthbarColor.Evaluate(slider.value/slider.maxValue);
    }
}
