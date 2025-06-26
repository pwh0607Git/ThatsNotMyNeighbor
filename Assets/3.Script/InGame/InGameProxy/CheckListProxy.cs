using UnityEngine;

public interface IOnClickButton
{
    public void OnClickOpenButton();
    public void OnClickCloseButton();
}

public class CheckListProxy : MonoBehaviour, IOnClickButton
{
    public void OnClickOpenButton()
    {
        InGameUIController.I.CheckListPresenter.OnClickOpenButton();
    }

    public void OnClickCloseButton()
    {
        InGameUIController.I.CheckListPresenter.OnClickCloseButton();
    }
}