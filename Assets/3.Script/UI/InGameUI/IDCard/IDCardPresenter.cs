// 위조 형태
using System;
using System.Linq;

public enum ForgedType
{
    None, Id, Mark, Expiration
}

public class IDCardPresenter
{
    private readonly IDCardView view;
    private readonly IDCardModel model;

    public IDCardPresenter(IDCardView view)
    {
        this.view = view;
        model = new();
    }

    public void SetIDCardModel(Profile profile, bool isForged)
    {
        model.SetModel(profile, isForged);
        view.SetData(model.GetProfile(), model.forgedType);
    }

    public void OnClickOpenButton() => view.SetActive(true);
    public void OnClickCloseButton() => view.SetActive(false);
}