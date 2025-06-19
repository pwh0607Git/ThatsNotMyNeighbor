using System;
using System.Collections.Generic;
using System.Linq;

// 위조 형태
public enum ForgedType
{
    None, Id, Mark, Expiration
}

public class IDCardModel
{
    public ForgedType forgedType;
    private Profile currentProfile;
    public List<ForgedType> forgedTypes;
    public bool isDoppel;

    public void SetProfile(Profile profile, bool isDoppel)
    {
        this.currentProfile = profile;
        this.isDoppel = isDoppel;

        if (isDoppel)
        {
            
        }
    }

    ForgedType GetRandomType()
    {
        //일단은 한개만...
         // None을 제외한 ForgedType 값 배열
        var validValues = Enum.GetValues(typeof(ForgedType))
                              .Cast<ForgedType>()
                              .ToList();

        // 랜덤 선택
        int index = UnityEngine.Random.Range(0, validValues.Count);
        return validValues[index];
    }
}