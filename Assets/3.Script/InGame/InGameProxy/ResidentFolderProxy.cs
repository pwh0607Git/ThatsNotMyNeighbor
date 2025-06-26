using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResidentFolderProxy : MonoBehaviour, IOnClickButton
{
    public void OnClickOpenButton()
    {
        InGameUIController.I.ResidentFolderPresenter.OnClickOpenButton();
    }

    public void OnClickCloseButton()
    {
        InGameUIController.I.ResidentFolderPresenter.OnClickCloseButton();
    }
}