using System;
using System.Collections;
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

    [Header("Popup")]
    [SerializeField] List<PopUpPair> pairs;

    [Header("Siren")]
    [SerializeField] GameObject siren;
    [SerializeField] GameObject shutdownDoor;

    [Header("Controller")]
    ResidentFileController residentFileController;
    TodayEntryListController todayEntryListController;

    [Header("Init Event")]
    private UnityAction OnCompleteInitEvent;

    protected override void Awake()
    {
        base.Awake();
        InitControllers();
    }

    void InitControllers()
    {
        residentFileController = GetComponentInChildren<ResidentFileController>();
        todayEntryListController = GetComponentInChildren<TodayEntryListController>();
    }

    public void OnClickButton(string key)
    {
        var p = pairs.Find((p) => p.key == key);
        p.popUp.SetActive(true);
    }

    public void OnClickDangerButton()
    {
        siren.transform.GetChild(0).gameObject.SetActive(true);

        //shutdowndoor 닫기
        MoveShutDownDoor(0f);
        Debug.Log("Danger Button Click!!");
    }

    public void MoveShutDownDoor(float y)
    {
        Vector2 targetPoint = new Vector2(0, y);
        shutdownDoor.GetComponent<RectTransform>().DOAnchorPos(targetPoint, 0.8f);
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
}