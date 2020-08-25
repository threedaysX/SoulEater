using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderEvent : MonoBehaviour
{
    public Slider selfSlider;

    private void Start()
    {
        selfSlider = GetComponent<Slider>();
    }

    public void AdjustSliderWithStep(float step)
    {
        selfSlider.value += step;
    }
}
