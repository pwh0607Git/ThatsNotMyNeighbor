using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCaseController : MonoBehaviour
{
    [SerializeField] GameObject case_Close;
    [SerializeField] GameObject case_Open;

    private GameObject currentCaseView;

    private bool isOpen;

    void Start()
    {
        isOpen = false;
        currentCaseView = case_Close;
    }

    public void OnClickOpenCase()
    {
        Debug.Log("Open Case");
        if (isOpen) return;
        isOpen = true;

        currentCaseView.SetActive(false);
        currentCaseView = case_Open;
        currentCaseView.SetActive(true);
    }

    public void OnClickCloseCase()
    {
        Debug.Log("Close Case");
        if (!isOpen) return;
        isOpen = false;

        currentCaseView.SetActive(false);
        currentCaseView = case_Close;
        currentCaseView.SetActive(true);
    }
}