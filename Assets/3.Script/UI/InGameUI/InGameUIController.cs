using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


[Serializable]
public struct PopUpPair {
    public string key;
    public GameObject popUp;
}

public class InGameUIController : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] List<PopUpPair> pairs;

    [Header("Siren")]
    [SerializeField] GameObject siren;
    [SerializeField] GameObject shutdownDoor;

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
}