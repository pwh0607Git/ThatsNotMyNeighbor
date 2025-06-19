using System.Collections.Generic;
using UnityEngine;

public class TodayEntryListController : MonoBehaviour
{
    [SerializeField] private TodayEntryListView view;
    private TodayEntryListPresenter presenter;

    private List<Profile> todayEntryList;
    public List<Profile> TodayEntryList => todayEntryList;

    void Awake()
    {
        todayEntryList = GetRandomList(InGameManager.I.Characters, 4);
        presenter = new(view, todayEntryList);
    }

    public List<Profile> GetRandomList(List<Profile> chararcters, int count)
    {
        todayEntryList = new();
        
        List<Profile> selected = new();
        while (selected.Count < count)
        {
            var c = chararcters[Random.Range(0, chararcters.Count)];
            if (!selected.Contains(c)) selected.Add(c);
        }

        return selected;
    }
    
    public void OnClickOpenButton() => presenter.OnClickOpenButton();
    public void OnClickExitButton() => presenter.OnClickCloseButton();
}