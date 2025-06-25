using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryRequestPresenter
{
    private readonly EntryRequestView view;
    private readonly EntryRequestModel model;

    public EntryRequestPresenter(EntryRequestView view)
    {
        this.view = view;

        model = new();
    }

    public void SetEntryRequestModel(Profile profile, string address, bool isForged)
    {
        model.SetModel(profile, address, isForged);
        view.SetData(model.GetProfile(), model.address, model.reason, model.forgedType);
    }

    public void OnClickOpenButton() => view.SetActive(true);
    public void OnClickCloseButton() => view.SetActive(false);
}