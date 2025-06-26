using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum QuestionType
{
    IDCard, EntryRequest, Appearance, TodayEntryList
}

public class CheckListView : MonoBehaviour
{
    Dictionary<QuestionType, bool> currentChecking  = new();

    public CheckListComponent prefab;
    [SerializeField] Transform groupParent;

    List<CheckListComponent> checkGroups = new();

    public void MakeCheckComponent(QuestionType key)
    {
        CheckListComponent instance = Instantiate(prefab, groupParent);
        instance.InitCheckComponent(key);
        currentChecking.Add(key, false);
        checkGroups.Add(instance);
        instance.OnUpdateToggle += OnToggleCheck;
    }

    public void OnToggleCheck(QuestionType id, bool on)
    {
        Debug.Log($"Type : {id} => {on}");
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

        foreach (var d in currentChecking.ToList())
        {
            currentChecking[d.Key] = false;
        }
    }

    public void OnClickQuestionButton()
    {
        InteractionManager.I.StartQuestion(currentChecking);
        this.gameObject.SetActive(false);
    }

    public void SetActive(bool on) => gameObject.SetActive(on);
}