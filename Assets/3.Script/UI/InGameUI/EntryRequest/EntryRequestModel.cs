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
        this.reason = "잠시 일이 있어 다녀왔습니다.";

        this.isForged = isForged;

        if (isForged)
            forgedType = ForgedType.Mark;
        else
            forgedType = ForgedType.None;
    }

    public Profile GetProfile() => profile;
}