using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


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

    void Start()
    {
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
    
    public void InitUI(Dictionary<string, Apartment> addressDic, List<Profile> characters)
    {
        Debug.Log(addressDic.Count);
        StartCoroutine(Init_Co(addressDic, characters));
    }

    IEnumerator Init_Co(Dictionary<string, Apartment> addressDic, List<Profile> characters)
    {
        yield return new WaitUntil(() => residentFileController != null && todayEntryListController != null);

        Debug.Log("InitResdientfiles...");
        residentFileController.InitFileDatas();
        todayEntryListController.InitTodayEntryList(characters);
    }
}