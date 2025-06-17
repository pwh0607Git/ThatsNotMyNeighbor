using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Family", menuName = "Character/Family")]
public class FamilyData : ScriptableObject
{
    public List<Mate> mateList;
}

[Serializable]
public struct Mate
{
    public List<Profile> mates;
}