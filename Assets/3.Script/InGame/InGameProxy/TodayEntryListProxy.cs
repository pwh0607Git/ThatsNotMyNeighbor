using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TodayEntryListProxy : MonoBehaviour,IOnClickButton
{
    public void OnClickOpenButton()
    {
        InGameUIController.I.TodayEntryListPresenter.OnClickOpenButton();
    }

    public void OnClickCloseButton()
    {
        InGameUIController.I.TodayEntryListPresenter.OnClickCloseButton();
    }
}