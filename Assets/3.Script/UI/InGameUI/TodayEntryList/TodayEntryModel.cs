using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TodayEntryModel
{
    private List<Profile> todayEntryList;
    public List<Profile> TodayEntryList => todayEntryList;

    public TodayEntryModel(List<Profile> todayEntryList)
    {
        this.todayEntryList = new(todayEntryList);
    }

    public string GetAddress(Profile resident)
    {
        foreach (var d in InGameManager.I.addressDic)
        {
            if (d.Value.residents == null) continue;

            if (d.Value.residents.Count <= 0) continue;

            if (d.Value.residents.Find(m => m == resident))
            {
                return d.Key;
            }
        }
        return "";
    }
}
