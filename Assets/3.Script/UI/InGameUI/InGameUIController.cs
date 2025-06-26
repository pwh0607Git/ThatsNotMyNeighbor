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

    [Header("View")]
    [SerializeField] ResidentFolderView residentFolderView;
    [SerializeField] TodayEntryListView todayEntryListView;
    [SerializeField] CheckListView checkListView;

    public ResidentFolderPresenter ResidentFolderPresenter{ get; private set; }
    public TodayEntryListPresenter TodayEntryListPresenter{ get; private set; }
    public CheckListPresenter CheckListPresenter{ get; private set; }
    [SerializeField] QuestionType[] groupKey;

    [Header("Popup")]
    [SerializeField] TextBoxController textBox;
    [SerializeField] TextBoxController textBox_noise;

    [Header("Init Event")]
    private UnityAction OnCompleteInitEvent;

    public void InitControllers()
    {
        this.ResidentFolderPresenter = new(residentFolderView);
        this.TodayEntryListPresenter = new(todayEntryListView, InGameManager.I.TodayEntryList);
        this.CheckListPresenter = new(checkListView, groupKey);

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

    public void ShowTextBox(Profile profile, Dialog dialog)
    {
        if (textBox.gameObject.activeSelf)
        {
            textBox.SetTextQueue(null, dialog, profile.talkClip);
            return;
        }
        textBox.gameObject.SetActive(true);
        textBox.SetTextQueue(null, dialog, profile.talkClip);
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

    [SerializeField] GameObject shutdownDoor;
    InGameSoundClipContainer clips;

    public void MoveShutDownDoor(float targetY)
    {
        SoundManager.I.SetEffectAudio(clips.windowClip);

        Vector2 targetPoint = new Vector2(0, targetY);
        shutdownDoor.GetComponent<RectTransform>().DOAnchorPos(targetPoint, 0.8f);
    }

    public void RegisterTextBoxAction(UnityAction action)
    {
        textBox.OnCompletePrintText += action;
    }

    public void UnregisterTextBoxAction(UnityAction action)
    {
        textBox.OnCompletePrintText -= action;
    }
}