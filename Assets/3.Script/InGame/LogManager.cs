using System.IO;
using UnityEngine;

public static class Record
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
        enterDoppelCount = 0;

        RecordManager.DeleteRecord();
    }

    public static void CalculateRank()
    {
        totalPoint = (enterResidentCount * 100) + (capturedDoppelCount * 500) + (enterDoppelCount * -100) + (deadResidentCount * -200);
        Debug.Log($"Total Point : {totalPoint}");
    }

    public static string GetRank()
    {
        string res = "";

        if (totalPoint >= 1000) res = "S";
        else if (totalPoint >= 700 && totalPoint < 1000) res = "A";
        else if (totalPoint >= 400 && totalPoint < 700) res = "B";
        else res = "F";

        return res;
    }
}

public class LevelManager : BehaviourSingleton<LevelManager>
{
    public class LevelData
    {
        public int capturedDoppelCount = 0;
        public int enterResidentCount = 0;
        public int enterDoppelCount = 0;
        public int deadResidentCount = 0;

        public void ResetData()
        {
            capturedDoppelCount = 0;
            enterDoppelCount = 0;
            deadResidentCount = 0;
        }
    }

    public LevelData data{ get; private set;} = new();

    public void WriteLog(CharacterType characterType, DespawnType despawnType)
    {
        if (characterType.Equals(CharacterType.Resident))
        {
            if (despawnType.Equals(DespawnType.Enter))
            {
                data.enterResidentCount++;
            }
            else if (despawnType.Equals(DespawnType.Death))
            {
                data.deadResidentCount++;
            }
        }
        else if (characterType.Equals(CharacterType.Doppel))
        {
            if (despawnType.Equals(DespawnType.Enter))
            {
                data.enterDoppelCount++;
            }
            else if (despawnType.Equals(DespawnType.Death))
            {
                data.capturedDoppelCount++;
            }
        }
    }

    public void AccumulateData()
    {
        Record.capturedDoppelCount += data.capturedDoppelCount;
        Record.deadResidentCount += data.deadResidentCount;
        Record.enterDoppelCount += data.enterDoppelCount;
        Record.enterResidentCount += data.enterResidentCount;

        data.ResetData();
    }


    protected override bool IsDontDestroy() => false;
}

[System.Serializable]
public class GameRecordData
{
    public int level;
    public int capturedDoppelCount;
    public int enterResidentCount;
    public int enterDoppelCount;
    public int deadResidentCount;
}

public static class RecordManager
{
    private static string logFolderPath => Path.Combine(Application.dataPath, "Record");
    private static string logFilePath => Path.Combine(logFolderPath, "EndlessRecord.json");

    public static void SaveRecord(int level)
    {
        Debug.Log($"Save Data Path : {logFolderPath}");
        GameRecordData data = new GameRecordData
        {
            level = level,
            capturedDoppelCount = Record.capturedDoppelCount,
            enterResidentCount = Record.enterResidentCount,
            enterDoppelCount = Record.enterDoppelCount,
            deadResidentCount = Record.deadResidentCount
        };

        string json = JsonUtility.ToJson(data, true);

        if (!File.Exists(logFolderPath))
        {
            Debug.Log("File 생성!");
            Directory.CreateDirectory(logFolderPath);
        }

        File.WriteAllText(logFilePath, json);
        Debug.Log($"Log saved to: {logFilePath}");
    }

    public static GameRecordData LoadRecord()
    {
        if (!File.Exists(logFilePath))
        {
            Debug.LogWarning("Log file not found.");
            return null;
        }

        string json = File.ReadAllText(logFilePath);
        GameRecordData data = JsonUtility.FromJson<GameRecordData>(json);
        Debug.Log($"Log loaded from: {logFilePath}");
        return data;
    }

    public static void DeleteRecord()
    {
        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
            Debug.Log($"Log file deleted: {logFilePath}");
        }
        else
        {
            Debug.Log("No log file to delete.");
        }
    }
}