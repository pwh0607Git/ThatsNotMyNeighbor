using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SignificantType
{
    Face, Voice, IDCard, EntryRequest, TodayList, InHouse,
    NoneAccessory
}

[CreateAssetMenu(fileName = "CharacterSignificant", menuName = "Character/CharacterSignificant")]
public class SignificantRules : ScriptableObject
{
    [Header("도플갱어 전용 특이사항")]
    public SignificantType[] doppelOnly;

    [Header("일반 주민 전용 특이사항")]
    public SignificantType[] residentOnly;
}
