using UnityEngine;

public enum CharacterType { Resident, Doppelganger }

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Character/Profile")]
public class Profile : ScriptableObject
{
    public string firstName;                // 성
    public string secondName;               // 이름
    public string id;                     // 주민 번호
    public Sprite profileImage;

    public string job;
}