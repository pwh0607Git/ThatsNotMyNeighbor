using System.Collections.Generic;
using UnityEngine;

public class TodayEntryListController : MonoBehaviour
{
    [SerializeField] GameObject popUp;
    [SerializeField] Transform todayEntryParent;
    [SerializeField] TodayEntryComponent todayListComponent_prefab;

    private List<Profile> todayListResidents;

    void Start()
    {
        todayListResidents = new();
    }

    void OnEnable()
    {
        InGameUIController.I.RegisterInitEvent(InitTodayEntryList);
    }

    void OnDisable()
    {
        InGameUIController.I.UnregisterInitEvent(InitTodayEntryList);
    }

    // 데이터를 받아서 
    public void InitTodayEntryList()
    {
        List<Profile> characters = InGameManager.I.characters;

        for (int i = 0; i < 4; i++)
        {
            TodayEntryComponent component = Instantiate(todayListComponent_prefab, todayEntryParent);

            Profile profile = null;

            // 랜덤 데이터 추출
            do
            {
                int index = Random.Range(0, characters.Count);
                profile = characters[index];
            } while (todayListResidents.Find(v => v == profile));

            if (profile == null) continue;

            todayListResidents.Add(profile);

            //주소 찾기
            string address = "";

            address = SearchAddress(profile);

            component.InitComponent(profile.profileImage, profile.firstName, profile.lastName, address);
        }
    }

    public string SearchAddress(Profile resident)
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

    public void OnClickOpenButton()
    {
        popUp.SetActive(true);
    }

    public void OnClickExitButton()
    {
        popUp.SetActive(false);
    }
}
