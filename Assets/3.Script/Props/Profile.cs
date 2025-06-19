using System;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType { RESIDENT, DOPPELGANGER }

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Character/Profile")]
public class Profile : ScriptableObject
{
    [Header("Information")]
    public string firstName;                // 성
    public string lastName;               // 이름
    public string id;                       // 주민 번호

    [Header("Sprite")]
    public Sprite profileImage;

    [Header("Characteristic")]
    public List<string> characteristics;    // 특징
    public string job;

    [Header("Props")]
    public string expiration;               // 민증 만기일
    public GameObject model;
    public List<Dialog> dialogs;
    public List<string> entryRequestReasons;

    [Header("Position")]
    public Vector2 startPoint = new Vector2(0, -1000f);
    public Vector2 targetPoint = new Vector2(0f, 0f);
    public Vector2 endPoint = new Vector2(0, 1000f);
}

[Serializable]
public class Dialog
{
    public string code;                     //대사 출력 고유 코드
    public List<string> msgs;
}