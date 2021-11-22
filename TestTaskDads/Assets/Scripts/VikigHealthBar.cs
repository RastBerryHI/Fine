using UnityEngine.UI;
using UnityEngine;

public class VikigHealthBar : MonoBehaviour
{
    public Slider _slider;

    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
        _slider.value = health;
    }
    public void SetHealth(float health)
    {
        _slider.value = health;
    }
}
