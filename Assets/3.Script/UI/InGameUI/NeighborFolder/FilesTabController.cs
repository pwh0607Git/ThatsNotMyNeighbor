using System.Collections.Generic;
using UnityEngine;

public class FilesTabController : MonoBehaviour
{
    Dictionary<(int, int), GameObject> fileDic = new();

    [SerializeField] int i1, i2;

    public (int, int) currentIndex;

    [SerializeField] GameObject[] files;
    [SerializeField] GameObject currentFile;

    void Start()
    {
        InitFiles();
    }

    void InitFiles()
    {
        int i = 0;
        for (int x = 0; x < i1; x++)
        {
            for (int y = 0; y < i2; y++)
            {
                fileDic[(x, y)] = files[i];
                fileDic[(x, y)].SetActive(false);
                i++;
            }
        }

        //초기에는 F01-01로 세팅.
        currentIndex = (0, 0);
        currentFile = fileDic[currentIndex];
        currentFile.SetActive(true);
    }

    public void OnClickFileButtonI1(int index)
    {
        currentIndex.Item1 = index;
        currentFile.SetActive(false);
        currentFile = fileDic[currentIndex];
        currentFile.SetActive(true);
    }

    public void OnClickFileButtonI2(int index)
    {
        currentIndex.Item2 = index;
        currentFile.SetActive(false);
        currentFile = fileDic[currentIndex];
        currentFile.SetActive(true);
    }

    public void OnClickExitButton()
    {
        this.gameObject.SetActive(false);
    }
}
