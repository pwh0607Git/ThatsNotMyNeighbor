using System.Collections.Generic;
using UnityEngine;

public class CheckList : MonoBehaviour
{
    Dictionary<string, bool> currentChecking;

    [SerializeField] CheckGroup prefab;
    [SerializeField] Transform groupParent;
    [SerializeField] string[] groupKey;
    List<CheckGroup> checkGroups;

    void Start()
    {
        currentChecking = new();
        checkGroups = new();
        MakeCheckGroup();
    }

    void MakeCheckGroup()
    {
        foreach (var key in groupKey)
        {
            CheckGroup instance = Instantiate(prefab, groupParent);
            instance.InitCheckComponent(key);
            currentChecking.Add(key, true);
            checkGroups.Add(instance);
            instance.OnUpdateToggle += OnToggleCheck;
        }
    }

    public void OnToggleCheck(string id, bool on)
    {
        currentChecking[id] = on;
    }

    void OnDisable()
    {
        foreach (var group in checkGroups)
        {
            group.OnUpdateToggle -= OnToggleCheck;
        }
    }

    void OnEnable()
    {
        foreach (var group in checkGroups)
        {
            group.OnUpdateToggle += OnToggleCheck;
        }
    }

    public void OnClickExitButton()
    {
        this.gameObject.SetActive(false);
    }

    public void OnClickCompleteButton()
    {
        // 질문 시퀀스 시작하기
        // InGameManager에서 시퀀스 시작!

        this.gameObject.SetActive(false);
    }
}