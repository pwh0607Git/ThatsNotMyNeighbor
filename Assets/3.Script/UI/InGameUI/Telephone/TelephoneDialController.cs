using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelephoneDialController : MonoBehaviour
{
    [SerializeField] TelephoneDialView view;
    
    public void OnClickOpenButton() => view.SetActive(true);
    public void OnClickExitButton() => view.SetActive(false);
}