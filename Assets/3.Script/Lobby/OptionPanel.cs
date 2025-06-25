using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    [Header("Slider")]
    [SerializeField] Slider sliderMaster;
    [SerializeField] Slider sliderEffect;

    public float amountMaster;
    public float amountEffect;

    void Start()
    {
        amountMaster = 0f;
        amountEffect = 0f;
    }

    public void OnChangedValueMaster()
    {
        amountMaster = sliderMaster.value;
    }
        
    public void OnChangedValueEffect()
    {
        amountEffect = sliderEffect.value;
    }
}
