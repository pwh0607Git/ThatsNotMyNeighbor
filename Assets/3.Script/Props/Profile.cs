using System.Collections.Generic;
using UnityEngine;

public enum CharacterType { RESIDENT, DOPPELGANGER }

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Character/Profile")]
public class Profile : ScriptableObject
{
    [Header("Information")]
    public string firstName;                // 성
    public string secondName;               // 이름
    public string id;                       // 주민 번호

    [Header("Sprite")]
    public Sprite profileImage;

    [Header("Characteristic")]
    public List<string> characteristics;    // 특징
    public string job;

    [Header("Props")]
    public string expiration;               // 민증 만기일
    public GameObject Model;

    [Header("Family..")]                    // 동거인
    public Profile[] mates;
}