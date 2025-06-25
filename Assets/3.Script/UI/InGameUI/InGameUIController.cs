using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public struct PopUpPair {
    public string key;
    public GameObject popUp;
}

public class InGameUIController : BehaviourSingleton<InGameUIController>
{
    protected override bool IsDontDestroy() => false;

    [Header("Controller")]
    ResidentFolderController residentFileController;
    TodayEntryListController todayEntryListController;

    [Header("Popup")]
    [SerializeField] List<PopUpPair> pairs;
    [SerializeField] TextBoxController textBox;
    [SerializeField] TextBoxController textBox_noise;

    [Header("Init Event")]
    private UnityAction OnCompleteInitEvent;

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        textBox.OnCompletePrintText += OnCompletePrintText;
        textBox_noise.OnCompletePrintText += OnCompletePrintText;
    }

    void OnDisable()
    {
        textBox.OnCompletePrintText -= OnCompletePrintText;
        textBox_noise.OnCompletePrintText -= OnCompletePrintText;
    }

    public void InitControllers(ResidentFolderController residentFileController, TodayEntryListController todayEntryListController)
    {
        this.residentFileController = residentFileController;
        this.todayEntryListController = todayEntryListController;
    }

    public void OnClickButton(string key)
    {
        var p = pairs.Find((p) => p.key == key);
        p.popUp.SetActive(true);
    }

    public void InitUI()
    {
        OnCompleteInitEvent?.Invoke();
    }

    public void RegisterInitEvent(UnityAction action)
    {
        OnCompleteInitEvent += action;
    }

    public void UnregisterInitEvent(UnityAction action)
    {
        OnCompleteInitEvent -= action;
    }

    public void ShowTextBox(Dialog dialog)
    {
        if (textBox.gameObject.activeSelf)
        {
            textBox.SetTextQueue(dialog);
            return;
        }
        textBox.gameObject.SetActive(true);
        textBox.SetTextQueue(dialog);
    }

    public void ShowTextBox_Noise(Dialog dialog)
    {
        if (textBox_noise.gameObject.activeSelf)
        {
            textBox_noise.SetTextQueue(dialog);
            return;
        }
        textBox_noise.gameObject.SetActive(true);
        textBox_noise.SetTextQueue(dialog);
    }

    void OnCompletePrintText(string code)
    {
        if (code.Equals("Tutorial") || code.Equals("Call"))
        {
            // 튜토리얼 종료 로직 수행하기
            InGameManager.I.EndDDDBehaviour();
        }
    }

    public void UpdateProps()
    {

    }
    
    
    [SerializeField] GameObject shutdownDoor;
    
    public void MoveShutDownDoor(float targetY)
    {
        Vector2 targetPoint = new Vector2(0, targetY);
        shutdownDoor.GetComponent<RectTransform>().DOAnchorPos(targetPoint, 0.8f);
    }
}