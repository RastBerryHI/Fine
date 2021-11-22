using UnityEngine.UI;
using UnityEngine;

public class MutantHealthBar : MonoBehaviour
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

    private void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
