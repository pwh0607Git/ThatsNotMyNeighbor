using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanelController : MonoBehaviour
{
    [SerializeField] OptionPanel panel;

    public float amountMaster;
    public float amountEffect;

    public void SetActivePanel(bool on)
    {
        panel.gameObject.SetActive(on);
    }

    public void OnClickExitButton()
    {
        SetActivePanel(false);
    }

    public void OnClickCompleteButton()
    {
        
    }
}