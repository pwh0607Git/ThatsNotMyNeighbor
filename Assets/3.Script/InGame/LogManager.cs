using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Log
{
    public static int capturedDoppelCount = 0;
    
    public static int enterResidentCount = 0;
    public static int enterDoppelCount = 0;
    public static int deadResidentCount = 0;
    public static int totalPoint = 0;

    public static void Reset()
    {
        capturedDoppelCount = 0;
        enterDoppelCount = 0;
        deadResidentCount = 0;
    }

    public static void CalculateRank()
    {
        totalPoint = (enterResidentCount * 100) + (capturedDoppelCount * 500) + (enterDoppelCount * -100) + (deadResidentCount * -200);
    }

    public static string GetRank()
    {
        string res = "";

        if (totalPoint >= 1000) res = "S";
        else if (totalPoint >= 700 && totalPoint < 1000) res = "A";
        else if (totalPoint >= 400 && totalPoint < 700) res = "B";
        else if (totalPoint >= 400 && totalPoint < 400) res = "C";
        else res = "F";

        return res;
    }
}

//로그 매니저는 InGame에만 존재.
public class LogManager : BehaviourSingleton<LogManager>
{
    protected override bool IsDontDestroy() => false;
}
