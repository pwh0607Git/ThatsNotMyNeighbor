using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Character/Profile")]
public class Profile : ScriptableObject
{
    [Header("Information")]
    public string firstName;                // 성
    public string lastName;               // 이름
    public string id;                       // 주민 번호
    public CharacterType characterType;

    [Header("Sprite")]
    public Sprite profileImage;
    public AudioClip talkClip;
    public AudioClip walkClip;
    
    [Header("Characteristic")]
    public string job;

    [Header("Props")]
    public string expiration;               // 민증 만기일
    public GameObject model;
    public List<string> entryRequestReasons;
    public List<Dialog> dialogs;

    [Header("Position")]
    public Vector2 startPoint = new Vector2(0, -1000f);
    public Vector2 targetPoint = new Vector2(0f, 0f);
    public Vector2 endPoint = new Vector2(0, 1000f);

    [Header("DoppelData")]
    public DoppelData doppelData;
}

[Serializable]
public class Dialog
{
    public string code;                     //대사 출력 고유 코드
    public List<string> msgs;
}