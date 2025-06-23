using System;
using System.Collections.Generic;
using UnityEngine;

public enum DoppelType
{
    Apearence_NoneMouth, ID, EntryRequest, TodayEntryList, AtHome
}

[CreateAssetMenu(fileName = "DoppelgangerData", menuName = "Character/DoppelgangerData")]
public class DoppelData : ScriptableObject
{
    public List<DoppelInform> models;

    public List<Dialog> revealDialog;               //정체가 밝혀 졌을 때의 대사.
}

[Serializable]
public class DoppelInform
{
    public DoppelType type;
    public GameObject model;
}