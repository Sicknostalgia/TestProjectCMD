using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    private Slider slider;
    public Gradient gradient;
    public Image fill;

    private void OnEnable()
    {
        if (TryGetComponent<Slider>(out slider))
        {
         /*   TryGetComponent<BaseEnemyUnit>(out BaseEnemyUnit en);
            SetMaxHealth(en.health);*/
        }
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
