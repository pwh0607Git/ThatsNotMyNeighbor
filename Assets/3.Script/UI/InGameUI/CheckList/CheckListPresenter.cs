using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckListPresenter
{
    private readonly CheckListView view;
    private readonly CheckListModel model;

    public CheckListPresenter(CheckListView view)
    {
        this.view = view;

        model = new();
    }

    public void InitCheckList(QuestionType[] groupKey)
    {
        foreach (var key in groupKey)
        {
            view.MakeCheckComponent(key);
        }
    }


    public void OnClickOpenButton() => view.SetActive(true);
    public void OnClickCloseButton() => view.SetActive(false);
}