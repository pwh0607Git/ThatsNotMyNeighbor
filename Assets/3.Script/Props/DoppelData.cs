using System;
using System.Collections.Generic;
using UnityEngine;

public enum DoppelApearanceType
{
    ETC = 1,
    NoneMouth,
    NoneLanguage,
    None
}

public enum DoppelType
{
    None,               // 딱히 문제 없음 => 전화로만 확인 가능
    NonePaper,          // 문서 없음
    ForgedID,           // 위증 문서
    ForgedEntryRequest, // 위증 출입증
    Appearance,         // 외모 상의 문제.
    TodayEntryList,
}

[CreateAssetMenu(fileName = "DoppelgangerData", menuName = "Character/DoppelgangerData")]
public class DoppelData : ScriptableObject
{
    public List<DoppelInform> models;
    public AudioClip talkClip;
    public List<Dialog> revealDialog;               //정체가 밝혀 졌을 때의 대사.
}

[Serializable]
public class DoppelInform
{
    public DoppelApearanceType type;
    public GameObject model;
    public string reason;
}