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
    CheckListController checkListController;


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
        TryGetComponent(out clips);
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

    public void ShowTextBox(ResidentController resident, Dialog dialog)
    {
        if (textBox.gameObject.activeSelf)
        {
            textBox.SetTextQueue(resident, dialog, resident.profile.talkClip);
            return;
        }
        textBox.gameObject.SetActive(true);
        textBox.SetTextQueue(resident, dialog, resident.profile.talkClip);
    }

    public void ShowTextBox_Noise(ResidentController resident, Dialog dialog)
    {
        if (textBox_noise.gameObject.activeSelf)
        {
            textBox_noise.SetTextQueue(resident, dialog, resident.profile.doppelData.talkClip);
            return;
        }
        textBox_noise.gameObject.SetActive(true);
        textBox_noise.SetTextQueue(resident, dialog, resident.profile.doppelData.talkClip);
    }

    void OnCompletePrintText(string code)
    {
        if (code.Equals("Tutorial") || code.Equals("Call"))
        {
            // 튜토리얼 종료 로직 수행하기
            InGameManager.I.EndDDDBehaviour();
        }
    }
        
    [SerializeField] GameObject shutdownDoor;
    InGameSoundClipContainer clips;

    public void MoveShutDownDoor(float targetY)
    {
        SoundManager.I.SetEffectAudio(clips.windowClip);
        
        Vector2 targetPoint = new Vector2(0, targetY);
        shutdownDoor.GetComponent<RectTransform>().DOAnchorPos(targetPoint, 0.8f);
    }
}