
using System.Collections.Generic;

public class TodayEntryListPresenter
{
    private readonly TodayEntryListView view;
    private readonly TodayEntryModel model;
    private List<Profile> todayEntryList;

    public List<Profile> TodayEntryList => todayEntryList;

    public TodayEntryListPresenter(TodayEntryListView view, List<Profile> todayEntryList)
    {
        model = new(todayEntryList);

        this.view = view;

        // InGameUIController가 InGameManager가 초기화 작업을 완료되었을 때 실행하도록 하는 이벤트 리스너
        InGameUIController.I.RegisterInitEvent(Init);
    }

    public void Init()
    {
        view.Clear();

        var totalCharacter = InGameManager.I.Characters;
        var todayList = model.TodayEntryList;

        foreach (var res in todayList)
        {
            string address = model.GetAddress(res);
            view.CreateEntry(res, address);
        }
    }

    public void OnClickOpenButton() => view.SetActive(true);
    public void OnClickCloseButton() => view.SetActive(false);
}