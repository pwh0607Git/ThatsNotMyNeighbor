using System.Collections.Generic;
using UnityEngine;

public class FilesTabController : MonoBehaviour
{
    Dictionary<string, GameObject> fileDic = new();

    [SerializeField] ResidentInfoPanel pan;
    
    [SerializeField] int i1, i2;

    public (int, int) currentIndex;

    [SerializeField] GameObject[] files;
    [SerializeField] GameObject currentFile;

    void Start()
    {
        InitFiles();
    }

    public void InitFileDatas()
    {
        Debug.Log($"Init File datas...");

        Dictionary<string, Apartment> dic = new(InGameManager.I.addressDic);

        foreach (var data in dic)
        {
            List<Profile> profiles = new(data.Value.residents);
            Transform layout = fileDic[data.Key].transform.Find("_Layout");

            foreach (Profile p in profiles) {
                ResidentInfoPanel info = Instantiate(pan, layout);
                info.InitInformation(p);
            }
        }
    }

    void InitFiles()
    {
        int i = 0;
        for (int x = 1; x <= i1; x++)
        {
            for (int y = 1; y <= i2; y++)
            {
                string address = $"F{x:D2}-{y:D2}";
                fileDic[address] = files[i];
                fileDic[address].SetActive(false);
                i++;
            }
        }

        //초기에는 F01-01로 세팅.
        currentIndex = (1, 1);
        string key = $"F{currentIndex.Item1:D2}-{currentIndex.Item2:D2}";
        currentFile = fileDic[key];
        currentFile.SetActive(true);
    }

    public void OnClickFileButtonI1(int index)
    {
        currentIndex.Item1 = index;
        currentFile.SetActive(false);

        string key = $"F{currentIndex.Item1:D2}-{currentIndex.Item2:D2}";
        currentFile = fileDic[key];

        currentFile.SetActive(true);
    }

    public void OnClickFileButtonI2(int index)
    {
        currentIndex.Item2 = index;
        currentFile.SetActive(false);

        string key = $"F{currentIndex.Item1:D2}-{currentIndex.Item2:D2}";
        currentFile = fileDic[key];

        currentFile.SetActive(true);
    }
}