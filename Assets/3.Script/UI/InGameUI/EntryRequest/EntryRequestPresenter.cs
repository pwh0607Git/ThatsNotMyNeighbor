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

    public void SetEntryRequestModel(Profile profile, string address, List<ForgedType> types = null)
    {
        model.SetModel(profile, address);
        view.SetData(model.profile, model.address, model.reason);
    }

    public void OnClickOpenButton() => view.SetActive(true);
    public void OnClickCloseButton() => view.SetActive(false);
}