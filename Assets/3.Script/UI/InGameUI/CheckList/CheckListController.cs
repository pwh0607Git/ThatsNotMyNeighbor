using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckListController : MonoBehaviour
{
    [SerializeField] CheckListView view;
    [SerializeField] QuestionType[] groupKey;
    public CheckListPresenter presenter;

    void Start()
    {
        presenter = new(view);

        InitView();
    }

    void InitView()
    {
        presenter.InitCheckList(groupKey);
    }

    public void OnClickOpenButton() => presenter.OnClickOpenButton();
    public void OnClickExitButton() => presenter.OnClickCloseButton();
}
