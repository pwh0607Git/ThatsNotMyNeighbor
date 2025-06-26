using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckListPresenter
{
    private readonly CheckListView view;
    private readonly CheckListModel model;

    public CheckListPresenter(CheckListView view, QuestionType[] groupKey)
    {
        this.view = view;
        model = new();
        InitCheckList(groupKey);
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