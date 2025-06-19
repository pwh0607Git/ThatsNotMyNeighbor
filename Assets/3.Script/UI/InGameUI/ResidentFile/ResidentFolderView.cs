using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResidentFolderView : MonoBehaviour
{
    Dictionary<string, GameObject> fileDic = new();

    [SerializeField] ResidentInfoPanel prefab;

    [SerializeField] int i1, i2;

    public (int, int) currentIndex;

    [SerializeField] GameObject[] files;
    [SerializeField] GameObject currentFile;
    [SerializeField] TextMeshProUGUI telephoneNumber;

    public void SetFileIndex()
    {
        Debug.Log("FileTabController : initFiles");
        int i = 0;
        for (int x = 1; x <= i1; x++)
        {
            for (int y = 1; y <= i2; y++)
            {
                string address = $"F{y:D2}-{x:D2}";
                fileDic[address] = files[i];
                fileDic[address].SetActive(false);
                i++;
            }
        }
    }

    public void CreateEntry(Profile profile, string address)
    {
        Transform layout = fileDic[address].transform.Find("_Layout");

        ResidentInfoPanel comp = Instantiate(prefab, layout);
        comp.InitInformation(profile);
    }


    public void Init()
    {
        //초기에는 F01-01로 세팅.
        currentIndex = (1, 1);
        string key = $"F{currentIndex.Item1:D2}-{currentIndex.Item2:D2}";
        currentFile = fileDic[key];
        currentFile.SetActive(true);

        //파일들을 초기화 한 다음 모두 비활성화 => 초기화를 위해.
        foreach (var file in files)
        {
            if (file.Equals(currentFile)) continue;
            file.SetActive(false);
        }

        this.gameObject.SetActive(false);
    }

    public void SetActive(bool on)
    {
        this.gameObject.SetActive(on);
    }
        
    // 호 수
    public void OnClickFileButtonI1(int index)
    {
        currentIndex.Item2 = index;
        currentFile.SetActive(false);

        string key = $"F{currentIndex.Item1:D2}-{currentIndex.Item2:D2}";
        currentFile = fileDic[key];
        currentFile.SetActive(true);

        telephoneNumber.text = InGameManager.I.addressDic[key].telephoneNum;
    }

    // 층 번호
    public void OnClickFileButtonI2(int index)
    {
        currentIndex.Item1 = index;
        currentFile.SetActive(false);

        string key = $"F{currentIndex.Item1:D2}-{currentIndex.Item2:D2}";
        currentFile = fileDic[key];
        currentFile.SetActive(true);

        telephoneNumber.text = InGameManager.I.addressDic[key].telephoneNum;
    }
}