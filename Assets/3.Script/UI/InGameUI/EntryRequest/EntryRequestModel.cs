using UnityEngine;

public class EntryRequestModel
{
    public ForgedType forgedType { get; private set; } = ForgedType.None;
    private Profile profile;
    public string address;
    public string reason;

    public bool isForged { get; private set; }

    public void SetModel(Profile profile, string address, bool isForged)
    {
        // 이유 reason은 profile에 있는 것 사용하기 => profile에 추가하기.
        this.profile = profile;
        this.address = address;

        int rnd = Random.Range(0, profile.entryRequestReasons.Count);
        this.reason = profile.entryRequestReasons[rnd];

        this.isForged = isForged;

        if (isForged)
            forgedType = ForgedType.Mark;
        else
            forgedType = ForgedType.None;
    }

    public Profile GetProfile() => profile;
}