using System;
using System.Linq;


public class IDCardModel
{
    public ForgedType forgedType { get; private set; } = ForgedType.None;
    private Profile profile;
    public bool isForged { get; private set; }

    public void SetModel(Profile profile, bool isForged)
    {
        this.profile = profile;
        this.isForged = isForged;

        if (isForged)
            forgedType = GetRandomForgedType();
        else
            forgedType = ForgedType.None;
    }

    private ForgedType GetRandomForgedType()
    {
        var values = Enum.GetValues(typeof(ForgedType))
                         .Cast<ForgedType>()
                         .Where(t => t != ForgedType.None)
                         .ToList();

        if (values.Count == 0) return ForgedType.None;

        int index = UnityEngine.Random.Range(0, values.Count);
        return values[index];
    }

    public Profile GetProfile() => profile;
}