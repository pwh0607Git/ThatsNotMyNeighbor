using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField] Slider sliderMaster;
    [SerializeField] Slider sliderEffect;

    public float amountMaster { get; private set; }
    public float amountEffect { get; private set; }

    public UnityAction<float> OnChangedMasterVolume;
    public UnityAction<float> OnChangedEffectVolume;

    void Start()
    {
        sliderMaster.value = 0.5f;
        sliderEffect.value = 0.5f;

        amountMaster = sliderMaster.value;
        amountEffect = sliderEffect.value;
    }

    public void SetSlideBar(float amountMaster, float amountEffect)
    {
        sliderMaster.value = amountMaster;
        sliderEffect.value = amountEffect;
    }

    public void OnChangedValueMaster()
    {
        amountMaster = sliderMaster.value;
        OnChangedMasterVolume?.Invoke(sliderMaster.value);
    }

    public void OnChangedValueEffect()
    {
        amountEffect = sliderEffect.value;
        
        OnChangedEffectVolume?.Invoke(sliderEffect.value);
    }
}
